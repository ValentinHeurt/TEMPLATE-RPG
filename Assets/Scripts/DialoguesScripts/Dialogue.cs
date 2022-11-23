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
        DialogueLine line = ScriptableObject.CreateInstance("DialogueLine") as DialogueLine;//new DialogueLine();//ScriptableObject.CreateInstance("DialogueLine") as DialogueLine;
        line.line = "";
        line.guid = GUID.Generate().ToString();
        this.dialogueLine = line;

        AssetDatabase.AddObjectToAsset(line, this);
        AssetDatabase.SaveAssets();
        return line;
    }

    public DialogueLine CreateDialogueLine()
    {
        DialogueLine line = ScriptableObject.CreateInstance("DialogueLine") as DialogueLine; //new DialogueLine();//ScriptableObject.CreateInstance("DialogueLine") as DialogueLine;
        line.line = "";
        line.guid = GUID.Generate().ToString();
        AssetDatabase.AddObjectToAsset(line, this);
        AssetDatabase.SaveAssets();
        return line;
    }


    public AnswerData CreateAnswerData()
    {
        AnswerData answer = ScriptableObject.CreateInstance("AnswerData") as AnswerData;
        answer.text = "";
        answer.guid = GUID.Generate().ToString();
        AssetDatabase.AddObjectToAsset(answer, this);
        AssetDatabase.SaveAssets();
        return answer;
    }

    public void DeleteDialogueLine()
    {
        this.dialogueLine = null;

    }


}
