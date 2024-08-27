﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Bonehead Games

public class ItemButton : MonoBehaviour {

    public Image buttonImage;
    public Text amountText;
    public int buttonValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Press()
    {
        if (GameMenu.instance.GetTheMenu.activeInHierarchy)
        {
            if (GameManager.instance.GetItemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.GetItemsHeld[buttonValue]));
            }
        }

        if(Shop.instance.shopMenu.activeInHierarchy)
        {
            if(Shop.instance.buyMenu.activeInHierarchy)
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }

            if(Shop.instance.sellMenu.activeInHierarchy)
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.GetItemsHeld[buttonValue]));
            }
        }
    }
}