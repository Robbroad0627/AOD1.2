/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: ShopKeeper.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 29, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    private bool canOpen;
    public string[] ItemsForSale = new string[40];

	void Update ()
    {
		if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.Access.GetCanMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            Shop.instance.itemsForSale = ItemsForSale;
            Shop.instance.OpenShop();
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
        }
    }
}