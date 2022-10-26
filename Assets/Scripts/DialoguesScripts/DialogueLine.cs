using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DialogueLine
{
    public List<AnswerData> answers = new List<AnswerData>();
    public List<DialogueLine> directNextLine = new List<DialogueLine>();
    public string line;
    public string guid;

    // For Editor
    public Vector2 position;
}
