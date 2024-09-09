/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: CharacterAttributes.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public enum BaseAttributes
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    };

    public enum Races
    {
        Dwarf,
        Elf,
        HalfElf,
        Halfling,
        HalfOrc,
        Human,
    };

    public enum Classes
    {
        Cleric,
        Druid,
        Fighter,
        MagicUser,
        Paladin,
        Ranger,
        Thief
    };

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
}