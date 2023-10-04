using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class Item : MonoBehaviour {
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    //public bool isArmour;
    public bool isArmour => armorSlot == ArmorLocation.NotArmor;

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
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if(isItem)
        {
            if(affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if(affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if(affectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }

        if(isWeapon)
        {
            if(selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
        }

        if(isArmour)
        {
            EquipArmour(selectedChar);
        }

        GameManager.instance.RemoveItem(itemName);
    }

    private void EquipArmour(CharStats selectedChar)
    {
        //Unequip armour if it is equipped
        if (selectedChar.equippedArmr != "")
        {
            GameManager.instance.AddItem(selectedChar.equippedArmr);
            Item prevArmour = GameManager.instance.GetItemDetails(selectedChar.equippedArmr);
            selectedChar.armrPwr -= prevArmour.armorStrength;
        }

        selectedChar.equippedArmr = itemName;
        selectedChar.armrPwr = armorStrength;
    }
}
