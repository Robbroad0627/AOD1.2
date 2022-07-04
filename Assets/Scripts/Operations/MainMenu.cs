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
}
