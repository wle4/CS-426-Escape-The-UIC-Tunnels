using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;

    [Header("UI Elements")]
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Settings")]
    [TextArea(2, 5)]
    public string[] dialogueLines;  // Now editable from the Inspector!

    [Header("Optional Barrier")]
    public GameObject barrierToDisable;

    [Header("Optional Item Reward")]
    public string itemToGive;

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
        if (dialogueLines.Length == 0) return;

        isTalking = true;
        patrol?.Pause();
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
        patrol?.Resume();

        if (barrierToDisable != null)
            barrierToDisable.SetActive(false);

        if (!string.IsNullOrEmpty(itemToGive))
        {
            PlayerInventory inv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            if (inv != null)
                inv.AddItem(itemToGive);
        }
        Debug.Log("EndDialogue triggered. itemToGive = " + itemToGive);
    }

}
