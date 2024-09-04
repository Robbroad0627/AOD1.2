/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: Boat.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: 
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: September 4, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

//ENUMERATORS
#region Public Enumerators for use Outside of this Class

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

#endregion

public class Boat : MonoBehaviour
{
    //SINGLETON
    #region Singleton - this Class has One and Only One Instance

    private static Boat mInstance;

    private void InitializeSingleton()
    {
        if (mInstance != null && mInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static Boat Access => mInstance;

    #endregion

    //VARIABLES
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private SpriteRenderer MyRenderer = null;
    [SerializeField] private Collider2D MyCollider = null;
    [SerializeField] private Animator MyAnimator = null;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private bool mHasLeftPort;
    private bool mIsLeavingPort;
    private bool mIsEnteringPort;
    private bool mIsPlayerOnBoat;
    private Direction mDirection;

    #endregion

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        mHasLeftPort = false;
        mIsLeavingPort = false;
        mIsEnteringPort = false;
        mIsPlayerOnBoat = false;
        mDirection = Direction.Left;
    }

    private void Update()
    {
        if (mIsPlayerOnBoat)
        {
            PlayerController pc = PlayerController.Access;

            if (pc != null) 
            {
                pc.transform.position = transform.position;
            }
        }

        if (mIsEnteringPort)
        {
            MyAnimator.SetBool("Enter", true);

            switch ((int)mDirection)
            {
                case 0: //opposite myDirection.Left
                    MyAnimator.SetBool("RightDir", true);
                    MyAnimator.SetBool("LeftDir", false);
                    MyAnimator.SetBool("UpDir", false);
                    MyAnimator.SetBool("DownDir", false);
                    break;

                case 1: //opposite myDirection.Right
                    MyAnimator.SetBool("LeftDir", true);
                    MyAnimator.SetBool("UpDir", false);
                    MyAnimator.SetBool("DownDir", false);
                    MyAnimator.SetBool("RightDir", false);
                    break;

                case 2: //opposite myDirection.Up
                    MyAnimator.SetBool("DownDir", true);
                    MyAnimator.SetBool("UpDir", false);
                    MyAnimator.SetBool("LeftDir", false);
                    MyAnimator.SetBool("RightDir", false);
                    break;

                case 3: //opposite myDirection.Down
                    MyAnimator.SetBool("UpDir", true);
                    MyAnimator.SetBool("LeftDir", false);
                    MyAnimator.SetBool("RightDir", false);
                    MyAnimator.SetBool("DownDir", false);
                    break;

                default:
                    Debug.Log("No Valid Direction");
                    break;
            }
        }

        if (mIsLeavingPort)
        {
            MyAnimator.SetBool("Leave", true);
        }

        if (!mIsEnteringPort)
        {
            MyAnimator.SetBool("Enter", false);

            switch ((int)mDirection)
            {
                case 0: //myDirection.Left
                    MyAnimator.SetBool("LeftDir", true);
                    MyAnimator.SetBool("RightDir", false);
                    MyAnimator.SetBool("UpDir", false);
                    MyAnimator.SetBool("DownDir", false);
                    break;

                case 1: //myDirection.Right
                    MyAnimator.SetBool("RightDir", true);
                    MyAnimator.SetBool("UpDir", false);
                    MyAnimator.SetBool("DownDir", false);
                    MyAnimator.SetBool("LeftDir", false);
                    break;

                case 2: //myDirection.Up
                    MyAnimator.SetBool("UpDir", true);
                    MyAnimator.SetBool("DownDir", false);
                    MyAnimator.SetBool("LeftDir", false);
                    MyAnimator.SetBool("RightDir", false);
                    break;

                case 3: //myDirection.Down
                    MyAnimator.SetBool("DownDir", true);
                    MyAnimator.SetBool("LeftDir", false);
                    MyAnimator.SetBool("RightDir", false);
                    MyAnimator.SetBool("UpDir", false);
                    break;

                default:
                    Debug.Log("No Valid Direction");
                    break;
            }
        }

        if (!mIsLeavingPort)
        {
            MyAnimator.SetBool("Leave", false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (mIsLeavingPort && collision.gameObject.name == "PortEntrance")
        {
            transform.position = collision.transform.position;
            mHasLeftPort = true;
            mIsLeavingPort = false;
            PortController.boatIsLeaving = true;
        }

        if (mIsEnteringPort && collision.gameObject.name == "DockedSpot")
        {
            transform.position = collision.transform.position;
            mIsEnteringPort = false;
            mHasLeftPort = false;
            PortController.boatIsDocked = true;
        }
    }

    public void PortDirection(Direction dir)
    {
        mDirection = dir;
    }
}