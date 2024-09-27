using DG.Tweening;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource effectsAudioSource;
    public AudioSource gunLoopAudioSource;
    public AudioSource gunOneShotAudioSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Zombie Death Sounds")]
    public AudioClip zombieRisenDeathSound;
    public AudioClip zombieInfestedDeathSound;
    public AudioClip zombieUndyingDeathSound;
    public AudioClip zombieHulkDeathSound;
    public AudioClip zombieSprinterDeathSound;
    public AudioClip[] randomZombieSounds;

    [Header("Other SFX")]
    public AudioClip playerDeathSound;
    public AudioClip ammoLootSound;
    public AudioClip[] gunShootSounds;
    public AudioClip gunLoopSound;

    public static SoundManager selfInstance;
    [HideInInspector] public Tweener gunLoopStopTween;

    private void Awake()
    {
        selfInstance = this;
    }

    private void Start()
    {
        PlayBackgroundMusic();

        GameEventsManager.selfInstance.onAmmoLooted += PlayAmmoLootSound;
        GameEventsManager.selfInstance.onDeath += PlayCreatureDeathSound;
        GameEventsManager.selfInstance.onGunShot += PlayGunShotSound;
        GameEventsManager.selfInstance.onGunStartStopFiring += PlayGunLoop;
    }

    private void PlayCreatureDeathSound(Character deadCharacter)
    {
        switch (deadCharacter.creatureID)
        {
            case 0:
                effectsAudioSource.PlayOneShot(zombieRisenDeathSound);
                break;
                
            case 1:
                effectsAudioSource.PlayOneShot(zombieInfestedDeathSound);
                break;

            case 2:
                effectsAudioSource.PlayOneShot(zombieUndyingDeathSound);
                break;

            case 3:
                effectsAudioSource.PlayOneShot(zombieHulkDeathSound);
                break;

            case 4:
                effectsAudioSource.PlayOneShot(zombieSprinterDeathSound);
                break;

            // Player
            case 999999:
                effectsAudioSource.PlayOneShot(playerDeathSound);
                break;
        }
    }

    private void PlayAmmoLootSound()
    {
        effectsAudioSource.PlayOneShot(ammoLootSound);
    }

    private void PlayGunShotSound()
    {
        gunOneShotAudioSource.pitch = Random.Range(0.98f, 1.02f);
        gunOneShotAudioSource.volume = Random.Range(0.95f, 1.05f);

        // Randomly play one of the gun fire sounds
        if (gunShootSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, gunShootSounds.Length);
            gunOneShotAudioSource.PlayOneShot(gunShootSounds[randomIndex]);
        }
    }

    private void PlayBackgroundMusic()
    {
        musicAudioSource.volume = 0;
        musicAudioSource.DOFade(1, 10).SetUpdate(true);

        musicAudioSource.clip = backgroundMusic;
        musicAudioSource.Play();
    }

    public void PlayGunLoop(bool startStop)
    {
        gunLoopStopTween?.Kill();

        if (!startStop)
        {
            gunLoopStopTween = gunLoopAudioSource.DOFade(0, 0.15f)
                .OnComplete(() =>
                {
                    gunLoopAudioSource.Stop(); 
                });
            return;
        }

        if (UserInterface.isGamePaused) return;

        gunLoopAudioSource.volume = 1;
        gunLoopAudioSource.clip = gunLoopSound;
        gunLoopAudioSource.Play();
    }

    void PlayRandomZombieSound()
    {
        if (randomZombieSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, randomZombieSounds.Length);
            effectsAudioSource.PlayOneShot(randomZombieSounds[randomIndex]);
        }
    }

}
