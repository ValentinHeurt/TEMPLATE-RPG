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
    private bool _isCurrentlyHarvestable = true;
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
            if (metiers.CheckLevel(metierRequired.ID, metierRequired.harvestableData.First(m => m.item.ID == templateItem.ID).minLvlToHarvest)
                && _isCurrentlyHarvestable)
            {
                _isCurrentlyHarvestable = false;
                StartCoroutine(StartHarvest(interacted, metiers));
            }
        }
    }

    public IEnumerator StartHarvest(GameObject interacted, Metiers metiers)
    {
        Animator animator = interacted.GetComponent<Animator>();
        animator.SetBool(metierRequired.animationBool, true);
        yield return new WaitForSeconds(metierRequired.harvestTime);
        animator.SetBool(metierRequired.animationBool, false);
        metiers.GiveXp(metierRequired.ID, metierRequired.harvestableData.First(m => m.item.ID == templateItem.ID).xpToGive);
        FinishHarvest();
    }

    public void FinishHarvest()
    {
        _isCurrentlyHarvestable = true;
        onHarvestFinished.Raise(new ItemSlot(itemToGive, UnityEngine.Random.Range(minAmountToGive,maxAmountToGive)));
    }

}
