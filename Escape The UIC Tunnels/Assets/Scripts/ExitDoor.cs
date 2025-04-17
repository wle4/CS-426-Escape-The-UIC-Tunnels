using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private string nextSceneName = "Level2";  // Set this in the inspector

    public string GetPrompt() => "Enter [E]";

    public void OnInteract()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.LoadSceneWithFade(nextSceneName);
        }
        else
        {
            Debug.LogWarning("UIManager not found, falling back to direct load.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}