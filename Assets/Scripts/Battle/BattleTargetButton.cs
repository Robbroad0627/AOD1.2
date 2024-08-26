/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleTargetButton.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Text targetName;

    #endregion
    #region Private Variables

    private string mMoveName;
    private int mTarget;

    #endregion

    //GETTERS/SETTERS
    #region Setters/Mutators

    public int SetTarget(int target) => mTarget = target;
    public string SetMoveName(string name) => mMoveName = name;
    public string SetTargetName(string name) => targetName.text = name;

    #endregion

    //FUNCTIONS
    #region Buttons

    public void Press()
    {
        BattleManager.instance.PlayerAttack(mMoveName, mTarget);
    }

    #endregion
}