/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: PickupItem.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 27, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string PLAYER = "Player";
    private const string INTERACT = "Fire1";

    #endregion
    #region Private Variables

    private bool mCanPickup;

    #endregion

    //FUNCTIONS
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update ()
    {
		if (mCanPickup && Input.GetButtonDown(INTERACT) && PlayerController.Access.GetCanMove)
        {
            GameManager.Access.AddItem(GetComponent<Item>().GetName);
            Destroy(gameObject);
        }
	}
#pragma warning restore IDE0051

    #endregion
    #region Physics Functions/MEthods

#pragma warning disable IDE0051
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanPickup = true;
        }
    }
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanPickup = false;
        }
    }
#pragma warning restore IDE0051

    #endregion
}