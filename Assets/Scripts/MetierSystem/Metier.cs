using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "New Metier", menuName = "Assets/Metier")]
public class Metier : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public string animationBool;
    public Sprite icon;
    public float harvestTime;
    public LevelSystem levelSystem;
    public List<HarvestableData> harvestableData;
    



}
[System.Serializable]
public class HarvestableData
{
    public BaseItem item;
    public int xpToGive;
    public int minLvlToHarvest;
}