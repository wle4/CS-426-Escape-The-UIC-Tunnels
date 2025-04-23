using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Panels")]
    public GameObject fadePanel;
    public GameObject mainMenuUI;
    public GameObject hudUI;
    public GameObject pauseMenuUI;
    public GameObject creditsMenuUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(FadeFromBlack());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // SCENE HANDLING
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUIForScene(scene.name);
        StartCoroutine(FadeFromBlack());
    }

    private void UpdateUIForScene(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            ShowOnly(mainMenuUI);
        }
        else if (sceneName.StartsWith("Level") || sceneName == "GameScene")
        {
            ShowOnly(hudUI);
        }
        else if (sceneName == "Credits")
        {
            ShowOnly(creditsMenuUI);
        }
        else
        {
            ShowOnly(null); // show nothing
        }
    }

    public void ShowOnly(GameObject panelToShow)
    {
        GameObject[] panels = { mainMenuUI, hudUI, pauseMenuUI, creditsMenuUI };
        foreach (GameObject panel in panels)
        {
            if (panel != null)
                panel.SetActive(panel == panelToShow);
        }
    }

    public void ShowFade(bool show)
    {
        if (fadePanel != null)
            fadePanel.SetActive(show);
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeToBlackAndLoad(sceneName));
    }

    private IEnumerator FadeToBlackAndLoad(string sceneName)
    {
        yield return StartCoroutine(FadeToBlack());
        SceneLoader.Quick.Load(sceneName); // your clean SceneLoader
    }

    private IEnumerator FadeToBlack()
    {
        if (fadePanel == null) yield break;

        // Ensure the fadePanel is active and has CanvasGroup
        fadePanel.SetActive(true);
        CanvasGroup cg = fadePanel.GetComponent<CanvasGroup>();
        if (cg == null) cg = fadePanel.AddComponent<CanvasGroup>();

        cg.blocksRaycasts = true;
        cg.alpha = 0f; // Start transparent

        float t = 0f;
        float duration = 1f; // or make this a serialized field if needed

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(t / duration);
            yield return null;
        }

        cg.alpha = 1f; // Make absolutely sure it's fully black
    }


    public IEnumerator FadeFromBlack()
    {
        if (fadePanel == null) yield break;

        CanvasGroup cg = fadePanel.GetComponent<CanvasGroup>();
        if (cg == null) cg = fadePanel.AddComponent<CanvasGroup>();

        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            cg.alpha = t;
            yield return null;
        }

        cg.blocksRaycasts = false;
        fadePanel.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    // END OF SCENE HANDLING

    public HUDController GetHUDController()
    {
        return hudUI.GetComponent<HUDController>();
    }

}
