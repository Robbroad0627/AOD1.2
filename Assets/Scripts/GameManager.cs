using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public CharStats[] playerStats;

    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, battleActive;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    public int currentGold;

    //Used to load data if the game was loaded form a scene instad of the main menu
    //Should also be harmless if loading from main menu.
    public bool dataLoadedOnce =false;
    public bool haveBoat;

    //HACK: Assuming character zero is the player since this is not defined.
    public string playerName => playerStats[0].charName;

    // Use this for initialization
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
        if(!dataLoadedOnce)
        {
            LoadData();
            //QuestManager.instance.LoadQuestData();
        }

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player"))
        {
            if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive)
            {
                PlayerController.instance.canMove = false;
            }
            else
            {
                PlayerController.instance.canMove = true;
            }

#if UNITY_EDITOR
            //Cheats
            if (Input.GetKeyDown(KeyCode.J))
            {
                AddItem("Iron Armor");
                AddItem("Blabla");

                RemoveItem("Health Potion");
                RemoveItem("Bleep");
            }
#endif

            if (Input.GetKeyDown(KeyCode.O))
            {
                SaveData();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                LoadData();
            }
        }
    }


    public Item GetItemDetails(string itemToGrab)
    {

        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }




        return null;
    }

    public void SortItems()
    {
        bool itemAFterSpace = true;

        while (itemAFterSpace)
        {
            itemAFterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if (itemsHeld[i] != "")
                    {
                        itemAFterSpace = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Add one of the named item to the inventory.
    /// </summary>
    /// <param name="itemToAdd"></param>
    public void AddItem(string itemToAdd)
    {
        if(null == itemToAdd)
        {
            Debug.LogError("Couldn't add null item to player inventory.");
            return;
        }
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {
                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = referenceItems.Length;
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist!!");
            }
        }

        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        if (foundItem)
        {
            numberOfItems[itemPosition]--;

            if (numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }

            GameMenu.instance.ShowItems();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void ModalPromptSaveGame()
    {
        DialogManager.instance.Prompt("Do you want to save the game now?", SaveData, null);
    }

    public void ModalPromptInn(int goldCost)
    {
        var dm = DialogManager.instance;
        if (currentGold >= goldCost)
        {
            dm.Prompt($"Do you want to stay the night? It will cost {goldCost}", InnSequence, null);
        }
        else
        {

        }
    }

    public void FullRestoreParty()
    {
        foreach(var m in this.playerStats)
        {
            m.currentHP = m.maxHP;
            m.currentMP = m.maxMP;
        }
    }

    private void InnSequence()
    {
        UIFade.instance.FadeToBlack();
        //HACK: The screen fader is too dumb to do callbacks so just use a timer.
        GameManager.instance.fadingBetweenAreas = false;
        Invoke("InnPostFade", 1.5f);

    }

    private void InnPostFade()
    {
        FullRestoreParty();
        Inn.WarpUpstairs();
        
        UIFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
        ModalPromptSaveGame();
    }

    public void SaveData()
    {
        if (Inn.isUpstairs)
        {
            if (null != Inn.s_downstairsTransitionPosition)
            {
                StorePlayerScene(Inn.s_downstairsSceneName);
                StorePlayerPosition((Vector3)Inn.s_downstairsTransitionPosition);
            }
            else
            {
                Debug.LogError("SAVE ABORT! Saving while upstairs aborted could not safely restore downstairs scene.");
            }
        }
        else
        {
            StorePlayerScene(SceneManager.GetActiveScene().name);
            StorePlayerPosition(PlayerController.instance.transform.position);
        }

        //save character info
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);
        }

        //store inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    }

    private static void StorePlayerScene(string sceneName) => PlayerPrefs.SetString("Current_Scene", sceneName );
    private static void StorePlayerPosition(Vector3 pos)
    {
        PlayerPrefs.SetFloat("Player_Position_x", pos.x);
        PlayerPrefs.SetFloat("Player_Position_y", pos.y);
        PlayerPrefs.SetFloat("Player_Position_z", pos.z);
    }

    public void LoadData()
    {
        dataLoadedOnce = true;
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        Debug.LogError("Character art loading not implemented");
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");


            string myClass = "Cleric";
            string mySex = "F";
            string myRace = "Dwarf";
            playerStats[i].battleChar = Resources.Load<BattleChar>("Prefabs/Players/PlayerOptions/" + myClass + "/" + mySex + myRace);
        }

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }


    }
}

