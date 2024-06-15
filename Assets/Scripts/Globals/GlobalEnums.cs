/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: GlobalEnums.cs
 * Date Created: June 14, 2024
 * Created By: Jeff Moreau
 * Modified By:
 * Date Last Modified:
 * Description: Globaly sourced Enums
 * TODO: 
 * Known Bugs: 
 ****************************************************************************************/

namespace AOD
{
    #region Enumerator Declarations

    public enum ArmorSlot
    {
        NotArmor = -1,
        Head,
        Body,
        Hands,
        Legs,
        Feet,
        Other,
        Shield
    }

    public enum ClassType
    {
        Cleric,
        Druid,
        Fighter,
        Mage,
        Paladin,
        Ranger,
        Thief
    }

    #endregion
}
