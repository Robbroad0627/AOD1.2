/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: InnUpstairsExit.cs
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

public class LoadingScene : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private float waitToLoad = 0.0f;

    #endregion

    //FUNCTIONS
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update ()
    {
		if (waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;

            if (waitToLoad <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
                GameManager.Access.LoadData();
                QuestManager.instance.LoadQuestData();
            }
        }
	}
#pragma warning restore IDE0051

    #endregion
}