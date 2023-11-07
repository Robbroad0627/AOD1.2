using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class PlayerLoader : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Awake () {
		if(PlayerController.instance == null)
        {
            Instantiate(player);
			//if(!GameManager.instance.dataLoadedOnce)
   //         {
			//	GameManager.instance.LoadData();
   //         }
        }
	}

    private void Start()
    {
        
    }

    private void OnValidate()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
