/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: EssentialsLoader.cs
 * Date Created: June 14, 2024
 * Created By: Jeff Moreau
 * Modified By:
 * Date Last Modified:
 * Description: Scriptable Object for Base Item Data
 * TODO: 
 * Known Bugs: 
 ****************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace AOD.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemBaseSO", menuName = "ScriptableObjects/ItemBaseSO", order = 0)]
    public class ItemBaseSO : ScriptableObject
    {
        //VARIABLES
        #region Inspector Variable Declarations and Initializations

        //Do NOT rename these Variables unless you know what you are changing
        [Header("---===========< ITEM DETAILS >===========---")]
        [SerializeField] private string Name = "New Item Name";
        [SerializeField] private string Description = "What am I?";
        [SerializeField] private int Value = 0;
        [SerializeField] private Sprite SpriteIcon = null;

        [Header("---===========< ITEM STATS >===========---")]
        [Space]
        [SerializeField] private bool IsItem = false;
        [SerializeField] private int StatBonusAmount = 0;
        [SerializeField] private bool HPBonus = false;
        [SerializeField] private bool MPBonus = false;
        [SerializeField] private bool StrBonus = false;

        [Header("---===========< WEAPON STATS >===========---")]
        [Space]
        [SerializeField] private bool IsWeapon = false;
        [SerializeField] private int AttackStrength = 0;

        [Header("---===========< ARMOR STATS >===========---")]
        [Space]
        [SerializeField] private bool IsArmor = false;
        [SerializeField] private int ArmorAmount = 0;
        [SerializeField] private ArmorSlot ArmorLocation = ArmorSlot.NotArmor;

        [Header("---===========< USEABLE BY >===========---")]
        [Space]
        [SerializeField] private List<ClassType> ClassUseable = null;

        #endregion

        //GETTERS
        #region Accessors/Getters

        public int GetValue => Value;
        public int GetArmorAmount => ArmorAmount;
        public int GetAttackStrength => AttackStrength;
        public int GetStatBonusAmount => StatBonusAmount;

        public bool GetIsItem => IsItem;
        public bool GetHPBonus => HPBonus;
        public bool GetMPBonus => MPBonus;
        public bool GetIsWeapon => IsWeapon;
        public bool GetIsArmor => IsArmor;
        public bool GetStrBonus => StrBonus;

        public string GetName => Name;
        public string GetDescription => Description;

        public Sprite GetSprite => SpriteIcon;

        public ArmorSlot GetArmorSlot => ArmorLocation;

        public List<ClassType> GetClassTypes => ClassUseable;

        #endregion
    }
}
