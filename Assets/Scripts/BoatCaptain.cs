/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BoatCaptain.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: September 9, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class BoatCaptain : MonoBehaviour
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

    void Update()
    {
        if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.Access.GetCanMove && !boatTripConfirmed)
        {
            GameManager.Access.ModalPromptBoatTrip(goldCost);
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