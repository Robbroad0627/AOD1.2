﻿/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: DestroyOverLifetime.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class DestroyOverLifetime : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private float lifetime;

    #endregion

    //FUNCTIONS
    #region Implementation Functions/Methods

    private void Update ()
    {
        Destroy(gameObject, lifetime);
	}

    #endregion
}