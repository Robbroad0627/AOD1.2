/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: Item.cs
 * Date Created: 
 * Created By: Rob Broad
 * Modified By: Jeff Moreau
 * Date Last Modified: June 14, 2024
 * Description: Base Script to take care of Item functionality
 * TODO: 
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

namespace AOD
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ScriptableObjects.ItemBaseSO ItemData;

        public ScriptableObjects.ItemBaseSO GetItemData => ItemData;
        /*[Header("Item Type")]
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
        public bool Thief;*/


        /*        public enum ArmorLocation
                {
                    NotArmor = -1, Head, Body, Hand, Legs, Feet, Other, Shield
                }

                // Use this for initialization
                void Start()
                {

                }

                // Update is called once per frame
                void Update()
                {

                }
        */
        public void Use(int charToUseOn)
        {
            CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

            if (ItemData.GetIsItem)
            {
                if (ItemData.GetHPBonus)
                {
                    selectedChar.currentHP += ItemData.GetStatBonusAmount;

                    if (selectedChar.currentHP > selectedChar.maxHP)
                    {
                        selectedChar.currentHP = selectedChar.maxHP;
                    }
                }

                if (ItemData.GetMPBonus)
                {
                    selectedChar.currentMP += ItemData.GetStatBonusAmount;

                    if (selectedChar.currentMP > selectedChar.maxMP)
                    {
                        selectedChar.currentMP = selectedChar.maxMP;
                    }
                }

                if (ItemData.GetStrBonus)
                {
                    selectedChar.strength += ItemData.GetStatBonusAmount;
                }
            }

            if (ItemData.GetIsWeapon)
            {
                if (selectedChar.equippedWpn != "")
                {
                    GameManager.instance.AddItem(selectedChar.equippedWpn);
                }

                selectedChar.equippedWpn = ItemData.GetName;
                selectedChar.wpnPwr = ItemData.GetAttackStrength;
            }

            if (ItemData.GetIsArmor)
            {
                EquipArmour(selectedChar);
            }

            GameManager.instance.RemoveItem(ItemData.GetName);
        }

        private void EquipArmour(CharStats selectedChar)
        {
            Item newArmor = GameManager.instance.GetItemDetails(ItemData.GetName);

            string currentArmorKey = "";

            switch (newArmor.ItemData.GetArmorSlot)
            {
                case ArmorSlot.NotArmor:
                    throw new System.Exception("Can't equip non armor to armor.");
                case ArmorSlot.Head:
                    currentArmorKey = selectedChar.equippedHeadArmr;
                    break;
                case ArmorSlot.Body:
                    currentArmorKey = selectedChar.equippedBodyArmr;
                    break;
                case ArmorSlot.Hands:
                    currentArmorKey = selectedChar.equippedHandArmr;
                    break;
                case ArmorSlot.Legs:
                    currentArmorKey = selectedChar.equippedLegsArmr;
                    break;
                case ArmorSlot.Feet:
                    currentArmorKey = selectedChar.equippedFeetArmr;
                    break;
                case ArmorSlot.Other:
                    currentArmorKey = selectedChar.equippedOtherArmr;
                    break;
                case ArmorSlot.Shield:
                    currentArmorKey = selectedChar.equippedShieldArmr;
                    break;
            }
            //Unequip armour if it is equipped
            if (currentArmorKey != "")
            {
                GameManager.instance.AddItem(currentArmorKey);
                Item prevArmour = GameManager.instance.GetItemDetails(currentArmorKey);

                if (null != prevArmour)
                {
                    Debug.Log($"Unequip {prevArmour.ItemData.GetName} from {selectedChar.charName}  => -{prevArmour.ItemData.GetArmorAmount}");
                    selectedChar.armrPwr -= prevArmour.ItemData.GetArmorAmount;
                }

            }

            switch (newArmor.ItemData.GetArmorSlot)
            {
                case ArmorSlot.NotArmor:
                    throw new System.Exception("Can't equip non armor to armor.");
                case ArmorSlot.Head:
                    selectedChar.equippedHeadArmr = ItemData.GetName;
                    break;
                case ArmorSlot.Body:
                    selectedChar.equippedBodyArmr = ItemData.GetName;
                    break;
                case ArmorSlot.Hands:
                    selectedChar.equippedHandArmr = ItemData.GetName;
                    break;
                case ArmorSlot.Legs:
                    selectedChar.equippedLegsArmr = ItemData.GetName;
                    break;
                case ArmorSlot.Feet:
                    selectedChar.equippedFeetArmr = ItemData.GetName;
                    break;
                case ArmorSlot.Other:
                    selectedChar.equippedOtherArmr = ItemData.GetName;
                    break;
                case ArmorSlot.Shield:
                    selectedChar.equippedShieldArmr = ItemData.GetName;
                    break;
            }

            selectedChar.equippedArmr = ItemData.GetName;
            selectedChar.armrPwr += newArmor.ItemData.GetArmorAmount;
            Debug.Log($"Equip {newArmor.ItemData.GetName} from {selectedChar.charName}  => +{newArmor.ItemData.GetArmorAmount}");
        }
    }
}