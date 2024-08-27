using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class Boat : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    } 

    //public Rigidbody2D theRigidBody;
    //public float moveSpeed;

    private Animator myAnim;
    private Collider2D myCollider;
    private SpriteRenderer myRenderer;

    private Direction myDirection;

    public static Boat instance;

    //public string areaTransitionName;
    //private Vector3 bottomLeftLimit;
    //private Vector3 topRightLimit;

    public static bool isPlayerOnBoat = false;
    public static bool isEnteringPort = false;
    public static bool isLeavingPort = false;
    public static bool boatLeftPort = false;

    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOnBoat)
        {
            PlayerController pc = PlayerController.Access;

            if (pc != null) 
            {
                //pc.GetComponent<SpriteRenderer>().enabled = false;
                //pc.GetComponent<Collider2D>().enabled = false;
                pc.transform.position = transform.position;
            }
        }
        //else
        //{
        //    PlayerController pc = PlayerController.instance;
        //    pc.GetComponent<SpriteRenderer>().enabled = true;
        //    pc.GetComponent<Collider2D>().enabled = true;
        //}

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

        //if (canMove)
        //{
        //    theRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        //}
        //else
        //{
        //    theRigidBody.velocity = Vector2.zero;
        //}


        //myAnim.SetFloat("moveX", theRigidBody.velocity.x);
        //myAnim.SetFloat("moveY", theRigidBody.velocity.y);

        //if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        //{
        //    if (canMove)
        //    {
        //        myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
        //        myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        //    }
        //}

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

    //public void SetBounds(Vector3 botLeft, Vector3 topRight)
    //{
    //    bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
    //    topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    //}

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

        //switch ((int)dir)
        //{
        //    case 0:
        //        myDirection = Direction.Left;
        //        break;

        //    case 1:
        //        myDirection = Direction.Right;
        //        break;

        //    case 2:
        //        myDirection = Direction.Up;
        //        break;

        //    case 3:
        //        myDirection = Direction.Down;
        //        break;

        //    default:
        //        Debug.Log("Invalid direction selection");
        //        break;

        //}
    }
}


