/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AreaExit.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in all Area Exit Prefabs
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;
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
    #region Private Variable Declarations Only

    private bool mShouldLoadAfterFade;
    private bool mShouldRunAnimationBeforeFade;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Start() => InitializeVariables();
#pragma warning restore IDE0051

    private void InitializeVariables()
    {
        mShouldLoadAfterFade = false;
        mShouldRunAnimationBeforeFade = false;
    }

    #endregion
    #region Physics Methods/Functions

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
    #region Implementation Private Methods/Functions

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
            if (Boat.boatLeftPort)
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