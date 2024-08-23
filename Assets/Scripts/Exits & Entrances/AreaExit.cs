/****************************************************************************************
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
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    // The Proper way to expose variables to the editor
    // without exposing them outside of this script
    // [SerializeField] private variable variableName = initialvalue;
    // Example:
    // [SerializeField] private float health = 10.0f;
    public string areaToLoad;
    public string areaTransitionName;
    public float waitToLoad = 1f;
    public PortController portController;
    public bool needBoat = false;

    #endregion
    #region Private Variable Declarations Only

    private bool shouldLoadAfterFade;
    private bool shouldRunAnimationBeforeFade;

    #endregion

    //FUNCTIONS
    #region Physics Methods/Functions

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (needBoat && !GameManager.instance.haveBoat)
            {
                //Cant use boat area without one.
                Debug.Log("Area needs boat but GameManager.haveBoat == false");
                return;
            }
            else if (needBoat && GameManager.instance.haveBoat)
            {
                portController.PlayerEnterBoat(areaToLoad, areaTransitionName);                
                //shouldRunAnimationBeforeFade = true;
            }
            else if (!needBoat)
            {
                this.enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
                                    //SceneManager.LoadScene(areaToLoad);
                shouldLoadAfterFade = true;
                GameManager.instance.fadingBetweenAreas = true;

                UIFade.instance.FadeToBlack();

                PlayerController.instance.areaTransitionName = areaTransitionName;
            }
        }
    }

    #endregion
    #region Implementation Private Methods/Functions

    private void Update ()
    {
		if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;

            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }

        if (shouldRunAnimationBeforeFade)
        {
            if (Boat.boatLeftPort)
            {
                this.enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
                                    //SceneManager.LoadScene(areaToLoad);
                shouldLoadAfterFade = true;
                GameManager.instance.fadingBetweenAreas = true;

                UIFade.instance.FadeToBlack();

                PlayerController.instance.areaTransitionName = areaTransitionName;
                shouldRunAnimationBeforeFade = false;
            }
        }
    }

    #endregion
}
