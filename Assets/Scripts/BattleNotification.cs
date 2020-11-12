using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Bonehead Games

public class BattleNotification : MonoBehaviour {

    public float awakeTime;
    private float awakeCounter;
    public Text theText;

	
	void Start () {
		
	}
	
	
	void Update () {
		if(awakeCounter > 0)
        {
            awakeCounter -= Time.deltaTime;
            if(awakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

    public void Activate()
    {
        gameObject.SetActive(true);
        awakeCounter = awakeTime;
    }
}
