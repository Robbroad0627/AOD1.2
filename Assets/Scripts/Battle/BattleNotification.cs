/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleNotification.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in BattleManager Prefab BattleScene/Canvas/BattleNotification adds information to the battle screen
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 29, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour
{
    //VARIABLES
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [FormerlySerializedAs("awakeTime")]
    [SerializeField] private float OnScreenDuration = 2.0f;
    [FormerlySerializedAs("theText")]
    [SerializeField] private Text NotificationText = null;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private float mAwakeCounter;

    #endregion

    //GETTERS/SETTERS
    #region Public Setters/Mutators for use Outside of this Class Only

    public string SetNotificationText(string newText) => NotificationText.text = newText;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Start() => InitializeVariables();
#pragma warning restore IDE0051

    private void InitializeVariables() => mAwakeCounter = 0;

    #endregion
    #region Private Implementation Functions/Methods used in this Class Only

#pragma warning disable IDE0051
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
#pragma warning restore IDE0051

    #endregion
    #region Public Functions/Methods for use Outside of this Class

    public void Activate()
    {
        gameObject.SetActive(true);
        mAwakeCounter = OnScreenDuration;
    }

    #endregion
}