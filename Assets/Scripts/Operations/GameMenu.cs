using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Bonehead Games

public class GameMenu : MonoBehaviour {
    public GameObject minimap;
    public GameObject theMenu;
    public GameObject[] windows;

    private CharStats[] playerStats;

    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;

    public GameObject[] statusButtons;

    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;
    public Image statusImage;

    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDescription, useButtonText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public static GameMenu instance;
    public Text goldText;

    public string mainMenuName;

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;

                CloseMenu();
            } else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.SetGameMenuOpen(true);
            }

            AudioManager.instance.PlaySFX(5);
        }

        if (GameManager.instance.GetBattleActive)
        {
            minimap.SetActive(false);
        }
        else
        {
            minimap.SetActive(true);
        }
	}

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.GetCharacterStats;

        for(int i = 0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                nameText[i].text = playerStats[i].GetCharacterName;
                hpText[i].text = "HP: " + playerStats[i].GetCurrentHP + "/" + playerStats[i].GetMaxHP;
                mpText[i].text = "MP: " + playerStats[i].GetCurrentMP + "/" + playerStats[i].GetMaxMP;
                lvlText[i].text = "Lvl: " + playerStats[i].GetLevel;
                expText[i].text = "" + playerStats[i].GetCurrentXP + "/" + playerStats[i].GetXPToNextLevel[playerStats[i].GetLevel];
                expSlider[i].maxValue = playerStats[i].GetXPToNextLevel[playerStats[i].GetLevel];
                expSlider[i].value = playerStats[i].GetCurrentXP;
                charImage[i].sprite = playerStats[i].GetSprite;
            } else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.GetCurrentGold.ToString() + "g";
    }

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for(int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            } else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for(int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);

        GameManager.instance.SetGameMenuOpen(false);

        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();

        //update the information that is shown
        StatusChar(0);

        for(int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].GetCharacterName;
        }
    }

    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].GetCharacterName;
        statusHP.text = "" + playerStats[selected].GetCurrentHP + "/" + playerStats[selected].GetMaxHP;
        statusMP.text = "" + playerStats[selected].GetCurrentMP + "/" + playerStats[selected].GetMaxMP;
        statusStr.text = playerStats[selected].GetStrength.ToString();
        statusDef.text = playerStats[selected].GetDefence.ToString();

        if (playerStats[selected].GetEquippedWeapon != "")
        {
            statusWpnEqpd.text = playerStats[selected].GetEquippedWeapon;
        }

        statusWpnPwr.text = playerStats[selected].GetWeaponPower.ToString();

        if (playerStats[selected].GetEquippedArmor != "")
        {
            statusArmrEqpd.text = playerStats[selected].GetEquippedArmor;
        }

        statusArmrPwr.text = playerStats[selected].GetArmorPower.ToString();
        statusExp.text = (playerStats[selected].GetXPToNextLevel[playerStats[selected].GetLevel] - playerStats[selected].GetCurrentXP).ToString();
        statusImage.sprite = playerStats[selected].GetSprite;
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.GetItemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.GetItemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.GetNumberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if(activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if(activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.GetCharacterStats[i].GetCharacterName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.GetCharacterStats[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}