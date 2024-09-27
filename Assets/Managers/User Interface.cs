using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    public TextMeshProUGUI currentAmmoText;
    public static UserInterface selfInstance;

    public CanvasGroup lossBlur;
    public CanvasGroup restartExitMenu;
    public RectTransform lossReasonWidget;
    public TextMeshProUGUI lossReasonText;
    public CanvasGroup confirmExitGameMenu;
    public CanvasGroup fadeInOutBlur;
    public CanvasGroup continueMenu;
    public CanvasGroup pauseBlur;

    public static string lossStringText;

    public static bool isGamePaused = false;

    private void Awake()
    {
        selfInstance = this;
    }

    void Update()
    {
        // Check if the ESC key was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void Start()
    {
        ReloadLevelScene();
    }

    public static void UpdateAmmoCount()
    {
        selfInstance.currentAmmoText.text = PlayerStateManager.currentAmmo.ToString();
    }

    public static void InitializeLossInterface()
    {
        if (PlayerStateManager.gameLost) return;

        selfInstance.PauseGameOnLoss();

        PlayerStateManager.gameLost = true;

        selfInstance.lossReasonText.text = lossStringText;

        selfInstance.lossBlur.gameObject.SetActive(true);
        selfInstance.lossBlur.alpha = 0;

        selfInstance.lossBlur.DOFade(1, 0.7f).SetEase(Ease.InSine).SetUpdate(true)
            .OnComplete(() =>
            {
                selfInstance.ShowLossReasonText();
            });
    }

    void ShowLossReasonText()
    {
        lossReasonWidget.localScale = new Vector3(0, 0, 0);
        lossReasonWidget.gameObject.SetActive(true);

        lossReasonWidget.DOScale(1, 0.5f).SetEase(Ease.OutBack, 4f).SetUpdate(true)
            .OnComplete(() =>
            {
                FadeInRestartMenu();
            });
    }

    void FadeInRestartMenu()
    {
        restartExitMenu.alpha = 0;
        restartExitMenu.blocksRaycasts = false;
        restartExitMenu.gameObject.SetActive(true);

        restartExitMenu.DOFade(1, 0.5f).SetUpdate(true)
            .OnComplete(() =>
            {
                restartExitMenu.blocksRaycasts = true;
            });
    }

    void HideLossWidgets()
    {
        lossBlur.gameObject.SetActive(false);
        restartExitMenu.gameObject.SetActive(false);
        lossReasonWidget.gameObject.SetActive(false);
    }

    void PauseGameOnLoss()
    {
        SoundManager.selfInstance.gunLoopStopTween?.Kill();
        SoundManager.selfInstance.gunLoopAudioSource.volume = 0;

        Time.timeScale = 0;
        isGamePaused = true;
    }

    public void RestartGame()
    {
        EventSystem.current.SetSelectedGameObject(null);

        HideLossWidgets();
        PlayerStateManager.SetStartingAmmo();
        PlayerStateManager.gameLost = false;
        FadeInOut(1, true);

        pauseBlur.gameObject.SetActive(false);
        continueMenu.gameObject.SetActive(false);

        isGamePaused = false;
    }

    void ReloadLevelScene()
    {
        if (SceneManager.loadedSceneCount > 1)
        {
            // Unload the subscene first
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(PlayerStateManager.levelToLoad);

            // Reload after unloading completes
            asyncUnload.completed += (AsyncOperation op) => {
                SceneManager.LoadScene(PlayerStateManager.levelToLoad, LoadSceneMode.Additive);
            };
        }
        else
        {
            SceneManager.LoadScene(PlayerStateManager.levelToLoad, LoadSceneMode.Additive);
        }
    }

    public void SwitchToConfirmExitGame()
    {
        if (isGamePaused)
        {
            continueMenu.gameObject.SetActive(false);
        }

        restartExitMenu.gameObject.SetActive(false);
        confirmExitGameMenu.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CancelExitGame()
    {
        if (isGamePaused && !PlayerStateManager.gameLost)
        {
            continueMenu.gameObject.SetActive(true);
        }

        restartExitMenu.gameObject.SetActive(true);
        confirmExitGameMenu.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public static void FadeInOut(float endAlpha, bool reloadingLevel)
    {
        selfInstance.fadeInOutBlur.gameObject.SetActive(true);

        selfInstance.fadeInOutBlur.DOFade(endAlpha, 0.7f).SetEase(Ease.InSine).SetUpdate(true)
            .OnComplete(() =>
            {
                if (selfInstance.fadeInOutBlur.alpha == 0)
                {
                    selfInstance.fadeInOutBlur.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    LevelManager.levelStarted = true;
                }

                if (reloadingLevel)
                {
                    selfInstance.ReloadLevelScene();
                }
            });
    }

    public static void BlackoutOnLevelOpen()
    {
        selfInstance.fadeInOutBlur.gameObject.SetActive(true);
        selfInstance.fadeInOutBlur.alpha = 1;
    }

    void PauseGame()
    {
        if (PlayerStateManager.gameLost) return;

        if (Time.timeScale == 1 && !isGamePaused)
        {
            SoundManager.selfInstance.gunLoopStopTween?.Kill();
            SoundManager.selfInstance.gunLoopAudioSource.volume = 0;

            isGamePaused = true;

            Time.timeScale = 0;

            pauseBlur.gameObject.SetActive(true);
            continueMenu.gameObject.SetActive(true);
            restartExitMenu.gameObject.SetActive(true);
        }
        else if (isGamePaused && continueMenu.gameObject.activeSelf)
        {
            UnpauseGame();
        }

    }

    public void UnpauseGame()
    {
        SoundManager.selfInstance.gunLoopStopTween?.Kill();
        SoundManager.selfInstance.gunLoopAudioSource.volume = 1;

        Time.timeScale = 1;

        isGamePaused = false;

        pauseBlur.gameObject.SetActive(false);
        continueMenu.gameObject.SetActive(false);
        restartExitMenu.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
