using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private string nextSceneName = "Level2";  // Set in Inspector
    [SerializeField] private AudioClip doorSound;              // Sound to play
    [SerializeField] private float delayBeforeLoad = 1.0f;     // Adjust as needed

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public string GetPrompt() => "Enter [E]";

    public void OnInteract()
    {
        if (doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }

        if (UIManager.instance != null)
        {
            StartCoroutine(DelayedLoad());
        }
        else
        {
            Debug.LogWarning("UIManager not found, falling back to direct load.");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        UIManager.instance.LoadSceneWithFade(nextSceneName);
    }
}
