/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: Shop.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 29, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;
    public Text goldText;
    public string[] itemsForSale;
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;
    public Item selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

	void Start ()
    {
        instance = this;
	}
	
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
        {
            OpenShop();
        }
	}

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();
        GameManager.Access.SetShopActive(true);
        goldText.text = GameManager.Access.GetCurrentGold.ToString() + "g";
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.Access.SetShopActive(false);
    }

    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press();
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].SetValue(i);

            if (itemsForSale[i] != "")
            {
                buyItemButtons[i].GetImage.gameObject.SetActive(true);
                buyItemButtons[i].GetImage.sprite = GameManager.Access.GetItemDetails(itemsForSale[i]).GetSprite;
                buyItemButtons[i].GetAmount.text = "";
            }
            else
            {
                buyItemButtons[i].GetImage.gameObject.SetActive(false);
                buyItemButtons[i].GetAmount.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
        ShowSellItems();
    }

    private void ShowSellItems()
    {
        GameManager.Access.SortItems();
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].SetValue(i);

            if (GameManager.Access.GetItemsHeld[i] != "")
            {
                sellItemButtons[i].GetImage.gameObject.SetActive(true);
                sellItemButtons[i].GetImage.sprite = GameManager.Access.GetItemDetails(GameManager.Access.GetItemsHeld[i]).GetSprite;
                sellItemButtons[i].GetAmount.text = GameManager.Access.GetNumberOfItems[i].ToString();
            }
            else
            {
                sellItemButtons[i].GetImage.gameObject.SetActive(false);
                sellItemButtons[i].GetAmount.text = "";
            }
        }
    }

    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.GetName;
        buyItemDescription.text = selectedItem.GetDescription;
        buyItemValue.text = "Value: " + selectedItem.GetValue + "g";
    }

    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
        sellItemName.text = selectedItem.GetName;
        sellItemDescription.text = selectedItem.GetDescription;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.GetValue * .5f).ToString() + "g";
    }

    public void BuyItem()
    {
        if (selectedItem != null)
        {
            if (GameManager.Access.GetCurrentGold >= selectedItem.GetValue)
            {
                GameManager.Access.SetCurrentGold(GameManager.Access.GetCurrentGold - selectedItem.GetValue);

                GameManager.Access.AddItem(selectedItem.GetName);
            }
        }

        goldText.text = GameManager.Access.GetCurrentGold.ToString() + "g";
    }

    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.Access.SetCurrentGold(GameManager.Access.GetCurrentGold + Mathf.FloorToInt(selectedItem.GetValue * .5f));

            GameManager.Access.RemoveItem(selectedItem.GetName);
        }

        goldText.text = GameManager.Access.GetCurrentGold.ToString() + "g";

        ShowSellItems();
    }
}