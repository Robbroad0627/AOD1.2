﻿using System.Collections;
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

    public void Races()
    {
        SceneManager.LoadScene("Races");
    }

    public void Classes()
    {
        SceneManager.LoadScene("Classes");
    }

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
        SceneManager.LoadScene("Half Orc");
    }

    public void HalfElf()
    {
        SceneManager.LoadScene("Half Elf");
    }

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
        SceneManager.LoadScene("Magic User");
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
