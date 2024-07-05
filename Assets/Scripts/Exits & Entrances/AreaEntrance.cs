using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class AreaEntrance : MonoBehaviour {

    public string transitionName;
	public RuntimeAnimatorController playerAnimatorOverride;
	public bool isPort;
	public GameObject boatSpawn;
	public GameObject boat;

	private static RuntimeAnimatorController m_overridenAnimationController;
	private PlayerController player;

	
	void Start () {

		player = PlayerController.instance;

		if(transitionName == player.areaTransitionName)
        {
			if (isPort)
			{
                Instantiate(boat, boatSpawn.transform);
                boat.GetComponent<SpriteRenderer>().flipX = false;
                boat.GetComponent<Boat>().boatLeavingPort = false;
                boat.GetComponent<Boat>().boatEnteringPort = true;
				
			}
			else
			{
				player.transform.position = transform.position;
                player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                player.gameObject.GetComponent<Collider2D>().enabled = true;
                player.canMove = true;
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
		
	}
}
