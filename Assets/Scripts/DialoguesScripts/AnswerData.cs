using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnswerData : ScriptableObject
{
    public DialogueLine nextDialogueLine = null;
    public Quest questToStart;
    public Quest questNeededToDisplay;
    public Item itemToGive;
    public string text;
    public string guid;

    // For Editor
    public Vector2 position;
}
