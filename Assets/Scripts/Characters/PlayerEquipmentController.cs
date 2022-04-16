using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerEquipmentController : MonoBehaviour
{
    public GameObject playerHand;
    public GameObject EquippedWeapon { get; set; }
    
    private Dictionary<EquipmentPart, Equipment> equipmentList = new Dictionary<EquipmentPart, Equipment>();

    public Equipment equippedHelmet;
    public Equipment equippedArmor;
    public Equipment equippedGauntlets;
    public Equipment equippedBoots;

    private void Awake()
    {
        equipmentList.Add(EquipmentPart.Helmet, equippedHelmet);
        equipmentList.Add(EquipmentPart.Armor, equippedArmor);
        equipmentList.Add(EquipmentPart.Gauntlets, equippedGauntlets);
        equipmentList.Add(EquipmentPart.Boots, equippedBoots);
    }
    private void OnEnable()
    {
        EquipmentSlot.onEquipmentEquiped += EquipEquipment;
        Equipment.onEquipmentEquiped += EquipEquipment;
        InventorySlot.onRemoveEquipment += RemoveEquipment;
        Equipment.onRemoveEquipment += RemoveEquipment;
    }
    private void OnDisable()
    {
        Equipment.onEquipmentEquiped -= EquipEquipment;
        EquipmentSlot.onEquipmentEquiped -= EquipEquipment;
        InventorySlot.onRemoveEquipment -= RemoveEquipment;
        Equipment.onRemoveEquipment -= RemoveEquipment;
    }
    private void Start()
    {
    }
    public void EquipEquipment(Equipment equipment)
    {
        // Temporaire pour la partie armure je pense
        if (equipment.equipmentPart == EquipmentPart.Weapon)
        {
            if (EquippedWeapon != null)
            {
                PlayerController.Instance.m_CharacterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
                Destroy(playerHand.transform.GetChild(0).gameObject);
            }

            //Notifie le joueur qu'il a équipé une arme
            PlayerController.Instance.isWeaponEquipped = true;
            EquippedWeapon = (GameObject)Instantiate((equipment as Weapon).weaponPrefab, playerHand.transform);
            EquippedWeapon.GetComponent<IWeapon>().Stats = equipment.stats;
            PlayerController.Instance.m_CharacterStats.AddStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
            PlayerController.Instance.weapon = (MeleeWeapon)EquippedWeapon.GetComponent<IWeapon>(); // Fix fonctionne car qu'un type d'arme pour l'insant
        }
        else
        {
            if (equipmentList[equipment.equipmentPart] != null)
            {
                PlayerController.Instance.m_CharacterStats.RemoveStatBonus(equipmentList[equipment.equipmentPart].stats);
            }
            PlayerController.Instance.m_CharacterStats.AddStatBonus(equipment.stats);
            equipmentList[equipment.equipmentPart] = equipment;
        }


    }

    public void RemoveEquipment(EquipmentPart equipmentPart)
    {
        if (equipmentPart == EquipmentPart.Weapon)
        {
            PlayerController.Instance.m_CharacterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
            Destroy(playerHand.transform.GetChild(0).gameObject);
            EquippedWeapon = null;
            PlayerController.Instance.isWeaponEquipped = false;
        }
        else
        {
            PlayerController.Instance.m_CharacterStats.RemoveStatBonus(equipmentList[equipmentPart].stats);
            equipmentList[equipmentPart] = null;
        }
    }
}
