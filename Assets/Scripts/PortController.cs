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

    private Boat boatController;
    private AreaExit areaExitController;
    private AreaEntrance areaEntranceController;

    private bool boatEnteringPort = false;
    private bool boatDocked = false;

    // Start is called before the first frame update
    void Start()
    {
        areaExitController = portAreaExit.GetComponent<AreaExit>();
        areaEntranceController = portAreaEntrance.GetComponent<AreaEntrance>();

        if (areaExitController != null) 
        {
            if (areaExitController.needBoat && GameManager.instance.haveBoat)
            {
                boatController = Instantiate(boat, dockedSpot).GetComponent<Boat>();
                Boat.instance = boatController;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AreaEnter()
    {
        boatDocked = false;
        boatEnteringPort = true;
    }

    public void Docked()
    {
        boatEnteringPort = false;
        boatEnteringPort = true;
    }
}
