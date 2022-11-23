using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : Singleton<DialogueManager>
{

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] Answer answerPrefab;
    [SerializeField] Transform answersContainer;
    [SerializeField] Text dialogueText, nameText;
    
    string npcName;
    Dialogue dialogueLines;
    public DialogueLine dialogueLine;
    int dialogueIndex;

    public float timeSinceLastLine;

    public override void Awake()
    {
        base.Awake();
        dialoguePanel.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<OnAnswerChosen>(ContinueDialogue);
    }

    private void Update()
    {
        if (GameManager.Instance.IsInDialogue)
        {
            timeSinceLastLine += Time.deltaTime;
        }
    }
    public void AddNewDialogue(Dialogue lines, NPC npc)
    {
        GameManager.Instance.Dialogue();
        timeSinceLastLine = 0;
        npcName = npc.interactableName;
        dialogueLines = lines;
        CreateDialogue();
    }

    void CreateDialogue()
    {
        if (dialogueLines == null)
        {
            Debug.LogError($"Erreur de paramétrage, le pnj {npcName} n'a pas de ligne de dialogue.");
        }
        else
        {
            dialogueLine = dialogueLines.dialogueLine;
            dialogueText.text = dialogueLine.line;
            if (dialogueLine.answers.Count > 0)
            {
                answersContainer.gameObject.SetActive(true);
                InstantiateNewAnswers(dialogueLine.answers);
            }
            else
            {
                answersContainer.gameObject.SetActive(false);
            }
            nameText.text = npcName;
            dialoguePanel.SetActive(true);
        }
    }

    public void ContinueDialogue(OnAnswerChosen eventInfo)
    {
        timeSinceLastLine = 0;
        if (eventInfo.answerData.questToStart != null)
        {
            QuestManager.Instance.AddQuest(eventInfo.answerData.questToStart);
        }
        if (eventInfo.answerData.questNeededToDisplay != null)
        {
            QuestManager.Instance.TryFinishQuest(eventInfo.answerData.questNeededToDisplay);
        }
        if (eventInfo.answerData.itemToGive != null)
        {
            EventManager.Instance.QueueEvent(new AddOneItemGameEvent(eventInfo.answerData.itemToGive));
        }
        //if (eventInfo.answerData)
        //{

        //}

        if (eventInfo.answerData.nextDialogueLine == null || eventInfo.answerData.nextDialogueLine.line == "")
        {
            DestroyAnswers();
            dialoguePanel.SetActive(false);
            npcName = "none";
            dialogueLines = null;
            GameManager.Instance.Play();
            return;
        }
        dialogueLine = eventInfo.answerData.nextDialogueLine;
        dialogueText.text = dialogueLine.line;
        if (dialogueLine.answers.Count > 0)
        {
            answersContainer.gameObject.SetActive(true);
            InstantiateNewAnswers(dialogueLine.answers);
        }
        else
        {
            answersContainer.gameObject.SetActive(false);
        }
    }

    public void ContinueDialogueNoAnswers()
    {
        if (dialogueLine.answers == null || dialogueLine.answers.Count == 0)
        {
            timeSinceLastLine = 0;
            if(dialogueLine.directNextLine == null)
            {
                DestroyAnswers();
                dialoguePanel.SetActive(false);
                npcName = "none";
                dialogueLines = null;
                GameManager.Instance.Play();
                return;
            }
        }
        dialogueLine = dialogueLine.directNextLine;
        dialogueText.text = dialogueLine.line;
        if (dialogueLine.answers.Count > 0)
        {
            answersContainer.gameObject.SetActive(true);
            InstantiateNewAnswers(dialogueLine.answers);
        }
        else
        {
            answersContainer.gameObject.SetActive(false);
        }
    }

    void DestroyAnswers()
    {
        foreach (Transform answers in answersContainer)
        {
            GameObject.Destroy(answers.gameObject);
        }
    }

    public void InstantiateNewAnswers(List<AnswerData> answerDatas)
    {
        foreach (Transform answers in answersContainer)
        {
            GameObject.Destroy(answers.gameObject);
        }

        foreach (AnswerData answerData in answerDatas)
        {
            if (answerData.questNeededToDisplay == null)
            {
                Answer tempAnswer = Instantiate(answerPrefab, answersContainer);
                tempAnswer.SetData(answerData);
            }
            else
            {
                if (QuestManager.Instance.HasQuest(answerData.questNeededToDisplay) 
                    && QuestManager.Instance.CanQuestBeFinished(answerData.questNeededToDisplay))
                {
                    Answer tempAnswer = Instantiate(answerPrefab, answersContainer);
                    tempAnswer.SetData(answerData);
                }
            }
        }
    }
}
