/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleNotification.cs
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
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private float awakeTime = 2.0f;
    [SerializeField] private Text theText = null;

    #endregion
    #region Private Variables

    private float mAwakeCounter;

    #endregion

    //GETTERS/SETTERS
    #region Setters/Mutators

    public string SetNotificationText(string newText) => theText.text = newText;

    #endregion

    //FUNCTIONS
    #region Implementation Methods/Functions

    void Update ()
    {
		if(mAwakeCounter > 0)
        {
            mAwakeCounter -= Time.deltaTime;

            if(mAwakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

    #endregion
    #region Public Functions/Methods

    public void Activate()
    {
        gameObject.SetActive(true);
        mAwakeCounter = awakeTime;
    }

    #endregion
}