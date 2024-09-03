/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: InnUpstairsExit.cs
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
using UnityEngine.SceneManagement;

public class InnUpstairsExit :MonoBehaviour
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
    [SerializeField] private float waitToLoad = 1f;
    [SerializeField] private bool needBoat = false;

    #endregion
    #region Private Variables

    private string mAreaToLoad;
    private bool mDoesLoadAfterFade;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Start() => mAreaToLoad = Inn.GetDownstairsSceneName;
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update()
    {
        if (mDoesLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;

            if (waitToLoad <= 0)
            {
                mDoesLoadAfterFade = false;
                SceneManager.LoadScene(mAreaToLoad);
                Inn.SetIsPlayerUpstairs(false);
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Physics Functions/Methods

#pragma warning disable IDE0051
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            if (needBoat && !GameManager.Access.GetHasBoat)
            {
                //Cant use boat area without one.
                Debug.Log("Area needs boat but GameManager.GetHasBoat == false");
                return;
            }

            enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
            mDoesLoadAfterFade = true;
            GameManager.Access.SetFadingBetweenAreas(true);

            UIFade.instance.FadeToBlack();
        }
    }
#pragma warning restore IDE0051

    #endregion
}