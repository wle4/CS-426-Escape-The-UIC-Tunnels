using TMPro;
using UnityEngine;
using System.Collections;

public class IntroDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] lines;
    public float textSpeed = 0.05f;

    private int index = 0;
    private bool finishedLine = false;
    private bool hasStarted = false;

    void Start()
    {
        dialogueText.text = "";
        // Scene starts paused, waiting for key press to begin
    }

    void Update()
    {
        if (!hasStarted && Input.anyKeyDown)
        {
            hasStarted = true;
            StartCoroutine(StartDialogue());
        }
        else if (hasStarted && Input.anyKeyDown)
        {
            if (!finishedLine)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                finishedLine = true;
            }
            else
            {
                NextLine();
            }
        }
    }

    IEnumerator StartDialogue()
    {
        // fade from black first
        if (UIManager.instance != null)
        {
            yield return UIManager.instance.FadeFromBlack();
        }

        // then start the first line
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        finishedLine = false;
        dialogueText.text = "";

        foreach (char c in lines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        finishedLine = true;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            UIManager.instance?.LoadSceneWithFade("Level1");
        }
    }
}
