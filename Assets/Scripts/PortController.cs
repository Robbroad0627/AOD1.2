using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class PortController : MonoBehaviour
{
    public GameObject boat;
    public GameObject portAreaExit;
    public GameObject portAreaEntrance;
    public Transform dockedSpot;
    public Transform portEntrance;
    
    public Boat.Direction direction;

    public float disembarkTimer;

    private Boat boatController;
    private AreaExit areaExitController;
    private AreaEntrance areaEntranceController;

    public static bool boatIsDocked = false;

    // Start is called before the first frame update
    void Start()
    {
        areaExitController = portAreaExit.GetComponent<AreaExit>();
        areaEntranceController = portAreaEntrance.GetComponent<AreaEntrance>();

        if (Boat.instance == null) 
        {
            boatController = Instantiate(boat).GetComponent<Boat>();
            Boat.instance = boatController;
            Boat.instance.PortDirection(direction);
        }

        if (!Boat.boatLeftPort) 
        {
            boatController.gameObject.transform.SetParent(dockedSpot, false);
            boatController.gameObject.transform.position = dockedSpot.position;
        }
        else if (Boat.boatLeftPort)
        {
            boatController.gameObject.transform.SetParent(portEntrance, false);
            boatController.gameObject.transform.position = portEntrance.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boatIsDocked) 
        {
            boatController.gameObject.transform.SetParent(dockedSpot, false);
            boatController.gameObject.transform.position = dockedSpot.position;

            disembarkTimer -= Time.deltaTime;
            if (disembarkTimer <= 0) 
            {
                PlayerExitBoat();
            }
        }
    }

    public void PlayerEnterBoat()
    {
        Boat.isPlayerOnBoat = true;
        Boat.isLeavingPort = true;
    }

    public void PlayerExitBoat() 
    {
        Boat.isPlayerOnBoat = false;
        boatIsDocked = false;
    }
}
