using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunControls : MonoBehaviour
{
    AudioSource gunAudioSource;
    CinemachineImpulseSource gunImpulseSource;

    RaycastHit hitInfo;
    Vector3 mousePosition;
    Vector3 mouseWorldPosition;

    public GameObject muzzleFlash;
    public GameObject aimAt;


    public PanelScript panelScript;
    private Inventory inventory;

    [Header("Audio clips")]
    public AudioClip gunShootAudioClip;
    public AudioClip emptyGunShootAudioClip;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunAudioSource = GetComponent<AudioSource>();
        gunImpulseSource = GetComponent<CinemachineImpulseSource>();
        inventory = Inventory.GetInventory();
        inventory.addObserver(panelScript);

        muzzleFlash.SetActive(false);
    }

    private void Update()
    {
        SetUpLaserCrossHair();
    }

    /// <summary>
    /// Move the crosshair following the mouse position
    /// </summary>
    private void SetUpLaserCrossHair()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 15f;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        aimAt.transform.position = mouseWorldPosition;
    }


    /// <summary>
    /// Shoot and decrease ammunition from the inventory
    /// check the rayCast if something was hit
    /// play the gun audio
    /// </summary>
    public void PlayerShoot()
    {
        if (Inventory.GetInventory().ammunition > 0)
        {
            inventory.updateAmmunitionsByValueAndNotifyObservers(-1);
            if (Physics.Raycast(transform.position, aimAt.transform.position, out hitInfo, 15f))
            {
                if(hitInfo.transform.gameObject.tag.Equals("Enemy"))
                {
                    // enemy was hit
                }
            }
            PlayAudioClip(gunAudioSource, gunShootAudioClip);
            ScreenShake(transform.up);
        }
        else
        {
            PlayAudioClip(gunAudioSource, emptyGunShootAudioClip);
        }
    }

    /// <summary>
    /// plays the given audio clip on the given audio source if it is not already playing
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clip"></param>
    private void PlayAudioClip(AudioSource audioSource, AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }
    /// <summary>
    /// Shake the screen when the player shoots
    /// it also start a Coroutine to turn on / off the muzzle flash
    /// </summary>
    /// <param name="dir"></param>
    private void ScreenShake(Vector3 dir)
    {

        StartCoroutine(StopMuzzleFlash());
        gunImpulseSource.GenerateImpulseWithForce(0.5f);
    }

    /// <summary>
    /// Coroutine to turn on / off the muzzle flash
    /// </summary>
    /// <returns></returns>
    IEnumerator StopMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(muzzleFlash.GetComponent<ParticleSystem>().main.duration);
        muzzleFlash.SetActive(false);
    }

    /// <summary>
    /// Gizmos for debugging and visualizing purposes
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, aimAt.transform.position);
    }
}
