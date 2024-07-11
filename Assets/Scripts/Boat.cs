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
            PlayerController pc = PlayerController.instance;

            if (pc != null) 
            {
                pc.transform.position = transform.position;
            }
        }

        if (isEnteringPort)
        {
            myAnim.SetBool("Enter", true);
        }

        if (!isEnteringPort)
        {
            myAnim.SetBool("Enter", false);
        }

        if (isLeavingPort)
        {
            myAnim.SetBool("Leave", true);
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
            boatLeftPort = true;
            isLeavingPort = false;
        }

        if (isEnteringPort && collision.gameObject.name == "DockedSpot")
        {
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


