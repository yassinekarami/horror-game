using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private bool isDead = false;
    private bool enemyTouchedPlayer = false;
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
    [SerializeField] private float xRotation = 0f;

    [Header("Fear Settings")]
    [SerializeField] private float lowerMidFear = 20;
    [SerializeField] private float midFear = 50f;
    [SerializeField] private float upperMidFear = 80f;
    [SerializeField] private float maxFear = 100f;


    [Header("Audio clips")]
    public SoundsScriptableObject sounds;


    public PanelScript panelScript;
    public InventoryScriptableObject inventory;
    [Header("Event")]
    public EnemyKilledTargetEvent enemyKilledTargetEvent;
    public LampExplodeAtPositionEvent lampExplodeEventChanel;

    // Start is called once before the first execution of Update after the MonoBehavior is created
    void Start()
    {
        Cursor.visible = false;
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
        if (!enemyTouchedPlayer) return;
        if (isDead) return;
 
        StartCoroutine(PlayerIsDead(value0));

    }

    /// <summary>
    /// Handles player death by playing a death sound, waiting for its duration, and destroying the specified
    /// GameObject.
    /// </summary>
    /// <param name="value0">The GameObject to destroy upon player death.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator PlayerIsDead(GameObject value0) 
    {
        isDead = true;
        AudioClip audioToPlay = sounds.GetAudioClipAtIndex(3);
        sounds.PlayAudioClip(playerAudioSource, audioToPlay);
        yield return new WaitForSeconds(audioToPlay.length +1);
        Destroy(value0);
    }

    /// <summary>
    /// Handles the lamp explosion event by increasing the player's fear level and notifying observers.
    /// </summary>
    /// <param name="position">The position where the lamp explosion occurs.</param>
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
        float speed = updateSpeedRegardingFear(GetCurrentSpeed(out isRunning));
        characterController.Move(GetMovementDirection(out isMoving) * speed * Time.deltaTime);
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
 
        inventory.updateFearAndNotifyObservers(Mathf.Clamp((inventory.fear + Time.deltaTime *4f), 0f, maxFear));
        if (inventory.fear >= maxFear)
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 2.75f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 2.75f + additionalValue;
        }
        if (inventory.fear >= upperMidFear)
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 2.25f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 2.25f + additionalValue;
        }
        else if (inventory.fear >= midFear)
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 2f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 2f + additionalValue;
        }
        else if (inventory.fear >= lowerMidFear)
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 1.5f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 1.5f + additionalValue;
        }
        else
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = 1f;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = 1f + additionalValue;
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
            inventory.updateFearByValueAndNotifyObservers(-50f);
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
        float mouseY = Input.GetAxis("Mouse Y") * 500f * Time.deltaTime;

        // Inverser si nécessaire
        xRotation -= mouseY;

        // Clamp
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Appliquer rotation
        cameraHolder.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        gunHolder.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        torchHolder.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
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
    /// Adjusts the speed based on the current fear level in the inventory.
    /// </summary>
    /// <param name="currentSpeed">The current speed value to be modified.</param>
    /// <returns>The adjusted speed according to the fear thresholds.</returns>
    private float updateSpeedRegardingFear(float currentSpeed)
    {
        if (inventory.fear >= maxFear)
        {
            return currentSpeed * 0.5f;
        }
        else if (inventory.fear >= upperMidFear)
        {
            return currentSpeed * 0.75f;
        }
        else if (inventory.fear >= midFear)
        {
            return currentSpeed * 0.85f;
        }
        else if (inventory.fear >= lowerMidFear)
        {
            return currentSpeed * 0.95f;
        }
        else
        {
            return currentSpeed;
        }
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

    /// <summary>
    /// Handles trigger events by updating the inventory when colliding with a first aid item and marking the player as
    /// touched when colliding with an enemy.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Firstaid"))
        {
            inventory.updateMedicineByValueAndNotifyObservers(1);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            enemyTouchedPlayer = true;
        }
    }

    /// <summary>
    /// Resets the enemyTouchedPlayer flag when a collider exits the trigger.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        enemyTouchedPlayer = false;
    }

}
