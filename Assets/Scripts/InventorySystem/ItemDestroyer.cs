using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDestroyer : MonoBehaviour
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI areYouSureText = null;

    private int slotIndex = 0;

    private void OnDisable() => slotIndex = -1;

    public void Activate(ItemSlot itemSlot, int slotIndex)
    {
        this.slotIndex = slotIndex;
        areYouSureText.text = $"Voulez-vous jeter {itemSlot.item.name} ?";
        gameObject.SetActive(true);
    }
    public void Destroy()
    {
        inventory.RemoveAt(slotIndex);
        gameObject.SetActive(false);
    }
}
