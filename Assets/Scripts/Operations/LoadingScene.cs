/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: LoadingScene.cs
 * Date Created: 
 * Created By: Rob Broad
 * Modified By: Jeff Moreau
 * Date Last Modified: June 13, 2024
 * Description: Script to Load all data
 * TODO: 
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    //VARIABLES
    #region Inspector Variable Declarations and Initializations

    [SerializeField] private float mLoadingWaitTime = 1.0f;

    #endregion

    //FUNCTIONS
    #region Implementation Methods/Functions

    void Update ()
    {
        mLoadingWaitTime -= Time.deltaTime;

        if(mLoadingWaitTime <= 0)
        {
            mLoadingWaitTime = 0;

            GameManager.instance.LoadData();
            QuestManager.instance.LoadQuestData();
            SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
        }
	}

    #endregion
}