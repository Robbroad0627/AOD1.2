/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleMove.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Used in BattleManager Prefab under List Of Attacks
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: September 3, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: This needs a complete rewrite
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

    public bool MoveAllowed(CharStats character)
    {
        if (moveName != "Slash")
        {
            int? lvl = MinLevelForClass(character.GetClass);

            switch (filterAction)
            {
                case MoveFilterMode.DisallowMoveForClassIfNotListed:
                    return lvl != null && character.GetLevel >= lvl;

                case MoveFilterMode.OverrideDefaultLevelForClass:
                    return lvl == null ? character.GetLevel >= defaultMinimumLevel : character.GetLevel >= lvl;
            }
        }

        return false;
    }

    private int? MinLevelForClass(CharacterClass characterClass)
    {
        foreach (MoveFilter attack in classSpecificFilter)
        {
            if (attack.characterClass == characterClass)
            {
                return attack.minimumLevel;
            }
        }

        return null;
    }
}