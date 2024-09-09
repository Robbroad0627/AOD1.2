/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: GameMenu.cs
 * Date Created: November 11, 2020
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
using UnityEngine.Serialization;

public class GameMenu : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    private static GameMenu mInstance;

    private void Singleton()
    {
        if (mInstance != null && mInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static GameMenu Access => mInstance;

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
    [FormerlySerializedAs("minimap")]
    [SerializeField] private GameObject TheMinimap;
    [FormerlySerializedAs("questList")]
    [SerializeField] private GameObject TheQuestListDisplay;
    [FormerlySerializedAs("questTextList")]
    [SerializeField] private Text[] QuestTextList;
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

    private Item mActiveItem;
    private CharStats[] mPlayerStats;

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

#pragma warning disable IDE0051
    private void Start()
    {
        for (int i = 0 ; i < QuestTextList.Length ; i++)
        {
            QuestTextList[i].text = "";
        }

        for (int i = 0 ; i < QuestManager.instance.GetActiveQuestsNames().Length ; i++)
        {
            for (int j = 0; j < QuestTextList.Length; j++)
            {
                if (QuestTextList[j].text == "")
                {
                    QuestTextList[j].text = QuestManager.instance.GetActiveQuestsNames()[i];
                    break;
                }
            }
        }
    }
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
                GameManager.Access.SetGameMenuOpen(true);
            }

            AudioManager.Access.PlaySoundFX(5);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (TheQuestListDisplay.activeInHierarchy)
            {
                TheQuestListDisplay.SetActive(false);
            }
            else
            {
                TheQuestListDisplay.SetActive(true);
            }
        }

        if (GameManager.Access.GetBattleActive)
        {
            TheMinimap.SetActive(false);

            if (TheQuestListDisplay.activeInHierarchy)
            {
                TheQuestListDisplay.SetActive(false);
            }
        }
        else
        {
            TheMinimap.SetActive(true);
        }

        for (int i = 0 ; i < QuestManager.instance.GetActiveQuestsNames().Length ; i++)
        {
            for (int j = 0 ; j < QuestTextList.Length ; j++)
            {
                if (QuestTextList[j].text == "")
                {
                    QuestTextList[j].text = QuestManager.instance.GetActiveQuestsNames()[i];
                    break;
                }
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Private Functions/Methods

    private void UpdateMainStats()
    {
        mPlayerStats = GameManager.Access.GetCharacterStats;

        for (int i = 0; i < mPlayerStats.Length; i++)
        {
            if (mPlayerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);
                nameText[i].text = mPlayerStats[i].GetCharacterName;
                hpText[i].text = "HP: " + mPlayerStats[i].GetCurrentHP + "/" + mPlayerStats[i].GetMaxHP;
                mpText[i].text = "MP: " + mPlayerStats[i].GetCurrentMP + "/" + mPlayerStats[i].GetMaxMP;
                lvlText[i].text = "Lvl: " + mPlayerStats[i].GetLevel;
                expText[i].text = "" + mPlayerStats[i].GetCurrentXP + "/" + mPlayerStats[i].GetXPToNextLevel[mPlayerStats[i].GetLevel];
                expSlider[i].maxValue = mPlayerStats[i].GetXPToNextLevel[mPlayerStats[i].GetLevel];
                expSlider[i].value = mPlayerStats[i].GetCurrentXP;
                charImage[i].sprite = mPlayerStats[i].GetSprite;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.Access.GetCurrentGold.ToString() + "g";
    }

    private void CloseMenu()
    {
        for (int i = 0 ; i < windows.Length ; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);

        GameManager.Access.SetGameMenuOpen(false);

        itemCharChoiceMenu.SetActive(false);
    }

    private void StatusChar(int selected)
    {
        statusName.text = mPlayerStats[selected].GetCharacterName;
        statusHP.text = "" + mPlayerStats[selected].GetCurrentHP + "/" + mPlayerStats[selected].GetMaxHP;
        statusMP.text = "" + mPlayerStats[selected].GetCurrentMP + "/" + mPlayerStats[selected].GetMaxMP;
        statusStr.text = mPlayerStats[selected].GetStrength.ToString();
        statusDef.text = mPlayerStats[selected].GetDefence.ToString();

        if (mPlayerStats[selected].GetEquippedWeapon != "")
        {
            statusWpnEqpd.text = mPlayerStats[selected].GetEquippedWeapon;
        }

        statusWpnPwr.text = mPlayerStats[selected].GetWeaponPower.ToString();

        if (mPlayerStats[selected].GetEquippedArmor != "")
        {
            statusArmrEqpd.text = mPlayerStats[selected].GetEquippedArmor;
        }

        statusArmrPwr.text = mPlayerStats[selected].GetArmorPower.ToString();
        statusExp.text = (mPlayerStats[selected].GetXPToNextLevel[mPlayerStats[selected].GetLevel] - mPlayerStats[selected].GetCurrentXP).ToString();
        statusImage.sprite = mPlayerStats[selected].GetSprite;
    }

    private void CloseItemCharChoice() => itemCharChoiceMenu.SetActive(false);

    #endregion
    #region Public Functions/Methods

    public void ShowItems()
    {
        GameManager.Access.SortItems();

        for (int i = 0 ; i < itemButtons.Length ; i++)
        {
            itemButtons[i].SetValue(i);

            if (GameManager.Access.GetItemsHeld[i] != "")
            {
                itemButtons[i].GetImage.gameObject.SetActive(true);
                itemButtons[i].GetImage.sprite = GameManager.Access.GetItemDetails(GameManager.Access.GetItemsHeld[i]).GetSprite;
                itemButtons[i].GetAmount.text = GameManager.Access.GetNumberOfItems[i].ToString();
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
        mActiveItem = newItem;

        if (mActiveItem.GetIsItem)
        {
            useButtonText.text = "Use";
        }

        if (mActiveItem.GetIsWeapon || mActiveItem.GetIsArmor)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = mActiveItem.GetName;
        itemDescription.text = mActiveItem.GetDescription;
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
            statusButtons[i].SetActive(mPlayerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = mPlayerStats[i].GetCharacterName;
        }
    }

    public void DiscardItem()
    {
        if (mActiveItem != null)
        {
            GameManager.Access.RemoveItem(mActiveItem.GetName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.Access.GetCharacterStats[i].GetCharacterName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.Access.GetCharacterStats[i].gameObject.activeInHierarchy);
        }
    }

    public void UseItem(int selectChar)
    {
        mActiveItem.Use(selectChar);
        CloseItemCharChoice();
    }

    public void SaveGame()
    {
        GameManager.Access.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound() => AudioManager.Access.PlaySoundFX(4);

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.Access.gameObject);
        Destroy(PlayerController.Access.gameObject);
        Destroy(AudioManager.Access.gameObject);
        Destroy(gameObject);
    }

    #endregion
}