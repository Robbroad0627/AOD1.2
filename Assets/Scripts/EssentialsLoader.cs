/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: EssentialsLoader.cs
 * Date Created: 
 * Created By: Rob Broad
 * Modified By: Jeff Moreau
 * Date Last Modified: June 13, 2024
 * Description: Script to Load all esential scripts
 * TODO: 
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    //VARIABLES
    #region Inspector Variable Declarations and Initializations

    [SerializeField] private GameObject mPlayer = null;
    [SerializeField] private GameObject mUIManager = null;
    [SerializeField] private GameObject mGameManager = null;
    [SerializeField] private GameObject mAudioManager = null;
    [SerializeField] private GameObject mBattleManager = null;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

    void Start ()
    {
		if (UIFade.instance == null)
        {
            UIFade.instance = Instantiate(mUIManager).GetComponent<UIFade>();
        }

        if (PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(mPlayer).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(mGameManager).GetComponent<GameManager>();
        }

        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(mAudioManager).GetComponent<AudioManager>();
        }

        if (BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(mBattleManager).GetComponent<BattleManager>();
        }
	}

    #endregion
}