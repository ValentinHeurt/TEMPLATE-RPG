using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public Dialogue dialogue;

    public Color nameColor;

    public override void PlayerInteracted(GameObject player)
    {
        DialogueManager.Instance.AddNewDialogue(dialogue, this);
    }

    public override string ColoredName
    {
        get
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(nameColor);
            return $"<color=#{hexColor}>{interactableName}</color>";
        }
    }
}
