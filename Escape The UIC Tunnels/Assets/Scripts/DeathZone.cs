using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public string nextSceneName = "Level1";
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.LoadSceneWithFade(nextSceneName);
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
