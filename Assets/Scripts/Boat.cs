using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class Boat : MonoBehaviour
{
    //public Rigidbody2D theRigidBody;
    //public float moveSpeed;

    public Animator myAnim;

    public static Boat instance;

    //public string areaTransitionName;
    //private Vector3 bottomLeftLimit;
    //private Vector3 topRightLimit;

    public bool isPlayerOnBoat = false;

    public bool boatLeftPort = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOnBoat)
        {
            PlayerController pc = PlayerController.instance;

            if (pc != null) 
            {
                pc.gameObject.transform.position = transform.position;
            }
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

    public void BoatHasLeftPort()
    {
        boatLeftPort = true;
        myAnim.Play("Base Layer.Boat_Port_Idle", 1);
    }
}


