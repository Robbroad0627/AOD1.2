/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: DialogActivator.cs
 * Date Created: 
 * Created By: Rob Broad
 * Modified By: Jeff Moreau
 * Date Last Modified: June 13, 2024
 * Description: Start the dialog of the npc
 * TODO: 
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    //VARIABLES
    #region Inspector Variable Declarations and Initializations

    [SerializeField] private string[] lines;
    [SerializeField] private bool isPerson = true;
    [SerializeField] private string questToMark = null;
    [SerializeField] private bool markComplete = false;

    #endregion
    #region Private Variable Declarations Only

    private bool mCanActivate;

    #endregion

    //FUNCTIONS
    #region Physics Methods/Functions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            mCanActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            mCanActivate = false;
        }
    }

    #endregion
    #region Implementation Methods/Functions

    void Update ()
    {
		if (mCanActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
	}

    #endregion
}