/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: EssentialsLoader.cs
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

public class EssentialsLoader : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private GameObject UIScreen = null;
    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject gameMan = null;
    [SerializeField] private GameObject audioMan = null;
    [SerializeField] private GameObject battleMan = null;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

    void Start ()
    {
		if (UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        if (PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }

        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }

        if (BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        }
	}

    #endregion
}