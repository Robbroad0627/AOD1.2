
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games

public class PortController : MonoBehaviour
{
    public GameObject boat;
    public GameObject portAreaExit;
    public GameObject portAreaEntrance;
    public GameObject boatCaptain;
    public Transform dockedSpot;
    public Transform portEntrance;
    public float waitToLoad = 1f;
    public Boat.Direction direction;
    public float disembarkTimer;
    public static bool boatIsDocked = false;
    public static bool boatIsLeaving = false;

    private string areaToLoad;
    private string areaTransitionName;
    private Boat boatController;
    private BoatCaptain boatCaptainController;
    private AreaExit areaExitController;
    private AreaEntrance areaEntranceController;
    private bool shouldLoadAfterFade;
    private bool shouldRunAnimationBeforeFade;

    void Start()
    {
        areaExitController = portAreaExit.GetComponent<AreaExit>();
        areaEntranceController = portAreaEntrance.GetComponent<AreaEntrance>();
        boatCaptainController = boatCaptain.GetComponent<BoatCaptain>();

        if (Boat.Access == null) 
        {
            boatController = Instantiate(boat).GetComponent<Boat>();
            //Boat.Access = boatController;
        }

        if (!Boat.mHasLeftPort) 
        {
            boatController.gameObject.transform.SetParent(dockedSpot, false);
            boatController.PortDirection(direction);
        }
        else if (Boat.mHasLeftPort)
        {
            boatController.gameObject.transform.SetParent(portEntrance, false);
            boatController.PortDirection(direction);
        }
    }

    void Update()
    {
        if (Boat.mIsPlayerOnBoat || GameManager.Access.GetHasBoat)
        {
            boatCaptain.SetActive(false);
            portAreaExit.SetActive(true);
        }
        else
        {
            boatCaptain.SetActive(true);
            portAreaExit.SetActive(false);
        }

        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }

        if (shouldRunAnimationBeforeFade)
        {
            if (Boat.mHasLeftPort)
            {
                enabled = true;
                shouldLoadAfterFade = true;
                GameManager.Access.SetFadingBetweenAreas(true);
                UIFade.instance.FadeToBlack();
                PlayerController.Access.SetAreaTransitionName(areaTransitionName);
                shouldRunAnimationBeforeFade = false;
            }
        }

        if (boatIsDocked)
        {
            boatController.gameObject.transform.SetParent(dockedSpot, false);
            disembarkTimer -= Time.deltaTime;

            if (disembarkTimer <= 0)
            {
                PlayerExitBoat();
            }
        }

        if (boatIsLeaving)
        {
            boatController.gameObject.transform.SetParent(portEntrance, false);
            boatIsLeaving = false;
        }
    }

    public void PlayerEnterBoat(string toLoad, string transitionName)
    {
        Boat.mIsPlayerOnBoat = true;
        Boat.mIsLeavingPort = true;
        shouldRunAnimationBeforeFade = true;
        areaToLoad = toLoad;
        areaTransitionName = transitionName;
        
    }

    public void PlayerExitBoat() 
    {
        boatCaptainController.boatDestinationConfirmedNext = false;
        boatCaptainController.boatDestinationConfirmedPre = false;
        Boat.mIsPlayerOnBoat = false;
        boatIsDocked = false;
    }
}
