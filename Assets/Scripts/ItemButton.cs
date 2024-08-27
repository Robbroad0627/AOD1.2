/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: InnUpstairsExit.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 27, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Image buttonImage = null;
    [SerializeField] private Text amountText = null;
    [SerializeField] private int buttonValue = 0;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetValue => buttonValue;
    public Text GetAmount => amountText;
    public Image GetImage => buttonImage;

    #endregion
    #region Setters/Mutators

    public int SetValue(int amount) => buttonValue = amount;

    #endregion

    //FUNCTIONS
    #region Public Functions/Methods

    public void Press()
    {
        if (GameMenu.instance.GetTheMenu.activeInHierarchy)
        {
            if (GameManager.instance.GetItemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.GetItemsHeld[buttonValue]));
            }
        }

        if (Shop.instance.shopMenu.activeInHierarchy)
        {
            if (Shop.instance.buyMenu.activeInHierarchy)
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }

            if (Shop.instance.sellMenu.activeInHierarchy)
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.GetItemsHeld[buttonValue]));
            }
        }
    }

    #endregion
}