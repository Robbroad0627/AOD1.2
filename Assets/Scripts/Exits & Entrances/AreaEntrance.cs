/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AreaEntrance.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Used in all Area Entrance Prefabs
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 28, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;

public class AreaEntrance : MonoBehaviour
{
    //VARIABLES
    #region Private Variables/Fields Exposed to Inspector for Editing

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
    #region Public Getters/Accessors for use Outside of this Class Only

    public string GetSpawnPointName => SpawnPointName;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only 

#pragma warning disable IDE0051
    private void Start ()
	{
		if (PlayerController.Access.GetAreaTransitionName == SpawnPointName)
        {
			if (IsAPort && Boat.mHasLeftPort)
			{
				Boat.mIsEnteringPort = true;
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
    #region Private Implementation Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Update ()
	{
        if (PlayerController.Access.GetAreaTransitionName == SpawnPointName)
        {
			if (!Boat.mIsPlayerOnBoat)
			{
                PlayerController.Access.transform.position = transform.position;
				PlayerController.Access.SetAreaTransitionName(null);
			}
        }
    }
#pragma warning restore IDE0051

    #endregion
}