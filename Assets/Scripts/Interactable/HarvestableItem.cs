using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HarvestableItem : Interactable
{
    public Item templateItem;
    [HideInInspector] public Item itemToGive;
    private bool _isCurrentlyHarvestable;
    public int minLvlToHarvest;
    public int xpToGive;
    public float harvestTime;
    public Metier metierRequired;
    public int minAmountToGive;
    public int maxAmountToGive;

    [Header("Events")] 
    public ItemSlotEvent onHarvestFinished;

    private void Start()
    {
        itemToGive = Instantiate(templateItem);
    }
    public override string ColoredName
    {
        get
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(itemToGive.rarity.Color);
            return $"<color=#{hexColor}>{interactableName}</color>";
        }
    }
    public override void PlayerInteracted(GameObject interacted)
    {
        if (interacted.GetComponent<Metiers>() != null)
        {
            Metiers metiers = interacted.GetComponent<Metiers>();
            if (metiers.CheckLevel(metierRequired.ID, minLvlToHarvest))
            {
                StartCoroutine(StartHarvest(interacted, metiers));
            }
        }
    }

    public IEnumerator StartHarvest(GameObject interacted, Metiers metiers)
    {
        Animator animator = interacted.GetComponent<Animator>();
        animator.SetBool(metierRequired.animationBool, true);
        yield return new WaitForSeconds(harvestTime);
        animator.SetBool(metierRequired.animationBool, false);
        metiers.GiveXp(metierRequired.ID, xpToGive);
        FinishHarvest();
    }

    public void FinishHarvest()
    {
        onHarvestFinished.Raise(new ItemSlot(itemToGive, UnityEngine.Random.Range(minAmountToGive,maxAmountToGive)));
    }

}
