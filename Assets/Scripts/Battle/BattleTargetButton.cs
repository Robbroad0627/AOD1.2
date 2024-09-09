/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleTargetButton.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Used in BattleManager Prefab under Canvas/Target Menu/buttons
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: September 9, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour
{
    //VARIABLES
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Text targetName;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private int mTarget;
    private string mMoveName;

    #endregion

    //GETTERS/SETTERS
    #region Public Setters/Mutators for use Outside of this Class Only

    public int SetTarget(int target) => mTarget = target;
    public string SetMoveName(string name) => mMoveName = name;
    public string SetTargetName(string name) => targetName.text = name;

    #endregion

    //FUNCTIONS
    #region Public Functions/Methods for use with Buttons

    public void Press() => BattleManager.Access.PlayerAttack(mMoveName, mTarget);

    #endregion
}