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

    //Do NOT rename these Variables unless you know what you are changing
    [SerializeField] private GameObject ePlayer = null;
    [SerializeField] private GameObject eUIManager = null;
    [SerializeField] private GameObject eGameManager = null;
    [SerializeField] private GameObject eAudioManager = null;
    [SerializeField] private GameObject eBattleManager = null;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

    void Start ()
    {
		if (UIFade.instance == null)
        {
            UIFade.instance = Instantiate(eUIManager).GetComponent<UIFade>();
        }

        if (PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(ePlayer).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(eGameManager).GetComponent<GameManager>();
        }

        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(eAudioManager).GetComponent<AudioManager>();
        }

        if (BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(eBattleManager).GetComponent<BattleManager>();
        }
	}

    #endregion
}