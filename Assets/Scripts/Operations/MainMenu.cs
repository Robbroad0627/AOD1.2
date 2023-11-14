using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games

public class MainMenu : MonoBehaviour
{

    public string newGameScene;

    public GameObject continueButton;

    public string loadGameScene;

    public string instructions;

    public string races;

    public string classes;

    public string dwarf;

    public string elf;

    public string halfElf;

    public string halfOrc;

    public string halfling;

    public string human;

    public string cleric;

    public string druid;

    public string fighter;

    public string magicUser;

    public string paladin;

    public string ranger;

    public string thief;

    // Use this for initialization
    void Start()
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



    public void Continue()
    {
        SceneManager.LoadScene(loadGameScene);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("CharCreation");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void Exit()
    {
        Application.Quit();
    }


    //Customization Choices

    public void Races()
    {
        SceneManager.LoadScene("Races");
    }

    public void Classes()
    {
        SceneManager.LoadScene("Classes");
    }

    //Races

    public void Dwarf()
    {
        SceneManager.LoadScene("Dwarf");
    }

    public void Elf()
    {
        SceneManager.LoadScene("Elf");
    }

    public void Human()
    {
        SceneManager.LoadScene("Human");
    }

    public void Halfling()
    {
        SceneManager.LoadScene("Halfling");
    }

    public void HalfOrc()
    {
        SceneManager.LoadScene("HalfOrc");
    }

    public void HalfElf()
    {
        SceneManager.LoadScene("HalfElf");
    }

    //Races

    public void Cleric()
    {
        SceneManager.LoadScene("Cleric");
    }

    public void Druid()
    {
        SceneManager.LoadScene("Druid");
    }

    public void Fighter()
    {
        SceneManager.LoadScene("Fighter");
    }

    public void MagicUser()
    {
        SceneManager.LoadScene("MagicUser");
    }

    public void Paladin()
    {
        SceneManager.LoadScene("Paladin");
    }

    public void Ranger()
    {
        SceneManager.LoadScene("Ranger");
    }

    public void Thief()
    {
        SceneManager.LoadScene("Thief");
    }
}
