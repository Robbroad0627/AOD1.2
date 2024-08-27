/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: GameMenu.cs
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
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    public static GameMenu instance;

    private void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion

    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string MENU_BUTTON = "Fire2";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject theMenu;
    [SerializeField] private GameObject[] windows;
    [SerializeField] private Text[] nameText;
    [SerializeField] private Text[] hpText;
    [SerializeField] private Text[] mpText;
    [SerializeField] private Text[] lvlText;
    [SerializeField] private Text[] expText;
    [SerializeField] private Slider[] expSlider;
    [SerializeField] private Image[] charImage;
    [SerializeField] private GameObject[] charStatHolder;
    [SerializeField] private GameObject[] statusButtons;
    [SerializeField] private Text statusName;
    [SerializeField] private Text statusHP;
    [SerializeField] private Text statusMP;
    [SerializeField] private Text statusStr;
    [SerializeField] private Text statusDef;
    [SerializeField] private Text statusWpnEqpd;
    [SerializeField] private Text statusWpnPwr;
    [SerializeField] private Text statusArmrEqpd;
    [SerializeField] private Text statusArmrPwr;
    [SerializeField] private Text statusExp;
    [SerializeField] private Image statusImage;
    [SerializeField] private ItemButton[] itemButtons;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDescription;
    [SerializeField] private Text useButtonText;
    [SerializeField] private GameObject itemCharChoiceMenu;
    [SerializeField] private Text[] itemCharChoiceNames;
    [SerializeField] private Text goldText;
    [SerializeField] private string mainMenuName;

    #endregion
    #region Private Variables

    private CharStats[] playerStats;
    private Item activeItem;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public GameObject GetTheMenu => theMenu;
    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Awake() => Singleton();
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update ()
    {
		if (Input.GetButtonDown(MENU_BUTTON))
        {
            if (theMenu.activeInHierarchy)
            {
                CloseMenu();
            }
            else
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
#pragma warning restore IDE0051

    #endregion
    #region Private Functions/Methods

    private void UpdateMainStats()
    {
        playerStats = GameManager.instance.GetCharacterStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
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
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.GetCurrentGold.ToString() + "g";
    }

    private void CloseMenu()
    {
        for (int i = 0 ; i < windows.Length ; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);

        GameManager.instance.SetGameMenuOpen(false);

        itemCharChoiceMenu.SetActive(false);
    }

    private void StatusChar(int selected)
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

    private void CloseItemCharChoice() => itemCharChoiceMenu.SetActive(false);

    #endregion
    #region Public Functions/Methods

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0 ; i < itemButtons.Length ; i++)
        {
            itemButtons[i].SetValue(i);

            if (GameManager.instance.GetItemsHeld[i] != "")
            {
                itemButtons[i].GetImage.gameObject.SetActive(true);
                itemButtons[i].GetImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.GetItemsHeld[i]).GetSprite;
                itemButtons[i].GetAmount.text = GameManager.instance.GetNumberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].GetImage.gameObject.SetActive(false);
                itemButtons[i].GetAmount.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if (activeItem.GetIsItem)
        {
            useButtonText.text = "Use";
        }

        if (activeItem.GetIsWeapon || activeItem.GetIsArmor)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.GetName;
        itemDescription.text = activeItem.GetDescription;
    }

    #endregion
    #region Buttons

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();

        //update the information that is shown
        StatusChar(0);

        for (int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].GetCharacterName;
        }
    }

    public void DiscardItem()
    {
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.GetName);
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

    public void PlayButtonSound() => AudioManager.instance.PlaySFX(4);

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }

    #endregion
}