/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: InnUpstairsExit.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 27, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    //ENUMERATORS
    #region Private Enumerator Declarations Only

    private enum ArmorLocation
    {
        NotArmor = -1,
        Head,
        Body,
        Hand,
        Legs,
        Feet,
        Other,
        Shield
    }

    #endregion

    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [Header("Item Type")]
    [SerializeField] private bool isItem;
    [SerializeField] private bool isWeapon;
    [SerializeField] private bool isArmour => armorSlot != ArmorLocation.NotArmor;
    [Header("Item Details")]
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private int value;
    [SerializeField] private Sprite itemSprite;
    [Header("Item Details")]
    [SerializeField] private int amountToChange;
    [SerializeField] private bool affectHP, affectMP, affectStr;
    [Header("Weapon/Armor Details")]
    [SerializeField] private int weaponStrength;
    [SerializeField] private int armorStrength;
    [SerializeField] private ArmorLocation armorSlot = ArmorLocation.NotArmor;
    [Header("Usable By")]
    [SerializeField] private bool Cleric;
    [SerializeField] private bool Druid;
    [SerializeField] private bool Fighter;
    [SerializeField] private bool Mage;
    [SerializeField] private bool Paladin;
    [SerializeField] private bool Ranger;
    [SerializeField] private bool Thief;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetValue => value;
    public bool GetIsItem => isItem;
    public string GetName => itemName;
    public bool GetIsArmor => isArmour;
    public bool GetIsWeapon => isWeapon;
    public Sprite GetSprite => itemSprite;
    public string GetDescription => description;

    #endregion

    //FUNCTIONS
    #region Public Functions/Methods

    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.Access.GetCharacterStats[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.SetCurrentHP(selectedChar.GetCurrentHP + amountToChange);

                if (selectedChar.GetCurrentHP > selectedChar.GetMaxHP)
                {
                    selectedChar.SetCurrentHP(selectedChar.GetMaxHP);
                }
            }

            if (affectMP)
            {
                selectedChar.SetCurrentMP(selectedChar.GetCurrentMP + amountToChange);

                if (selectedChar.GetCurrentMP > selectedChar.GetMaxMP)
                {
                    selectedChar.SetCurrentMP(selectedChar.GetMaxMP);
                }
            }

            if (affectStr)
            {
                selectedChar.SetStrength(selectedChar.GetStrength + amountToChange);
            }
        }

        if (isWeapon)
        {
            if (selectedChar.GetEquippedWeapon != "")
            {
                GameManager.Access.AddItem(selectedChar.GetEquippedWeapon);
            }

            selectedChar.SetEquippedWeapon(itemName);
            selectedChar.SetWeaponPower(weaponStrength);
        }

        if (isArmour)
        {
            EquipArmour(selectedChar);
        }

        GameManager.Access.RemoveItem(itemName);
    }

    #endregion
    #region Private Functions/Methods

    private void EquipArmour(CharStats selectedChar)
    {
        Item newArmor = GameManager.Access.GetItemDetails(itemName);

        string currentArmorKey = "";

        switch (newArmor.armorSlot)
        {
            case ArmorLocation.NotArmor:
                throw new System.Exception("Can't equip non armor to armor.");

            case ArmorLocation.Head:
                currentArmorKey = selectedChar.GetEquippedHeadArmor;
                break;

            case ArmorLocation.Body:
                currentArmorKey = selectedChar.GetEquippedBodyArmor;
                break;

            case ArmorLocation.Hand:
                currentArmorKey = selectedChar.GetEquippedHandArmor;
                break;

            case ArmorLocation.Legs:
                currentArmorKey = selectedChar.GetEquippedLegArmor;
                break;

            case ArmorLocation.Feet:
                currentArmorKey = selectedChar.GetEquippedFootArmor;
                break;

            case ArmorLocation.Other:
                currentArmorKey = selectedChar.GetEquippedOtherArmor;
                break;

            case ArmorLocation.Shield:
                currentArmorKey = selectedChar.GetEquippedShield;
                break;
        }

        if (currentArmorKey != "")
        {
            GameManager.Access.AddItem(currentArmorKey);
            Item prevArmour = GameManager.Access.GetItemDetails(currentArmorKey);

            if(prevArmour != null)
            {
                Debug.Log($"Unequip {prevArmour.itemName} from {selectedChar.GetCharacterName}  => -{prevArmour.armorStrength}");
                selectedChar.SetArmorPower(selectedChar.GetArmorPower - prevArmour.armorStrength);
            }
            
        }

        switch (newArmor.armorSlot)
        {
            case ArmorLocation.NotArmor:
                throw new System.Exception("Can't equip non armor to armor.");

            case ArmorLocation.Head:
                 selectedChar.SetEquippedHeadArmor(itemName);
                break;

            case ArmorLocation.Body:
                 selectedChar.SetEquippedBodyArmor(itemName);
                break;

            case ArmorLocation.Hand:
                 selectedChar.SetEquippedHandArmor(itemName);
                break;

            case ArmorLocation.Legs:
                 selectedChar.SetEquippedLegArmor(itemName);
                break;

            case ArmorLocation.Feet:
                 selectedChar.SetEquippedFootArmor(itemName);
                break;

            case ArmorLocation.Other:
                 selectedChar.SetEquippedOtherArmor(itemName);
                break;

            case ArmorLocation.Shield:
                 selectedChar.SetEquippedShield(itemName);
                break;
        }

        selectedChar.SetEquippedArmor(itemName);
        selectedChar.SetArmorPower(selectedChar.GetArmorPower + newArmor.armorStrength);
        Debug.Log($"Equip {newArmor.itemName} from {selectedChar.GetCharacterName}  => +{newArmor.armorStrength}");
    }

    #endregion
}