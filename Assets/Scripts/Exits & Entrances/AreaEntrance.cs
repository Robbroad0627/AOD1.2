/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AreaEntrance.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in all Area Entrance Prefabs
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;

public class AreaEntrance : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [FormerlySerializedAs("transitionName")]
    [SerializeField] private string SpawnPointName = "";
    [FormerlySerializedAs("isPort")]
    [SerializeField] private bool IsAPort = false;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public string GetSpawnPointName => SpawnPointName;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

#pragma warning disable IDE0051
    private void Start ()
	{
		if (PlayerController.Access.GetAreaTransitionName == SpawnPointName)
        {
			if (IsAPort && Boat.boatLeftPort)
			{
				Boat.isEnteringPort = true;
			}
			else
			{
				PlayerController.Access.transform.position = transform.position;
            }
        }

        UIFade.instance.FadeFromBlack();
        GameManager.Access.SetFadingBetweenAreas(false);
	}
#pragma warning restore IDE0051

    #endregion
    #region Implementation Private Methods/Functions

#pragma warning disable IDE0051
    private void Update ()
	{
        if (PlayerController.Access.GetAreaTransitionName == SpawnPointName)
        {
			if (!Boat.isPlayerOnBoat)
			{
                PlayerController.Access.transform.position = transform.position;
				PlayerController.Access.SetAreaTransitionName(null);
			}
        }
    }
#pragma warning restore IDE0051

    #endregion
}