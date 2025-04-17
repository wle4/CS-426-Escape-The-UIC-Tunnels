using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpScareDoor1 : MonoBehaviour
{
    public GameObject jumpscareImageObject;
    public AudioSource screamAudio;
    public float fadeDuration = 2f;
    public float holdTime = 2f; // Time to hold E
    public float interactDistance = 3f;

    private bool triggered = false;
    private float holdTimer = 0f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (triggered || player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist <= interactDistance)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= holdTime)
                {
                    TryTriggerJumpscare();
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }

    void TryTriggerJumpscare()
    {
        var inv = player.GetComponent<PlayerInventory>();
        if (inv != null && inv.HasItem("Key"))
        {
            triggered = true;
            jumpscareImageObject.SetActive(true);
            screamAudio?.Play();
            inv.UseItem("Key");

            StartCoroutine(FadeOutImage(jumpscareImageObject.GetComponent<RawImage>()));
        }
    }

    IEnumerator FadeOutImage(RawImage image)
    {
        Color color = image.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, 0, timer / fadeDuration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, 0);
        jumpscareImageObject.SetActive(false);
    }
}
