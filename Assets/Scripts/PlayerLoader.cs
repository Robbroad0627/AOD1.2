using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class PlayerLoader : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
		if(PlayerController.instance == null)
        {
            Instantiate(player);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
