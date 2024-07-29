using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games


public class BoatCaptian : MonoBehaviour
{
    public string nextAreaToLoad;
    public string nextAreaTransitionName;
    public string previousAreaToLoad;
    public string previousAreaTransitionName;
    public string nextContinent;
    public string previousContinent;
    public int goldCost = 1;

    public float waitToLoad = 1f;

    public PortController portController;

    public bool canOpen;
    public bool boatTripConfirmed;
    public bool boatDestinationConfirmedNext;
    public bool boatDestinationConfirmedPre;

    private bool shouldLoadAfterFade;
    private bool shouldRunAnimationBeforeFade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !boatTripConfirmed)
        {
            GameManager.instance.ModalPromptBoatTrip(goldCost);
        }

        if (boatDestinationConfirmedNext) 
        {
            portController.PlayerEnterBoat(nextAreaToLoad, nextAreaTransitionName);
        }
        else if (boatDestinationConfirmedPre)
        {
            portController.PlayerEnterBoat(previousAreaToLoad, previousAreaTransitionName);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = true;
        }
    }
}
