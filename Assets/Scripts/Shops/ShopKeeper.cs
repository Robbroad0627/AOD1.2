﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class ShopKeeper : MonoBehaviour {

    private bool canOpen;

    public string[] ItemsForSale = new string[40];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.GetCanMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            Shop.instance.itemsForSale = ItemsForSale;

            Shop.instance.OpenShop();
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
