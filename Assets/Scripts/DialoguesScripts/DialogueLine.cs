using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueLine : ScriptableObject
{
    public List<AnswerData> answers = new List<AnswerData>();
    public DialogueLine directNextLine;
    public string line;
    public string guid;

    // For Editor
    public Vector2 position;
}
