using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class AnswerData
{
    public DialogueLine nextDialogueLine;
    public Quest questToStart;
    public Quest questNeededToDisplay;
    public string text;
}
