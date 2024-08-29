/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: MainMenu.cs
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

public class MainMenu : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private GameObject continueButton = null;
    [SerializeField] private string loadGameScene = null;
    [SerializeField] private string instructions = null;
    [SerializeField] private string races = null;
    [SerializeField] private string classes = null;
    [SerializeField] private string dwarf = null;
    [SerializeField] private string elf = null;
    [SerializeField] private string halfElf = null;
    [SerializeField] private string halfOrc = null;
    [SerializeField] private string halfling = null;
    [SerializeField] private string human = null;
    [SerializeField] private string cleric = null;
    [SerializeField] private string druid = null;
    [SerializeField] private string fighter = null;
    [SerializeField] private string magicUser = null;
    [SerializeField] private string paladin = null;
    [SerializeField] private string ranger = null;
    [SerializeField] private string thief = null;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

    private void Start()
    {
        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    #endregion
    #region Public Functions/Methods

    public void Continue() => SceneManager.LoadScene(loadGameScene);

    public void NewGame() => SceneManager.LoadScene("CharCreation");

    public void Instructions() => SceneManager.LoadScene("Instructions");

    public void Exit() => Application.Quit();

    public void Races() => SceneManager.LoadScene("Races");

    public void Classes() => SceneManager.LoadScene("Classes");

    public void Dwarf() => SceneManager.LoadScene("Dwarf");

    public void Elf() => SceneManager.LoadScene("Elf");

    public void Human() => SceneManager.LoadScene("Human");

    public void Halfling() => SceneManager.LoadScene("Halfling");

    public void HalfOrc() => SceneManager.LoadScene("HalfOrc");

    public void HalfElf() => SceneManager.LoadScene("HalfElf");

    public void Cleric() => SceneManager.LoadScene("Cleric");

    public void Druid() => SceneManager.LoadScene("Druid");

    public void Fighter() => SceneManager.LoadScene("Fighter");

    public void MagicUser() => SceneManager.LoadScene("MagicUser");

    public void Paladin() => SceneManager.LoadScene("Paladin");

    public void Ranger() => SceneManager.LoadScene("Ranger");

    public void Thief() => SceneManager.LoadScene("Thief");

    #endregion
}