using Unity.Cinemachine;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    CharacterController characterController;
    AudioSource playerAudioSource;
    GunControls gunControls;
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
    public AudioClip runningAudioClip;
    public AudioClip walkingAudioClip;
    public AudioClip takingPillsAudioClip;


    public PanelScript panelScript;
    Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAudioSource = GetComponent<AudioSource>();
        gunControls = GetComponentInChildren<GunControls>();
        torchControls = GetComponentInChildren<TorchControls>();
        cinemachineBasicMultiChannelPerlin = GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        inventory = Inventory.GetInventory();
        inventory.addObserver(panelScript);
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(GetMovementDirection(out isMoving) * GetCurrentSpeed(out isRunning) * Time.deltaTime);
        RotatePlayer();
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

        inventory.updateFearAndNotifyObservers(Mathf.Clamp(Inventory.GetInventory().fear + Time.deltaTime * 2f, 0f, maxFear));
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
            PlayAudioClip(playerAudioSource, takingPillsAudioClip);
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

    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * 5f * 100f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
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
    /// plays the given audio clip on the given audio source if it is not already playing
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clip"></param>
    private void PlayAudioClip(AudioSource audioSource,  AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// plays movement audio based on whether the player is moving and if they are running or walking
    /// </summary>
    /// <param name="isMoving"></param>
    private void PlayMovementAudio(bool isMoving)
    {
        if (playerAudioSource == null || !isMoving) return;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayAudioClip(playerAudioSource, runningAudioClip);
        }
        else
        {
            PlayAudioClip(playerAudioSource, walkingAudioClip);
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
