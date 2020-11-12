using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class AreaEntrance : MonoBehaviour {

    public string transitionName;

	
	void Start () {
		if(transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
        }

        UIFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
	}
	
	
	void Update () {
		
	}
}
