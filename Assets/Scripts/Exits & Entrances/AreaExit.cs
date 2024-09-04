/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AreaExit.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Used in all Area Exit Prefabs
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 28, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    //VARIABLES
    #region Private Constant Variables/Fields used in this Class Only

    private const string PLAYER = "Player";

    #endregion
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [FormerlySerializedAs("areaToLoad")]
    [SerializeField] private string SceneToLoad = "";
    [FormerlySerializedAs("areaTransitionName")]
    [SerializeField] private string EntranceSpawnPointName = "";
    [FormerlySerializedAs("waitToLoad")]
    [SerializeField] private float WaitToLoadDuration = 1.0f;
    [FormerlySerializedAs("needBoat")]
    [SerializeField] private bool PlayerNeedsBoat = false;
    [FormerlySerializedAs("portController")]
    [SerializeField] private PortController ThePortController = null;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private bool mShouldLoadAfterFade;
    private bool mShouldRunAnimationBeforeFade;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Start() => InitializeVariables();
#pragma warning restore IDE0051

    private void InitializeVariables()
    {
        mShouldLoadAfterFade = false;
        mShouldRunAnimationBeforeFade = false;
    }

    #endregion
    #region Private Physics Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            if (PlayerNeedsBoat && !GameManager.Access.GetHasBoat)
            {
                Debug.Log("Area needs a Boat but Player does not have one.");
                return;
            }
            else if (PlayerNeedsBoat && GameManager.Access.GetHasBoat)
            {
                ThePortController.PlayerEnterBoat(SceneToLoad, EntranceSpawnPointName);                
            }
            else if (!PlayerNeedsBoat)
            {
                enabled = true;
                mShouldLoadAfterFade = true;
                GameManager.Access.SetFadingBetweenAreas(true);
                UIFade.instance.FadeToBlack();
                PlayerController.Access.SetAreaTransitionName(EntranceSpawnPointName);
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Private Implementation Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Update ()
    {
		if (mShouldLoadAfterFade)
        {
            WaitToLoadDuration -= Time.deltaTime;

            if (WaitToLoadDuration <= 0)
            {
                mShouldLoadAfterFade = false;
                SceneManager.LoadScene(SceneToLoad);
            }
        }

        if (mShouldRunAnimationBeforeFade)
        {
            if (Boat.Access.GetHasLeftPort)
            {
                enabled = true;
                mShouldLoadAfterFade = true;
                GameManager.Access.SetFadingBetweenAreas(true);
                UIFade.instance.FadeToBlack();
                PlayerController.Access.SetAreaTransitionName(EntranceSpawnPointName);
                mShouldRunAnimationBeforeFade = false;
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
}