using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class Item : MonoBehaviour {
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    //public bool isArmour;
    public bool isArmour => armorSlot != ArmorLocation.NotArmor;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP, affectMP, affectStr;

    [Header("Weapon/Armor Details")]
    public int weaponStrength;

    public int armorStrength;

    public ArmorLocation armorSlot = ArmorLocation.NotArmor;
    //public bool head; 
    //public bool body;
    //public bool hand;
    //public bool legs;
    //public bool feet;
    //public bool other;
    //public bool shield;


    [Header("Usable By")]
    public bool Cleric;
    public bool Druid;
    public bool Fighter;
    public bool Mage;
    public bool Paladin;
    public bool Ranger;
    public bool Thief;


    public enum ArmorLocation
    {
        NotArmor=-1,Head, Body,Hand,Legs,Feet,Other,Shield
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.GetCharacterStats[charToUseOn];

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

        if(isWeapon)
        {
            if(selectedChar.GetEquippedWeapon != "")
            {
                GameManager.instance.AddItem(selectedChar.GetEquippedWeapon);
            }

            selectedChar.SetEquippedWeapon(itemName);
            selectedChar.SetWeaponPower(weaponStrength);
        }

        if(isArmour)
        {
            EquipArmour(selectedChar);
        }

        GameManager.instance.RemoveItem(itemName);
    }

    private void EquipArmour(CharStats selectedChar)
    {
        Item newArmor = GameManager.instance.GetItemDetails(itemName);

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
        //Unequip armour if it is equipped
        if (currentArmorKey != "")
        {
            GameManager.instance.AddItem(currentArmorKey);
            Item prevArmour = GameManager.instance.GetItemDetails(currentArmorKey);

            if(null != prevArmour)
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
}
