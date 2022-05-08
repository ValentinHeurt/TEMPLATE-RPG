using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoalUIHandler : MonoBehaviour
{
    public TextMeshProUGUI goalNameText;
    public TextMeshProUGUI goalInformationText;
    public void FillInformations(string name, string current, string required, bool completed)
    {
        goalNameText.text = name;
        goalInformationText.text = $"{current} / {required}";
    }
}
