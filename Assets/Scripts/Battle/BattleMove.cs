/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleMove.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using System;
using UnityEngine;

[Serializable]
public class BattleMove
{
    public enum CharacterClass
    {
        Cleric,
        Druid,
        Fighter,
        MagicUser,
        Paladin,
        Ranger,
        Thief
    }

    public enum MoveFilterMode
    {
        DisallowMoveForClassIfNotListed,
        OverrideDefaultLevelForClass
    }

    [Serializable]
    public class MoveFilter
    {
        public CharacterClass characterClass;
        public int minimumLevel;
    }

    public string moveName;
    public int movePower;
    public int moveCost;
    public bool allowOutsideBattle;
    public AttackEffect theEffect;

    [Header("Move Filter")]
    public MoveFilterMode filterAction;
    public int defaultMinimumLevel = 0;
    [UnityEngine.Serialization.FormerlySerializedAs("prerequisite")]
    public MoveFilter[] classSpecificFilter;


    public bool MoveAllowedOutsideBattle(CharStats character)
    {
        return MoveAllowed(character) && allowOutsideBattle;
    }

    public bool MoveAllowed(CharStats character)
    {
        int? lvl = MinLevelForClass(character.GetClass);
#pragma warning disable CS0162 // Unreachable code detected
        switch (filterAction)
        {
            case MoveFilterMode.DisallowMoveForClassIfNotListed:
                return lvl != null && character.GetLevel >= lvl;
                break;

            case MoveFilterMode.OverrideDefaultLevelForClass:
                return lvl == null? character.GetLevel >= defaultMinimumLevel : character.GetLevel >= lvl;
                break;
        }
#pragma warning restore CS0162 // Unreachable code detected
        return false;
        
    }

    private int? MinLevelForClass(CharacterClass characterClass)
    {
        foreach(var f in classSpecificFilter)
        {
            if(f.characterClass == characterClass)
            {
                return f.minimumLevel;
            }
        }
        return null;
    }

    internal void Apply(CharStats charStats)
    {
        throw new NotImplementedException();
    }
}