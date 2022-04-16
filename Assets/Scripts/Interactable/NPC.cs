using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    //pour être plus simple je fais comme ça mais il faudrait utiliser un petit fichier JSON pour structurer les dialogues
    public string[] dialogue;

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
