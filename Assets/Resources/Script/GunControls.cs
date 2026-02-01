using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GunControls : MonoBehaviour
{
    AudioSource gunAudioSource;
    CinemachineImpulseSource gunImpulseSource;

    RaycastHit hitInfo;

    public GameObject muzzleFlash;
    public GameObject aimAt;

    public GameObject gun;
    public InventoryScriptableObject inventory;
    public PanelScript panelScript;

    [Header("Audio clips")]
    public AudioClip gunShootAudioClip;
    public AudioClip emptyGunShootAudioClip;

    [Header("Event")]
    public EnemyIsHitEvent enemyIsHitEvent;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunAudioSource = GetComponent<AudioSource>();
        gunImpulseSource = GetComponent<CinemachineImpulseSource>();
        inventory.addObserver(panelScript);

        muzzleFlash.SetActive(false);

    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.green);
    }


    /// <summary>
    /// Shoot and decrease ammunition from the inventory
    /// check the rayCast if something was hit
    /// play the gun audio
    /// </summary>
    public void PlayerShoot()
    {
        if (inventory.ammunition > 0)
        {
            inventory.updateAmmunitionsByValueAndNotifyObservers(-1);
            Ray ray = Camera.main.ScreenPointToRay(
                new Vector2(Screen.width / 2f, Screen.height / 2f)
                );
            if (Physics.Raycast(ray, out hitInfo, 100f))
            {   Debug.Log("Hit: " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag.Equals("Enemy"))
                {
                    // enemy was hit
                    Debug.Log("Enemy hit: " + hitInfo.transform.gameObject.name);
                    enemyIsHitEvent.SendEventMessage(hitInfo.transform.gameObject, gameObject);
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

    }
}
