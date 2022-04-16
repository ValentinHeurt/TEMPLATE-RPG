using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InventoryController : Singleton<InventoryController>
{
    public Item weapon;
    public static Action<Weapon> OnWeaponEquipped;
    private void Start()
    {
        //EquipWeapon(weapon);
    }

    private void OnEnable()
    {
        //PickableItem.OnItemPicked += HandleItemPicked;
    }
    private void OnDisable()
    {
        //PickableItem.OnItemPicked -= HandleItemPicked;
    }

    void EquipWeapon(Weapon weapon)
    {
        OnWeaponEquipped?.Invoke(weapon);

    }
    void HandleItemPicked(Item item)
    {
        if (item is Weapon)
        {
            EquipWeapon((Weapon)item);
        }
        

    }
}
