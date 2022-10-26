using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Assets/Dialogue")]
[Serializable]
public class Dialogue : ScriptableObject
{

    public DialogueLine dialogueLine;

    public DialogueLine CreateFirstDialogueLine()
    {
        DialogueLine line = new DialogueLine();//ScriptableObject.CreateInstance("DialogueLine") as DialogueLine;
        line.line = "PlaceHolder";
        line.guid = GUID.Generate().ToString();
        this.dialogueLine = line;

        return line;
    }

    public DialogueLine CreateDialogueLine()
    {
        DialogueLine line = new DialogueLine();//ScriptableObject.CreateInstance("DialogueLine") as DialogueLine;
        line.line = "PlaceHolder";
        line.guid = GUID.Generate().ToString();

        return line;
    }


    public AnswerData CreateAnswerData()
    {
        AnswerData answer = new AnswerData();
        answer.text = "PlaceHolder";
        answer.guid = GUID.Generate().ToString();
        return answer;
    }

    public void DeleteDialogueLine()
    {
        this.dialogueLine = null;

    }


}
