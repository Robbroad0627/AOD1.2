/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AreaEntrance.cs
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

public class AreaEntrance : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private string transitionName = "";
	[SerializeField] private bool isPort = false;
	[SerializeField] private PortController portController = null;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public string GetTransitionName => transitionName;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions
    private void Start ()
	{
		if (transitionName == PlayerController.instance.GetAreaTransitionName)
        {
			if (isPort && Boat.boatLeftPort)
			{
				Boat.isEnteringPort = true;
			}
			else
			{
				PlayerController.instance.transform.position = transform.position;
            }
        }

        UIFade.instance.FadeFromBlack();
        GameManager.instance.SetFadingBetweenAreas(false);
	}

    #endregion
    #region Implementation Private Methods/Functions

    private void Update ()
	{
        if (transitionName == PlayerController.instance.GetAreaTransitionName)
        {
			if (!Boat.isPlayerOnBoat)
			{
                PlayerController.instance.transform.position = transform.position;
				PlayerController.instance.SetAreaTransitionName(null);
			}
        }
    }

    #endregion
}