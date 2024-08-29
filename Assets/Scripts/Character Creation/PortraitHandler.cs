/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: PortraitHandler.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 29, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: This really should be a ScriptableObject
 ****************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class PortraitHandler : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private List<Sprite> mDwarf;
    [SerializeField] private List<Sprite> fDwarf;
    [SerializeField] private List<Sprite> mElf;
    [SerializeField] private List<Sprite> fElf;
    [SerializeField] private List<Sprite> mHalf_Elf;
    [SerializeField] private List<Sprite> fHalf_Elf;
    [SerializeField] private List<Sprite> mHalf_Orc;
    [SerializeField] private List<Sprite> fHalf_Orc;
    [SerializeField] private List<Sprite> mHalfling;
    [SerializeField] private List<Sprite> fHalfling;
    [SerializeField] private List<Sprite> mHuman;
    [SerializeField] private List<Sprite> fHuman;

    #endregion

    //GETTERS
    #region Getters/Accessors

    public List<Sprite> GetDwarfMale => mDwarf;
    public List<Sprite> GetDwarfFemale => fDwarf;
    public List<Sprite> GetElfMale => mElf;
    public List<Sprite> GetElfFemale => fElf;
    public List<Sprite> GetHalfElfMale => mHalf_Elf;
    public List<Sprite> GetHalfElfFemale => fHalf_Elf;
    public List<Sprite> GetHalfOrcMale => mHalf_Orc;
    public List<Sprite> GetHalfOrcFemale => fHalf_Orc;
    public List<Sprite> GetHalflingMale => mHalfling;
    public List<Sprite> GetHalflingFemale => fHalfling;
    public List<Sprite> GetHumanMale => mHuman;
    public List<Sprite> GetHumanFemale => fHuman;

    #endregion
}