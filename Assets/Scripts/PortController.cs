
using UnityEngine;
using UnityEngine.SceneManagement;


public class PortController : MonoBehaviour
{
    public GameObject boat;
    public GameObject portAreaExit;
    public GameObject portAreaEntrance;
    public GameObject boatCaptain;
    public Transform dockedSpot;
    public Transform portEntrance;

    private string areaToLoad;
    private string areaTransitionName;

    public float waitToLoad = 1f;

    public Direction direction;

    public float disembarkTimer;

    private Boat boatController;
    private BoatCaptain boatCaptainController;
    private AreaExit areaExitController;
    private AreaEntrance areaEntranceController;

    private bool shouldLoadAfterFade;
    private bool shouldRunAnimationBeforeFade;

    public static bool boatIsDocked = false;
    public static bool boatIsLeaving = false;

    // Start is called before the first frame update
    void Start()
    {
        areaExitController = portAreaExit.GetComponent<AreaExit>();
        areaEntranceController = portAreaEntrance.GetComponent<AreaEntrance>();
        boatCaptainController = boatCaptain.GetComponent<BoatCaptain>();

        if (Boat.Access == null) 
        {
            Instantiate(boat).GetComponent<Boat>();
        }

        if (!Boat.boatLeftPort) 
        {
            boatController.gameObject.transform.SetParent(dockedSpot, false);
            boatController.PortDirection(direction);
        }
        else if (Boat.boatLeftPort)
        {
            boatController.gameObject.transform.SetParent(portEntrance, false);
            boatController.PortDirection(direction);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Boat.isPlayerOnBoat || GameManager.Access.GetHasBoat)
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
            if (Boat.boatLeftPort)
            {
                this.enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
                                    //SceneManager.LoadScene(areaToLoad);
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
        Boat.isPlayerOnBoat = true;
        Boat.isLeavingPort = true;
        shouldRunAnimationBeforeFade = true;
        areaToLoad = toLoad;
        areaTransitionName = transitionName;
        
    }

    public void PlayerExitBoat() 
    {
        boatCaptainController.boatDestinationConfirmedNext = false;
        boatCaptainController.boatDestinationConfirmedPre = false;
        Boat.isPlayerOnBoat = false;
        boatIsDocked = false;
    }
}
