using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class AreaEntrance : MonoBehaviour {

    public string transitionName;
	public RuntimeAnimatorController playerAnimatorOverride;
	public PortController portController;
	public bool isPort;

	private static RuntimeAnimatorController m_overridenAnimationController;

	
	void Start () {

		if(transitionName == PlayerController.instance.areaTransitionName)
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

		if(null != playerAnimatorOverride)
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
	
	
	void Update () {
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
			if (!Boat.isPlayerOnBoat)
			{
                PlayerController.instance.transform.position = transform.position;
				PlayerController.instance.areaTransitionName = null;
			}
        }
    }
}
