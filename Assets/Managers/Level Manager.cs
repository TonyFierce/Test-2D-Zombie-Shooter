using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int currentSpawnedEnemyIndex;

    public static int currentDroppedLootIndex;

    public static Transform selfTransform;

    public static bool levelStarted = false;

    private void Awake()
    {
        Time.timeScale = 1;

        levelStarted = false;

        selfTransform = transform;

        currentSpawnedEnemyIndex = 0;

        currentDroppedLootIndex = 0;
    }

    private void Start()
    {
        SoundManager.selfInstance.gunLoopAudioSource.volume = 1;
        SoundManager.selfInstance.gunLoopAudioSource.Stop();

        UserInterface.BlackoutOnLevelOpen();
        UserInterface.FadeInOut(0, false);
    }
}