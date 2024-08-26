﻿/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: DamageNumber.cs
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
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour 
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Text damageText = null;
    [SerializeField] private float lifetime = 1.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float placementJitter = 0.5f;

    #endregion

    //FUNCTIONS
    #region Implementation Functions/Methods

    private void Update ()
    {
        Destroy(gameObject, lifetime);
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
	}

    #endregion
    #region Public Functions/Methods

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
    }

    #endregion
}