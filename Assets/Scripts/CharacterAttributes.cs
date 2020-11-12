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
        Half_Elf,
        Halfling,
        Half_Orc,
        Human,
    };

    public enum Classes
    {
        Cleric,
        Druid,
        Fighter,
        Magic_User,
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
