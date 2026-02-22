using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private bool isDead = false;
    [Header("Components")]
    CharacterController characterController;
    AudioSource playerAudioSource;
    GunControls gunControls;
    GameObject cameraHolder;
    GameObject gunHolder;
    GameObject torchHolder;
    TorchControls torchControls;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;


    [Header("Movement Settings")]
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isRunning;

    [Header("Fear Settings")]
    [SerializeField] private float midFear = 50f;
    [SerializeField] private float maxFear = 100f;


    [Header("Audio clips")]
    public SoundsScriptableObject sounds;


    public PanelScript panelScript;
    public InventoryScriptableObject inventory;
    [Header("Event")]
    public EnemyKilledTargetEvent enemyKilledTargetEvent;
    public LampExplodeAtPositionEvent lampExplodeEventChanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraHolder = GameObject.Find("CameraHolder");
        gunHolder = GameObject.Find("GunHolder");
        torchHolder = GameObject.Find("TorchHolder");
        characterController = GetComponent<CharacterController>();
        playerAudioSource = GetComponent<AudioSource>();

        gunControls = gunHolder.GetComponent<GunControls>();
        torchControls = GetComponentInChildren<TorchControls>();
        cinemachineBasicMultiChannelPerlin = GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        enemyKilledTargetEvent.Event += KillThePlayer;
        lampExplodeEventChanel.Event += OnLampExplode;
        inventory.addObserver(panelScript);
    }

    /// <summary>
    /// Destroys the specified player GameObject and logs the event.
    /// </summary>
    /// <param name="value0">The player GameObject to be destroyed.</param>
    private void KillThePlayer(GameObject value0)
    {
        if (isDead) return;

        StartCoroutine(PlayerIsDead(value0));

    }

    IEnumerator PlayerIsDead(GameObject value0) 
    {
        isDead = true;
        AudioClip audioToPlay = sounds.GetAudioClipAtIndex(3);
        sounds.PlayAudioClip(playerAudioSource, audioToPlay);
        yield return new WaitForSeconds(audioToPlay.length +1);
        Destroy(value0);
    }


    private void OnLampExplode(Vector3 position)
    {
        Debug.Log("Lamp explode event triggered - Test_Event");
        Debug.Log("Current fear " + inventory.fear);

            
        inventory.updateFearAndNotifyObservers(Mathf.Clamp(inventory.fear + 20f, 0f, maxFear));
        Debug.Log("Updated fear " + inventory.fear);
    }


    // Update is called once per frame
    void Update()
    {
        characterController.Move(GetMovementDirection(out isMoving) * GetCurrentSpeed(out isRunning) * Time.deltaTime);
        RotatePlayer();
        LookUpAndDown();
        PlayMovementAudio(isMoving);
        UpdateCameraPerlinAmplitudeAndFrequency(isRunning);
        if (Input.GetMouseButtonDown(0))
        {
            PlayerShoot();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            ToggleTorch();
        }      
        else if (Input.GetKeyDown(KeyCode.F))
        {
            TakeMedicine();
        }
    }

    /// <summary>
    /// Determines the camera shake based on the player's fear level
    /// by updating the amplitude and frequency of the Cinemachine Basic Multi Channel Perlin
    /// </summary>
    /// <param name="isRunning">boolean to determine if the player is running or not</param>
    private void UpdateCameraPerlinAmplitudeAndFrequency(bool isRunning)
    {
        float additionalValue = isRunning ? 0.5f : 0;
        // increase fear over time

        inventory.updateFearAndNotifyObservers(Mathf.Clamp((inventory.fear + Time.deltaTime), 0f, maxFear));
        if (inventory.fear >= maxFear)
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 2f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 2.5f + additionalValue;
        }
        else if (inventory.fear >= midFear)
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 1.5f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 1f + additionalValue;
        }
        else
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 1f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 0.5f + additionalValue;
        }
    }

    /// <summary>
    /// player shooting logic
    /// </summary>
    private void PlayerShoot()
    {
        gunControls.PlayerShoot();
    }

    /// <summary>
    /// Toggles the state of the torch using torchControls.
    /// </summary>
    private void ToggleTorch()
    {
        torchControls.ToggleTorch();
    }
    /// <summary>
    /// Decrease medicine amount in inventory and reduce fear
    /// </summary>
    private void TakeMedicine()
    {
        if (inventory.medicine > 0)
        {
            inventory.updateMedicineByValueAndNotifyObservers(-1);
            sounds.PlayAudioClipAtIndex(playerAudioSource, 2);
            inventory.updateFearByValueAndNotifyObservers(-30f);
        }
    }

    /// <summary>
    /// move the player based on input and return whether they are moving
    /// also update the isMoving variable
    /// </summary>
    /// <param name="isMoving"></param>
    /// <returns></returns>
    private Vector3 GetMovementDirection(out bool isMoving)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        isMoving = (horizontalInput != 0 || verticalInput != 0);
     
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
   
        return transform.TransformDirection(moveDirection.normalized);
    }

    /// <summary>
    /// Rotates the player horizontally based on mouse X-axis input.
    /// </summary>
    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * 5f * 100f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Performs look up and down operations.
    /// </summary>
    private void LookUpAndDown()
    {
        float mouseY = Input.GetAxis("Mouse Y") * 5f * 100f * Time.deltaTime;
        cameraHolder.transform.Rotate(Vector3.left * mouseY);
        gunHolder.transform.Rotate(Vector3.left * mouseY);
        torchHolder.transform.Rotate(Vector3.left * mouseY);
    }
    /// <summary>
    /// Unsubscribes the KillThePlayer handler from the enemyKilledTargetEvent when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        enemyKilledTargetEvent.Event -= KillThePlayer;
        lampExplodeEventChanel.Event -= OnLampExplode;
    }

    /// <summary>
    /// returns the current speed based on whether the player is running or walking
    /// </summary>
    /// <returns>the speed to apply to the movement</returns>
    private float GetCurrentSpeed(out bool isRunning)
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        return isRunning ? runSpeed : walkSpeed;
    }


    /// <summary>
    /// plays movement audio based on whether the player is moving and if they are running or walking
    /// </summary>
    /// <param name="isMoving"></param>
    private void PlayMovementAudio(bool isMoving)
    {
        if (playerAudioSource == null) return;
        else if (!isMoving)
        {
            sounds.StopAudioClip(playerAudioSource);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {   
                sounds.PlayAudioClipAtIndex(playerAudioSource, 0);
            }
            else
            {
                sounds.PlayAudioClipAtIndex(playerAudioSource, 1);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Firstaid"))
        {
            inventory.updateMedicineByValueAndNotifyObservers(1);
            Destroy(other.gameObject);
        }
    }

}
