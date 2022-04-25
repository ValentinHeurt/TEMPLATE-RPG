using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : Singleton<DialogueManager>
{

    [SerializeField] GameObject dialoguePanel;

    [SerializeField] Text dialogueText, nameText;

    string npcName;
    List<string> dialogueLines = new List<string>();
    int dialogueIndex;

    public float timeSinceLastLine;

    public override void Awake()
    {
        base.Awake();
        dialoguePanel.SetActive(false);
    }
    private void Update()
    {
        if (GameManager.Instance.IsInDialogue)
        {
            timeSinceLastLine += Time.deltaTime;
        }
    }
    public void AddNewDialogue(string[] lines, NPC npc)
    {
        GameManager.Instance.Dialogue();
        timeSinceLastLine = 0;
        dialogueIndex = 0;
        dialogueLines = new List<string>(lines.Length);
        dialogueLines.AddRange(lines);
        npcName = npc.interactableName;
        CreateDialogue();
    }


    void CreateDialogue()
    {
        if (dialogueLines.Count == 0)
        {
            Debug.LogError($"Erreur de paramétrage, le pnj {npcName} n'a pas de ligne de dialogue.");
        }
        else
        {
            dialogueText.text = dialogueLines[dialogueIndex];
            nameText.text = npcName;
            dialoguePanel.SetActive(true);
        }
    }

    public void ContinueDialogue()
    {
        timeSinceLastLine = 0;
        if (dialogueIndex < dialogueLines.Count - 1)
        {
            
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];
        }
        else
        {
            dialoguePanel.SetActive(false);
            npcName = "none";
            GameManager.Instance.Play();
        }
    }
}
