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

    private void Update ()
    {
		if (mCanPickup && Input.GetButtonDown(INTERACT) && PlayerController.instance.canMove)
        {
            GameManager.instance.AddItem(GetComponent<Item>().GetName);
            Destroy(gameObject);
        }
	}

    #endregion
    #region Physics Functions/MEthods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanPickup = false;
        }
    }

    #endregion
}