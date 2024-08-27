﻿/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AreaExit.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string PLAYER = "Player";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private string areaToLoad = "";
    [SerializeField] private string areaTransitionName = "";
    [SerializeField] private float waitToLoad = 1f;
    [SerializeField] private bool needBoat = false;
    [SerializeField] private PortController portController = null;

    #endregion
    #region Private Variable Declarations Only

    private bool mShouldLoadAfterFade;
    private bool mShouldRunAnimationBeforeFade;

    #endregion

    //FUNCTIONS
    #region Physics Methods/Functions

#pragma warning disable IDE0051
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            if (needBoat && !GameManager.Access.GetHasBoat)
            {
                //Cant use boat area without one.
                Debug.Log("Area needs boat but GameManager.haveBoat == false");
                return;
            }
            else if (needBoat && GameManager.Access.GetHasBoat)
            {
                portController.PlayerEnterBoat(areaToLoad, areaTransitionName);                
                //shouldRunAnimationBeforeFade = true;
            }
            else if (!needBoat)
            {
                enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
                                    //SceneManager.LoadScene(areaToLoad);
                mShouldLoadAfterFade = true;
                GameManager.Access.SetFadingBetweenAreas(true);

                UIFade.instance.FadeToBlack();

                PlayerController.Access.SetAreaTransitionName(areaTransitionName);
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Implementation Private Methods/Functions

#pragma warning disable IDE0051
    private void Update ()
    {
		if (mShouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;

            if (waitToLoad <= 0)
            {
                mShouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }

        if (mShouldRunAnimationBeforeFade)
        {
            if (Boat.boatLeftPort)
            {
                enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
                                    //SceneManager.LoadScene(areaToLoad);
                mShouldLoadAfterFade = true;
                GameManager.Access.SetFadingBetweenAreas(true);

                UIFade.instance.FadeToBlack();

                PlayerController.Access.SetAreaTransitionName(areaTransitionName);
                mShouldRunAnimationBeforeFade = false;
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
}