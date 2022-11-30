using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableName;
    public Sprite icon;
    public InteractableDisplayOption displayOption;
    public virtual string ColoredName { get; }
    public virtual void PlayerInteracted(GameObject player)
    {
        
    }
}

public enum InteractableDisplayOption
{
    NAME,
    ICON,
    BOTH
}
