/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: Boat.cs
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

public class Boat : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    } 

    public static Boat instance;

    public static bool isPlayerOnBoat = false;
    public static bool isEnteringPort = false;
    public static bool isLeavingPort = false;
    public static bool boatLeftPort = false;

    private Animator myAnim;
    private Collider2D myCollider;
    private SpriteRenderer myRenderer;
    private Direction myDirection;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isPlayerOnBoat)
        {
            PlayerController pc = PlayerController.Access;

            if (pc != null) 
            {
                pc.transform.position = transform.position;
            }
        }

        if (isEnteringPort)
        {
            myAnim.SetBool("Enter", true);

            switch ((int)myDirection)
            {
                case 0: //opposite myDirection.Left
                    myAnim.SetBool("RightDir", true);
                    myAnim.SetBool("LeftDir", false);
                    myAnim.SetBool("UpDir", false);
                    myAnim.SetBool("DownDir", false);
                    break;

                case 1: //opposite myDirection.Right
                    myAnim.SetBool("LeftDir", true);
                    myAnim.SetBool("UpDir", false);
                    myAnim.SetBool("DownDir", false);
                    myAnim.SetBool("RightDir", false);
                    break;

                case 2: //opposite myDirection.Up
                    myAnim.SetBool("DownDir", true);
                    myAnim.SetBool("UpDir", false);
                    myAnim.SetBool("LeftDir", false);
                    myAnim.SetBool("RightDir", false);
                    break;

                case 3: //opposite myDirection.Down
                    myAnim.SetBool("UpDir", true);
                    myAnim.SetBool("LeftDir", false);
                    myAnim.SetBool("RightDir", false);
                    myAnim.SetBool("DownDir", false);
                    break;

                default:
                    Debug.Log("No Valid Direction");
                    break;
            }
        }

        if (isLeavingPort)
        {
            myAnim.SetBool("Leave", true);
        }

        if (!isEnteringPort)
        {
            myAnim.SetBool("Enter", false);

            switch ((int)myDirection)
            {
                case 0: //myDirection.Left
                    myAnim.SetBool("LeftDir", true);
                    myAnim.SetBool("RightDir", false);
                    myAnim.SetBool("UpDir", false);
                    myAnim.SetBool("DownDir", false);
                    break;

                case 1: //myDirection.Right
                    myAnim.SetBool("RightDir", true);
                    myAnim.SetBool("UpDir", false);
                    myAnim.SetBool("DownDir", false);
                    myAnim.SetBool("LeftDir", false);
                    break;

                case 2: //myDirection.Up
                    myAnim.SetBool("UpDir", true);
                    myAnim.SetBool("DownDir", false);
                    myAnim.SetBool("LeftDir", false);
                    myAnim.SetBool("RightDir", false);
                    break;

                case 3: //myDirection.Down
                    myAnim.SetBool("DownDir", true);
                    myAnim.SetBool("LeftDir", false);
                    myAnim.SetBool("RightDir", false);
                    myAnim.SetBool("UpDir", false);
                    break;

                default:
                    Debug.Log("No Valid Direction");
                    break;
            }
        }

        if (!isLeavingPort)
        {
            myAnim.SetBool("Leave", false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLeavingPort && collision.gameObject.name == "PortEntrance")
        {
            transform.position = collision.transform.position;
            boatLeftPort = true;
            isLeavingPort = false;
            PortController.boatIsLeaving = true;
        }

        if (isEnteringPort && collision.gameObject.name == "DockedSpot")
        {
            transform.position = collision.transform.position;
            isEnteringPort = false;
            boatLeftPort = false;
            PortController.boatIsDocked = true;
        }
    }

    public void PortDirection(Direction dir)
    {
        myDirection = dir;
    }
}