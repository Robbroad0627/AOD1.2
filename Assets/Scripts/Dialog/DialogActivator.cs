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
    [SerializeField] private string questToMark = "";
    [SerializeField] private bool markComplete = false;

    #endregion
    #region Private Variables

    private bool mCanActivate;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Start () => mCanActivate = false;
#pragma warning restore IDE0051

    #endregion
    #region Physics Functions/Methods

#pragma warning disable IDE0051
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanActivate = true;
        }
    }
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            mCanActivate = false;
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update ()
    {
		if (mCanActivate && Input.GetButtonDown(INTERACT) && !DialogManager.instance.GetDialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
	}
#pragma warning restore IDE0051

    #endregion
}