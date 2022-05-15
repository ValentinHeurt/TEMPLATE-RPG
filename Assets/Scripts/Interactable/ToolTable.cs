using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTable : Interactable
{
    public Color nameColor;
    public GameObject toolTable;
    public override void PlayerInteracted(GameObject player)
    {
        toolTable.SetActive(true);
        GameManager.Instance.CursorLocker += 1;
        if (GameManager.Instance.CursorLocker == 0)
            GameManager.Instance.Play();
        else
            GameManager.Instance.Inventory();
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
