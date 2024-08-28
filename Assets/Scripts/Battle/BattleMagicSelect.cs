/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleMagicSelect.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in BattleManager/MagicMenu/SpellButtons to set spells available
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class BattleMagicSelect : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [FormerlySerializedAs("nameText")]
    [SerializeField] private Text SpellNameText = null;
    [FormerlySerializedAs("costText")]
    [SerializeField] private Text SpellCostText = null;

    #endregion
    #region Private Variables

    private int mSpellCost;
    private string mSpellName;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetSpellCost => mSpellCost;
    public string GetSpellName => mSpellName;
    public Text GetSpellCostText => SpellCostText;
    public Text GetSpellNameText => SpellNameText;

    #endregion
    #region Setters/Mutators

    public int SetSpellCost(int cost) => mSpellCost = cost;
    public string SetSpellName(string newName) => mSpellName = newName;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Start() => InitializeVariables();
#pragma warning restore IDE0051

    private void InitializeVariables()
    {
        mSpellCost = 0;
        mSpellName = "";
    }

    #endregion
    #region Public Functions/Methods

    public void Press()
    {
        if (BattleManager.Access.GetActiveBattlers[BattleManager.Access.GetCurrentTurn].GetCurrentMP >= mSpellCost)
        {
            BattleManager.Access.GetMagicMenu.SetActive(false);
            BattleManager.Access.OpenTargetMenu(mSpellName);
            BattleManager.Access.GetActiveBattlers[BattleManager.Access.GetCurrentTurn].SetCurrentMP(BattleManager.Access.GetActiveBattlers[BattleManager.Access.GetCurrentTurn].GetCurrentMP - mSpellCost);
        }
        else
        {
            //let player know there is not enough MP
            BattleManager.Access.GetBattleNotice.SetNotificationText("Not Enough MP!");
            BattleManager.Access.GetBattleNotice.Activate();
            BattleManager.Access.GetMagicMenu.SetActive(false);
        }
    }

    #endregion
}