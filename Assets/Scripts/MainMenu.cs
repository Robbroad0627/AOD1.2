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
        SceneManager.LoadScene("Calimere");
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
