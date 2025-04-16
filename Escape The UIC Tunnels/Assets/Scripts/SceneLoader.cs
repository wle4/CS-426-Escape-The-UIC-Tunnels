using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");

    public static class Quick
    {
        public static void Load(string name) => SceneManager.LoadScene(name);
        public static void Reload() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
