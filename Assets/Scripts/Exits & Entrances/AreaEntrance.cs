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
    // The Proper way to expose variables to the editor
    // without exposing them outside of this script
    // [SerializeField] private variable variableName = initialvalue;
    // Example:
    // [SerializeField] private float health = 10.0f;
    public string transitionName;
	public RuntimeAnimatorController playerAnimatorOverride;
	public PortController portController;
	public bool isPort;

    #endregion
    #region Private Variable Declarations Only

    private static RuntimeAnimatorController m_overridenAnimationController;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions
    private void Start ()
	{
		if (transitionName == PlayerController.instance.areaTransitionName)
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
        GameManager.instance.fadingBetweenAreas = false;

		if (null != playerAnimatorOverride)
        {
			if (null != m_overridenAnimationController)
			{
				//store the default sprite and but only if we don't already have one
				m_overridenAnimationController = PlayerController.instance.animationController;
			}

			//replace the player sprite
			PlayerController.instance.animationController = playerAnimatorOverride;
		}
		else if ( null == playerAnimatorOverride && null != m_overridenAnimationController)
        {
			//restore the sprite if the overide field is empty
			PlayerController.instance.animationController = m_overridenAnimationController;
		}
	}

    #endregion
    #region Implementation Private Methods/Functions

    private void Update ()
	{
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
			if (!Boat.isPlayerOnBoat)
			{
                PlayerController.instance.transform.position = transform.position;
				PlayerController.instance.areaTransitionName = null;
			}
        }
    }

    #endregion
}
