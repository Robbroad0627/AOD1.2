/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: PlayerController.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 27, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    private static PlayerController mInstance;

    private void Singleton()
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

    public static PlayerController Access => mInstance;

    #endregion

    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string WORLD_MAP = "World Map";
    private const string LEFT_RIGHT = "Horizontal";
    private const string UP_DOWN = "Vertical";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private SpriteRenderer MySpriteRenderer = null;
    [SerializeField] private Rigidbody2D MyRigidbody = null;
    [SerializeField] private Animator MyAnimator = null;
    [SerializeField] private float MoveSpeed = 6.0f;
    [SerializeField] private GameObject TheBoat = null;

    #endregion
    #region Private Variables

    private bool canMove;
    private Scene mScene;
    private GameObject mWorldBoat;
    private Vector3 mTopRightLimit;
    private Vector3 mBottomLeftLimit;
    private string areaTransitionName;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public bool GetCanMove => canMove;
    public string GetAreaTransitionName => areaTransitionName;

    #endregion
    #region Setters/Mutators

    public bool SetCanMove(bool yesNo) => canMove = yesNo;
    public string SetAreaTransitionName(string newName) => areaTransitionName = newName;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Awake() => Singleton();
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update()
    {
        mScene = SceneManager.GetActiveScene();

        MyRigidbody.velocity = canMove ? new Vector2(Input.GetAxisRaw(LEFT_RIGHT), Input.GetAxisRaw(UP_DOWN)).normalized * MoveSpeed : Vector2.zero;

        MyAnimator.SetFloat("moveX", MyRigidbody.velocity.x);
        MyAnimator.SetFloat("moveY", MyRigidbody.velocity.y);

        if (Input.GetAxisRaw(LEFT_RIGHT) == 1 || Input.GetAxisRaw(LEFT_RIGHT) == -1 || Input.GetAxisRaw(UP_DOWN) == 1 || Input.GetAxisRaw(UP_DOWN) == -1)
        {
            if (canMove)
            {
                MyAnimator.SetFloat("lastMoveX", Input.GetAxisRaw(LEFT_RIGHT));
                MyAnimator.SetFloat("lastMoveY", Input.GetAxisRaw(UP_DOWN));
            }
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mBottomLeftLimit.x, mTopRightLimit.x), Mathf.Clamp(transform.position.y, mBottomLeftLimit.y, mTopRightLimit.y), transform.position.z);

        if (Boat.Access.GetIsPlayerOnboard)
        {
            MySpriteRenderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            MySpriteRenderer.enabled = true;
            GetComponent<Collider2D>().enabled = true;
            Vector3 temp = transform.position;
            transform.position = new Vector3(temp.x, temp.y, 0.0f);
        }

        if (mScene != null && mScene.name == WORLD_MAP && Boat.Access.GetIsPlayerOnboard) 
        {
            if (mWorldBoat == null)
            {
                mWorldBoat = Instantiate(TheBoat);
            }
            else
            {
                mWorldBoat.transform.SetParent(transform, false);
                mWorldBoat.transform.position = transform.position;
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Public Functions/Methods

    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        mBottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        mTopRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }

    #endregion
}