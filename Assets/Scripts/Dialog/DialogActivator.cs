/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: DialogActivator.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string PLAYER = "Player";
    private const string INTERACT = "Fire1";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private string[] lines = null;
    [SerializeField] private bool isPerson = true;
    [SerializeField] private bool shouldActivateQuest = false;
    [SerializeField] private string questToMark = "";
    [SerializeField] private bool markComplete = false;

    #endregion
    #region Private Variables

    private bool mCanActivate;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

    private void Start ()
    {
        mCanActivate = false;
	}

    #endregion
    #region Physics Functions/Methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanActivate = false;
        }
    }

    #endregion
    #region Implementation Functions/Methods

    private void Update ()
    {
		if (mCanActivate && Input.GetButtonDown(INTERACT) && !DialogManager.instance.GetDialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
	}

    #endregion
}