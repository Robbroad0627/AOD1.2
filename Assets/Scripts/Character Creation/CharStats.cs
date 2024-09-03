/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: CharStats.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using System;
using UnityEngine;
using System.Collections.Generic;

public class CharStats : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private string charName = "?";
    [SerializeField] private BattleChar battleChar = null;
    [SerializeField] private BattleMove.CharacterClass characterClass = 0;
    [SerializeField] private int playerLevel = 1;
    [SerializeField] private int currentEXP = 0;
    [SerializeField] private int[] expToNextLevel = null;
    [SerializeField] private int maxLevel = 100;
    [SerializeField] private int baseEXP = 1000;
    [SerializeField] private int currentHP = 0;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentMP = 0;
    [SerializeField] private int maxMP = 30;
    [SerializeField] private int[] mpLvlBonus = null;
    [SerializeField] private int strength = 0;
    [SerializeField] private int defence = 0;
    [SerializeField] private int wpnPwr = 0;
    [SerializeField] private int armrPwr = 0;
    [SerializeField] private string equippedWpn = null;
    [SerializeField] private string equippedArmr = null;
    [SerializeField] private Sprite charIamge = null;

    #endregion
    #region Private Variables

    private string mSex;
    private string mRace;
    private string mEquippedHeadArmr;
    private string mEquippedBodyArmr;
    private string mEquippedHandArmr;
    private string mEquippedLegsArmr;
    private string mEquippedFeetArmr;
    private string mEquippedOtherArmr;
    private string mEquippedShieldArmr;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetMaxHP => maxHP;
    public int GetMaxMP => maxMP;
    public string GetSex => mSex;
    public string GetRace => mRace;
    public int GetDefence => defence;
    public int GetLevel => playerLevel;
    public int GetStrength => strength;
    public int GetWeaponPower => wpnPwr;
    public int GetArmorPower => armrPwr;
    public int GetCurrentHP => currentHP;
    public int GetCurrentMP => currentMP;
    public Sprite GetSprite => charIamge;
    public int GetCurrentXP => currentEXP;
    public string GetCharacterName => charName;
    public string GetEquippedWeapon => equippedWpn;
    public string GetEquippedArmor => equippedArmr;
    public int[] GetXPToNextLevel => expToNextLevel;
    public BattleChar GetBattleCharacter => battleChar;
    public string GetEquippedLegArmor => mEquippedLegsArmr;
    public string GetEquippedShield => mEquippedShieldArmr;
    public string GetEquippedHeadArmor => mEquippedHandArmr;
    public string GetEquippedBodyArmor => mEquippedBodyArmr;
    public string GetEquippedHandArmor => mEquippedHandArmr;
    public string GetEquippedFootArmor => mEquippedFeetArmr;
    public string GetEquippedOtherArmor => mEquippedOtherArmr;
    public BattleMove.CharacterClass GetClass => characterClass;

    #endregion
    #region Setters/Mutators

    public int SetMaxHP(int hp) => maxHP = hp;
    public int SetMaxMP(int mp) => maxMP = mp;
    public string SetSex(string sex) => mSex = sex;
    public int SetCurrentMP(int mp) => currentMP = mp;
    public int SetCurrentHP(int hp) => currentHP = hp;
    public int SetCurrentXP(int xp) => currentEXP = xp;
    public string SetRace(string race) => mRace = race;
    public int SetLevel(int newLevel) => playerLevel = newLevel;
    public int SetDefense(int newDefense) => defence = newDefense;
    public string SetCharacterName(string name) => charName = name;
    public int SetArmorPower(int armorPower) => armrPwr = armorPower;
    public int SetStrength(int newStrength) => strength = newStrength;
    public int SetWeaponPower(int weaponPower) => wpnPwr = weaponPower;
    public Sprite SetSprite(Sprite newSprite) => charIamge = newSprite;
    public string SetEquippedArmor(string newArmor) => equippedArmr = newArmor;
    public string SetEquippedWeapon(string newWeapon) => equippedWpn = newWeapon;
    public string SetEquippedShield(string newShield) => mEquippedShieldArmr = newShield;
    public string SetEquippedOtherArmor(string newOther) => mEquippedOtherArmr = newOther;
    public string SetEquippedLegArmor(string newLegArmor) => mEquippedLegsArmr = newLegArmor;
    public string SetEquippedHeadArmor(string newHeadArmor) => mEquippedHeadArmr = newHeadArmor;
    public string SetEquippedBodyArmor(string newBodyArmor) => mEquippedBodyArmr = newBodyArmor;
    public string SetEquippedFootArmor(string newFootArmor) => mEquippedFeetArmr = newFootArmor;
    public string SetEquippedHandArmor(string newHandArmor) => mEquippedHandArmr = newHandArmor;
    public BattleChar SetBattleCharacter(BattleChar newBattleChar) => battleChar = newBattleChar;
    public BattleMove.CharacterClass SetClass(string newClass) => characterClass = (BattleMove.CharacterClass)Enum.Parse(typeof(BattleMove.CharacterClass), newClass, true);

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Start ()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
	}
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update ()
    {
        // Remove before final compile
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
	}
#pragma warning restore IDE0051

    #endregion
    #region Public Functions/Methods

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //determine whether to add to str or def based on odd or even level
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                maxMP += mpLvlBonus[playerLevel];
                currentMP = maxMP;
            }
        }

        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }

    public bool ApplyMove(CharStats character, BattleMove action)
    {
        if (character.currentMP >= action.moveCost)
        {
            character.currentMP -= action.moveCost;
            return true;
        }

        return false;
    }

    public string[] GetAllowedMovesNames(BattleMove[] actionList)
    {
        List<string> allowedActionsList = new List<string>();

        foreach (BattleMove action in actionList)
        {
            if (action.MoveAllowed(this))
            {
                allowedActionsList.Add(action.moveName);
            }
        }

        return allowedActionsList.ToArray();
    }

    #endregion
}