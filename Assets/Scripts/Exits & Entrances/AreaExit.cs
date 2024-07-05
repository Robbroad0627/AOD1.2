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

    private Boat boatController;
    private bool shouldLoadAfterFade;
    private bool shouldRunAnimationBeforeFade;

    public bool needBoat = false;

    private PlayerController player;

	void Start () {
        //if (needBoat)
        //{
        //    boatController = Instantiate(boat, boatSpawn.transform).GetComponent<Boat>();
        //    Boat.instance = boatController;            
        //}
    }
	
	void Update () {
		if(shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                boat.SetActive(false);
                SceneManager.LoadScene(areaToLoad);
            }
        }

        if (shouldRunAnimationBeforeFade)
        {
            if (boatController.boatLeftPort)
            {
                this.enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
                                    //SceneManager.LoadScene(areaToLoad);
                shouldLoadAfterFade = true;
                GameManager.instance.fadingBetweenAreas = true;

                UIFade.instance.FadeToBlack();

                player.areaTransitionName = areaTransitionName;
                shouldRunAnimationBeforeFade = false;
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

                //player = PlayerController.instance;
                //player.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //player.gameObject.GetComponent<Collider2D>().enabled = false;
                //player.canMove = false;
                //boatController.isPlayerOnBoat = true;
                //boatController.boatLeavingPort = true;
                //boatController.myAnim.enabled = true;
                //boatController.myCollider.enabled = true;
                //shouldRunAnimationBeforeFade = true;
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
