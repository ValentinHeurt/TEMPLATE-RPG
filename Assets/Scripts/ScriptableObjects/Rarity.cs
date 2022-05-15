using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Rarity", menuName = "Assets/Rarity")]
public class Rarity : ScriptableObject
{
    [SerializeField] private new string name = "New Rarity Name";

    [SerializeField] private Color textColor = new Color(1f, 1f, 1f);

    [SerializeField] public List<float> upgradeRates;

    public int maxAmeliorationState;

    public string ColoredName
    {
        get
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(Color);
            return $"<color=#{hexColor}>{name}</color>";
        }
    }

    public string Name => name;

    public Color Color => textColor;
}