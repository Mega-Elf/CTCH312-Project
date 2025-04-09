using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [Header("Sound Method 1")]
    public AudioSource ShootingChannel;
    public AudioSource ReloadingChannel;
    
    // Glock18 sound effects
    public AudioClip Glock18Shot;
    public AudioClip Glock18Reload;
    public AudioSource dryFireSoundGlock18;

    // AK47 sound effects
    public AudioClip AK47Shot;
    public AudioClip AK47Reload;
    public AudioSource dryFireSoundAK47;

    // Player sounds
    public AudioClip playerDamaged;
    public AudioClip playerDeath;
    public AudioClip gameOverMusic;
    public AudioSource PlayerChannel;

    [Header("Sound Method 2")]
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayFiringSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Glock18:
                ShootingChannel.PlayOneShot(Glock18Shot); 
                break;
            case WeaponModel.AK47:
                ShootingChannel.PlayOneShot(AK47Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Glock18:
                ReloadingChannel.PlayOneShot(Glock18Reload);
                break;
            case WeaponModel.AK47:
                ReloadingChannel.PlayOneShot(AK47Reload);
                break;
        }
    }

    public void PlayDryFireSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Glock18:
                dryFireSoundGlock18.Play();
                break;
            case WeaponModel.AK47:
                dryFireSoundAK47.Play();
                break;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // assign the audio clip
        audioSource.clip = audioClip;

        // assign the volume
        audioSource.volume = volume;

        // play the sound
        audioSource.Play();

        // get the length of the sound fx clip
        float clipLength = audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // assign a random index
        int rand = Random.Range(0, audioClip.Length);

        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // assign the audio clip
        audioSource.clip = audioClip[rand];

        // assign the volume
        audioSource.volume = volume;

        // play the sound
        audioSource.Play();

        // get the length of the sound fx clip
        float clipLength = audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
