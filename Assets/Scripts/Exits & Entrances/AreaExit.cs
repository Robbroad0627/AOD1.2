using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games

public class AreaExit : MonoBehaviour {

    public string areaToLoad;
    public string areaTransitionName;
   
    public float waitToLoad = 1f;

    public GameObject boat;
    public GameObject boatSpawn;

    private bool shouldLoadAfterFade;
    private bool shouldRunAnimationBeforeFade;

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
            if (needBoat && !GameManager.instance.haveBoat)
            {
                //Cant use boat area without one.
                Debug.Log("Area needs boat but GameManager.haveBoat == false");
                return;
            }
            else if (needBoat && GameManager.instance.haveBoat)
            {
                Instantiate(boat, boatSpawn.transform);
                var player = PlayerController.instance;
                player.GetComponentInParent<Transform>().position = boatSpawn.transform.position;
                player.GetComponentInParent<Transform>().position = boat.transform.localPosition;
                player.GetComponentInParent<SpriteRenderer>().enabled = false;
                player.GetComponentInParent<Collider2D>().enabled = false;
                player.canMove = false;
            }
            else if (!needBoat)
            {
                this.enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
                                    //SceneManager.LoadScene(areaToLoad);
                shouldLoadAfterFade = true;
                GameManager.instance.fadingBetweenAreas = true;

                UIFade.instance.FadeToBlack();

                PlayerController.instance.areaTransitionName = areaTransitionName;
            }
        }
    }
}
