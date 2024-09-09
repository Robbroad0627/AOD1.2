/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleMagicSelect.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Used in BattleManager Prefab under MagicMenu/SpellButtons to set spells available
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 28, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class BattleMagicSelect : MonoBehaviour
{
    //VARIABLES
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [FormerlySerializedAs("nameText")]
    [SerializeField] private Text SpellNameText = null;
    [FormerlySerializedAs("costText")]
    [SerializeField] private Text SpellCostText = null;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private int mSpellCost;
    private string mSpellName;

    #endregion

    //GETTERS/SETTERS
    #region Public Getters/Accessors for use Outside of this Class Only

    public int GetSpellCost => mSpellCost;
    public string GetSpellName => mSpellName;
    public Text GetSpellCostText => SpellCostText;
    public Text GetSpellNameText => SpellNameText;

    #endregion
    #region Public Setters/Mutators for use Outside of this Class Only

    public int SetSpellCost(int cost) => mSpellCost = cost;
    public string SetSpellName(string newName) => mSpellName = newName;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only

    #pragma warning disable IDE0051
    private void Start() => InitializeVariables();
    #pragma warning restore IDE0051

    private void InitializeVariables()
    {
        mSpellCost = 0;
        mSpellName = "";
    }

    #endregion
    #region Public Functions/Methods for use with Buttons

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