/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: Inn.cs
 * Date Created: 
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

public class Inn : MonoBehaviour
{
    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string PLAYER = "Player";
    private const string INTERACT = "Fire1";
    private const string UPSTAIRS_SCENE_NAME = "Inn Upper";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private bool canOpen;
    [SerializeField] private int goldCost = 1;
    [SerializeField] private AreaEntrance downstairsEntrance;
    [SerializeField] private static string s_downstairsTransitionName;
    [SerializeField] private static Vector3? s_downstairsTransitionPosition;
    [SerializeField] private static string s_downstairsSceneName;
    [SerializeField] private static int s_goldCost;
    [SerializeField] private static bool isUpstairs = false;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public static bool GetIsPlayerUpstairs => isUpstairs;
    public static string GetDownstairsSceneName => s_downstairsSceneName;
    public static Vector3? GetDownstairsLocation => s_downstairsTransitionPosition;
    public static string GetDownstairsTransitionName => s_downstairsTransitionName;

    #endregion
    #region Setters/Mutators

    public static bool SetIsPlayerUpstairs(bool yesNo) => isUpstairs = yesNo;

    #endregion

    //FUCNTIONS
    #region Initialization Functions/Methods

    private void Start ()
    {
		if (downstairsEntrance == null)
        {
            var areaEntrance = FindObjectOfType<AreaEntrance>();

            if (areaEntrance == null)
            {
                Debug.LogError("downstairsEntrance not set", this);
            }
            else
            {
                Debug.LogWarning($"downstairsEntrance not set using first available entrance {areaEntrance.GetTransitionName}", this);
            }

            s_downstairsTransitionName = areaEntrance.GetTransitionName;
        }
	}

    #endregion
    #region Physics Functions/Methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            canOpen = false;
        }
    }

    #endregion
    #region Implementation Functions/Methods

    private void Update ()
    {
        if (canOpen && Input.GetButtonDown(INTERACT) && PlayerController.instance.GetCanMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            s_downstairsTransitionName = downstairsEntrance?.GetTransitionName;
            s_downstairsTransitionPosition = downstairsEntrance?.transform.position;
            s_downstairsSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            s_goldCost = goldCost;
            GameManager.instance.ModalPromptInn(goldCost);
        }
	}

    #endregion
    #region Public Functions/Methods

    public static void WarpUpstairs()
    {
        GameManager.instance.SetCurrentGold(GameManager.instance.GetCurrentGold - s_goldCost);
        PlayerController.instance.SetAreaTransitionName(UPSTAIRS_SCENE_NAME);
        SceneManager.LoadScene(UPSTAIRS_SCENE_NAME);
        isUpstairs = true;
    }

    #endregion
}