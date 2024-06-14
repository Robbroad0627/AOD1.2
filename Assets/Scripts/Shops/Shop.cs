using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Bonehead Games

namespace AOD
{
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

        // Use this for initialization
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
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

            GameManager.instance.shopActive = true;

            goldText.text = GameManager.instance.currentGold.ToString() + "g";
        }

        public void CloseShop()
        {
            shopMenu.SetActive(false);
            GameManager.instance.shopActive = false;
        }

        public void OpenBuyMenu()
        {
            buyItemButtons[0].Press();

            buyMenu.SetActive(true);
            sellMenu.SetActive(false);

            for (int i = 0; i < buyItemButtons.Length; i++)
            {
                buyItemButtons[i].buttonValue = i;

                if (itemsForSale[i] != "")
                {
                    buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                    buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).GetItemData.GetSprite;
                    buyItemButtons[i].amountText.text = "";
                }
                else
                {
                    buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                    buyItemButtons[i].amountText.text = "";
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
            GameManager.instance.SortItems();
            for (int i = 0; i < sellItemButtons.Length; i++)
            {
                sellItemButtons[i].buttonValue = i;

                if (GameManager.instance.itemsHeld[i] != "")
                {
                    sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                    sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).GetItemData.GetSprite;
                    sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
                }
                else
                {
                    sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                    sellItemButtons[i].amountText.text = "";
                }
            }
        }

        public void SelectBuyItem(Item buyItem)
        {
            selectedItem = buyItem;
            buyItemName.text = selectedItem.GetItemData.GetName;
            buyItemDescription.text = selectedItem.GetItemData.GetDescription;
            buyItemValue.text = "Value: " + selectedItem.GetItemData.GetValue + "g";
        }

        public void SelectSellItem(Item sellItem)
        {
            selectedItem = sellItem;
            sellItemName.text = selectedItem.GetItemData.GetName;
            sellItemDescription.text = selectedItem.GetItemData.GetDescription;
            sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.GetItemData.GetValue * .5f).ToString() + "g";
        }

        public void BuyItem()
        {
            if (selectedItem != null)
            {
                if (GameManager.instance.currentGold >= selectedItem.GetItemData.GetValue)
                {
                    GameManager.instance.currentGold -= selectedItem.GetItemData.GetValue;

                    GameManager.instance.AddItem(selectedItem.GetItemData.GetName);
                }
            }

            goldText.text = GameManager.instance.currentGold.ToString() + "g";
        }

        public void SellItem()
        {
            if (selectedItem != null)
            {
                GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.GetItemData.GetValue * .5f);

                GameManager.instance.RemoveItem(selectedItem.GetItemData.GetName);
            }

            goldText.text = GameManager.instance.currentGold.ToString() + "g";

            ShowSellItems();
        }
    }
}