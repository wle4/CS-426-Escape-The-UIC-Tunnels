using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class JumpScareDoor1 : MonoBehaviour
{
    public GameObject jumpscareImageObject;
    public AudioSource screamAudio;
    public float fadeDuration = 2f;
    public float holdTime = 2f;
    public float interactDistance = 3f;

    public TextMeshProUGUI promptText;    // "Press E to enter"
    public TextMeshProUGUI popupMessage;  // "You need a key"

    private bool triggered = false;
    private float holdTimer = 0f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (promptText != null) promptText.enabled = false;
        if (popupMessage != null) popupMessage.enabled = false; // ✅ ADD THIS
    }

    private bool hasUsedDoor = false; // 🆕

void Update()
{
    if (triggered || player == null) return;

    float dist = Vector3.Distance(player.position, transform.position);

    if (dist <= interactDistance && !hasUsedDoor)
    {
        if (promptText != null)
        {
            promptText.text = "Hold E to enter";
            promptText.enabled = true;
        }

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
    else
    {
        if (promptText != null) promptText.enabled = false;
        holdTimer = 0f;
    }
}


    void TryTriggerJumpscare()
    {
        if (promptText != null)
            promptText.enabled = false;

        var inv = player.GetComponent<PlayerInventory>();
        if (inv != null && inv.HasItem("Key"))
        {
            triggered = true;
            hasUsedDoor = true; // ✅ ONLY mark it used if they had the key!

            inv.UseItem("Key");
            jumpscareImageObject.SetActive(true);
            screamAudio?.Play();
            StartCoroutine(FadeOutImage(jumpscareImageObject.GetComponent<RawImage>()));
        }
        else
        {
            // ❌ Do NOT mark as used!
            StartCoroutine(ShowPopup("I need a key..."));
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

    IEnumerator ShowPopup(string message)
    {
        popupMessage.text = message;
        popupMessage.enabled = true;
        yield return new WaitForSeconds(2f);
        popupMessage.enabled = false;
    }
}
