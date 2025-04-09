using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public float interactionDistance = 3f;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    private string[] dialogueLines = {
        "Hello there!",
        "You look like you're new around here.",
        "Pass the obstacle... hehehe",
        "OooOOOooOO!"
    };

    private int currentLine = 0;
    private bool isTalking = false;
    private Transform player;
    private NPCRoam patrol;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        patrol = GetComponent<NPCRoam>();
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        

        if (dist < interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
                StartDialogue();
            else
                NextLine();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        patrol.Pause();
        dialogueUI.SetActive(true);
        currentLine = 0;
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = dialogueLines[currentLine];
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);
        patrol.Resume();
    }
}
