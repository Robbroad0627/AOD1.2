/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: GameOver.cs
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

public class GameOver : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private string mainMenuScene = null;
    [SerializeField] private string loadGameScene = null;

    #endregion

    //FUNCTIONS
    #region Public Functions/Methods

    public void QuitToMain()
    {
        Destroy(GameManager.Access.gameObject);
        Destroy(PlayerController.Access.gameObject);
        Destroy(GameMenu.Access.gameObject);
        Destroy(AudioManager.Access.gameObject);
        Destroy(BattleManager.Access.gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLastSave()
    {
        Destroy(GameManager.Access.gameObject);
        Destroy(PlayerController.Access.gameObject);
        Destroy(GameMenu.Access.gameObject);

        SceneManager.LoadScene(loadGameScene);
    }

    #endregion
}