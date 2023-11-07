using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games

public class AreaExit : MonoBehaviour {

    public string areaToLoad;

    public string areaTransitionName;

   
    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    public bool needBoat = false;


	void Start () {
        
    }
	
	
	void Update () {
		if(shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(needBoat && !GameManager.instance.haveBoat)
            {
                //Cant use boat area without one.
                Debug.Log("Area needs boat but GameManager.haveBoat == false");
                return;
            }
            this.enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
            //SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true;
            GameManager.instance.fadingBetweenAreas = true;

            UIFade.instance.FadeToBlack();

            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
