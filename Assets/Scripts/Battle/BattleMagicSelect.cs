/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleMagicSelect.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private string spellName = "";
    [SerializeField] private int spellCost = 0;
    [SerializeField] private Text nameText = null;
    [SerializeField] private Text costText = null;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public Text GetNameText => nameText;
    public Text GetCostText => costText;
    public int GetSpellCost => spellCost;
    public string GetSpellName => spellName;

    #endregion
    #region Setters/Mutators

    public int SetSpellCost(int cost) => spellCost = cost;
    public string SetSpellName(string newName) => spellName = newName;

    #endregion

    //FUNCTIONS
    #region Public Functions/Methods

    public void Press()
    {
        if (BattleManager.instance.GetActiveBattlers[BattleManager.instance.GetCurrentTurn].GetCurrentMP >= spellCost)
        {
            BattleManager.instance.GetMagicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.GetActiveBattlers[BattleManager.instance.GetCurrentTurn].SetCurrentMP(BattleManager.instance.GetActiveBattlers[BattleManager.instance.GetCurrentTurn].GetCurrentMP - spellCost);
        }
        else
        {
            //let player know there is not enough MP
            BattleManager.instance.GetBattleNotice.theText.text = "Not Enough MP!";
            BattleManager.instance.GetBattleNotice.Activate();
            BattleManager.instance.GetMagicMenu.SetActive(false);
        }
    }

    #endregion
}