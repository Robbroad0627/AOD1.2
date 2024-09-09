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
using UnityEngine.Serialization;

public enum MoveFilterMode
{
    DisallowMoveForClassIfNotListed,
    OverrideDefaultLevelForClass
}

[Serializable]
public class BattleMove
{
    [Serializable]
    public class MoveFilter
    {
        public CharacterAttributes.CharacterClass characterClass;
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
    [FormerlySerializedAs("prerequisite")]
    public MoveFilter[] classSpecificFilter;

    public bool MoveAllowed(CharStats character)
    {
        if (moveName != "Slash")
        {
            int? level = MinLevelForClass(character.GetClass);

            switch (filterAction)
            {
                case MoveFilterMode.DisallowMoveForClassIfNotListed:
                    return level != null && character.GetLevel >= level;

                case MoveFilterMode.OverrideDefaultLevelForClass:
                    return level == null ? character.GetLevel >= defaultMinimumLevel : character.GetLevel >= level;
            }
        }

        return false;
    }

    private int? MinLevelForClass(CharacterAttributes.CharacterClass characterClass)
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