using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level2Door : MonoBehaviour
{
    public float interactDistance = 3f;
    public string nextSceneName = "Level3";  // Set this in Inspector
    public TextMeshProUGUI promptText;
    private AudioSource audioSource;
    [SerializeField] private AudioClip doorSound;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (promptText != null) promptText.enabled = false;
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance)
        {
            if (!JumpScareDoor1.hasTriedLeftDoor)
            {
                if (promptText != null)
                {
                    promptText.text = "Maybe I should try the left door first...";
                    promptText.enabled = true;
                }

                return; // Block interaction until left door is tried
            }

            if (promptText != null)
            {
                promptText.text = "Press E to exit";
                promptText.enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(UIManager.instance != null)
                    {
                        if (doorSound != null)
                        {
                            audioSource.PlayOneShot(doorSound);
                        }
                        UIManager.instance.LoadSceneWithFade(nextSceneName);
                    }
                    else { 
                        SceneManager.LoadScene(nextSceneName); 
                    }
                }
            }

            
        }
        else
        {
            if (promptText != null) promptText.enabled = false;
        }
    }
}
