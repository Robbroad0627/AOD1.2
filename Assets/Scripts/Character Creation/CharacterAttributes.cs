using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

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

    
    public enum Vitals
    {
        HP,
        MP,
        AC
    };
}
