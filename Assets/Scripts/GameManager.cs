using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games

public class GameManager : MonoBehaviour
{
    private const string kPlayercharacterPreferenceKey = "!!!Special:Player";
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
        //if(!dataLoadedOnce)
        //{
        //    LoadData();
        //    //QuestManager.instance.LoadQuestData();
        //}

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player"))
        {
            if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive || Boat.isPlayerOnBoat)
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

    public void ModalPromptBoatTrip(int goldCost)
    {
        var dm = DialogManager.instance;
        if(currentGold >= goldCost)
        {
            dm.Prompt($"Do you want to travel by boat? It will cost {goldCost}g.", PortChoice, null, "Yes", "No", "Boat Captian");
        }
        else
        {

        }
    }

    public void ModalPromptInn(int goldCost)
    {
        var dm = DialogManager.instance;
        if (currentGold >= goldCost)
        {
            dm.Prompt($"Do you want to stay the night? It will cost {goldCost}g.", InnSequence, null);
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
    
    private void PortChoice()
    {
        var dm = DialogManager.instance;
        var captian = BoatCaptain.FindObjectOfType<BoatCaptain>();
        captian.boatTripConfirmed = true;
        var nextContinent = captian.nextContinent;
        var preContinent = captian.previousContinent;
        var nextAreatoLoad = captian.nextAreaToLoad;
        var preAreatoLoad = captian.previousAreaToLoad;
        dm.PortPrompt($"Where would you like to go?", NextPortChoice, PreviousPortChoice, $"{nextContinent} - {nextAreatoLoad}", $"{preContinent} - {preAreatoLoad}", "Boat Captian");
    }

    private void NextPortChoice()
    {
        var captian = BoatCaptain.FindObjectOfType<BoatCaptain>();
        captian.boatTripConfirmed = false;
        captian.boatDestinationConfirmedNext = true;        
    }

    private void PreviousPortChoice()
    {
        var captian = BoatCaptain.FindObjectOfType<BoatCaptain>();
        captian.boatTripConfirmed = false;
        captian.boatDestinationConfirmedPre = true;
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

        var playerCharacter = playerStats[0];
        StoreCharacter(playerCharacter, kPlayercharacterPreferenceKey);
        SaveNonCustomCharacters();

        //store inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }

        //Store quests
        var qm = QuestManager.instance;
        qm.isReady = false;
        PlayerPrefs.SetString("QuestsAvailable",JsonUtility.ToJson(qm.activeQuestNames));
        PlayerPrefs.SetString("QuestsComplete",JsonUtility.ToJson(qm.completedQuestNames));
        qm.isReady = true;
    }

    private void SaveNonCustomCharacters()
    {
        //save character info
        for (int i = 1; i < playerStats.Length; i++)
        {
            CharStats currentCharacterStats = playerStats[i];

            StoreCharacter(currentCharacterStats);
        }
    }

    private static void StoreCharacter(CharStats currentCharacterStats,string charKey=null)
    {
        string charName = charKey ?? currentCharacterStats.charName;
        //NOTE this hsould be bool, maintaining for backward compatability
        if (currentCharacterStats.gameObject.activeInHierarchy)
        {
            PlayerPrefs.SetInt("Player_" + charName + "_active", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Player_" + charName + "_active", 0);
        }

        PlayerPrefs.SetString("Player_" + charName + "_CharName", currentCharacterStats.charName);
        PlayerPrefs.SetInt("Player_" + charName + "_Level", currentCharacterStats.playerLevel);
        PlayerPrefs.SetInt("Player_" + charName + "_CurrentExp", currentCharacterStats.currentEXP);
        PlayerPrefs.SetInt("Player_" + charName + "_CurrentHP", currentCharacterStats.currentHP);
        PlayerPrefs.SetInt("Player_" + charName + "_MaxHP", currentCharacterStats.maxHP);
        PlayerPrefs.SetInt("Player_" + charName + "_CurrentMP", currentCharacterStats.currentMP);
        PlayerPrefs.SetInt("Player_" + charName + "_MaxMP", currentCharacterStats.maxMP);
        PlayerPrefs.SetInt("Player_" + charName + "_Strength", currentCharacterStats.strength);
        PlayerPrefs.SetInt("Player_" + charName + "_Defence", currentCharacterStats.defence);
        PlayerPrefs.SetInt("Player_" + charName + "_WpnPwr", currentCharacterStats.wpnPwr);
        PlayerPrefs.SetInt("Player_" + charName + "_ArmrPwr", currentCharacterStats.armrPwr);
        StoreEquips(currentCharacterStats, charName);


        //Store appearence
        PlayerPrefs.SetString("Player_" + charName + "_Class", currentCharacterStats.classString);
        PlayerPrefs.SetString("Player_" + charName + "_Sex", currentCharacterStats.sexString);
        PlayerPrefs.SetString("Player_" + charName + "_Race", currentCharacterStats.raceString);
    }

    private static void StoreEquips(CharStats currentCharacterStats, string charKey)
    {
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedWpn", currentCharacterStats.equippedWpn);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmr", currentCharacterStats.equippedArmr);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrBody", currentCharacterStats.equippedBodyArmr);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrFeet", currentCharacterStats.equippedFeetArmr);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrHand", currentCharacterStats.equippedHandArmr);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrLegs", currentCharacterStats.equippedLegsArmr);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrshield", currentCharacterStats.equippedShieldArmr);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrOther", currentCharacterStats.equippedOtherArmr);

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
        var qm = QuestManager.instance;
        if(null == qm)
        {
            //HACK the quest manager wasn't created yet but important object
            //liftetime managemnt is too messy so just retry after a timer
            Invoke("LoadData", 0.2f);
            return;
        }
        qm.isReady = false;
        dataLoadedOnce = true;

        var pc = PlayerController.instance;
        if (null == pc)
        {
            //HACK the player controller wasn't created yet but important object
            //liftetime managemnt is too messy so just retry after a timer
            Invoke("LoadData", 0.1f);
            return;
        }

        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        //Debug.LogError("Character art loading not implemented");

        //Load the player
        LoadCharacterByName(playerStats[0], kPlayercharacterPreferenceKey);
        LoadAppearance(playerStats[0], kPlayercharacterPreferenceKey); //Onl;y the player's appearance is custom for now

        LoadNonCustomCharacters(); // Load everyone else assuming hardcoded names

        //Load inventory
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }

        //Load quests
        //NOTE order is important
        qm.questMarkerNames=JsonUtility.FromJson<string[]>(PlayerPrefs.GetString("QuestsAvailable"));
        qm.completedQuestNames= JsonUtility.FromJson<string[]>(PlayerPrefs.GetString("QuestsComplete"));
        qm.isReady = true;


    }

    private void LoadNonCustomCharacters()
    {
        for (int i = 1; i < playerStats.Length; i++)
        {
            CharStats currentCharacterStats = playerStats[i];
            string charName = currentCharacterStats.charName;
            LoadCharacterByName(currentCharacterStats, charName);
        }
    }

    private static void LoadCharacterByName(CharStats currentCharacterStats, string charName)
    {
        if (PlayerPrefs.GetInt("Player_" + charName + "_active") == 0)
        {
            currentCharacterStats.gameObject.SetActive(false);
        }
        else
        {
            currentCharacterStats.gameObject.SetActive(true);
        }

        currentCharacterStats.charName = PlayerPrefs.GetString("Player_" + charName + "_CharName") ?? currentCharacterStats.charName;
        currentCharacterStats.playerLevel = PlayerPrefs.GetInt("Player_" + charName + "_Level");
        currentCharacterStats.currentEXP = PlayerPrefs.GetInt("Player_" + charName + "_CurrentExp");
        currentCharacterStats.currentHP = PlayerPrefs.GetInt("Player_" + charName + "_CurrentHP");
        currentCharacterStats.maxHP = PlayerPrefs.GetInt("Player_" + charName + "_MaxHP");
        currentCharacterStats.currentMP = PlayerPrefs.GetInt("Player_" + charName + "_CurrentMP");
        currentCharacterStats.maxMP = PlayerPrefs.GetInt("Player_" + charName + "_MaxMP");
        currentCharacterStats.strength = PlayerPrefs.GetInt("Player_" + charName + "_Strength");
        currentCharacterStats.defence = PlayerPrefs.GetInt("Player_" + charName + "_Defence");
        currentCharacterStats.wpnPwr = PlayerPrefs.GetInt("Player_" + charName + "_WpnPwr");
        currentCharacterStats.armrPwr = PlayerPrefs.GetInt("Player_" + charName + "_ArmrPwr");
        LoadEquips(currentCharacterStats, charName);

        
    }

    private static void LoadAppearance(CharStats currentCharacterStats, string charName)
    {
        string myClass = PlayerPrefs.GetString("Player_" + charName + "_Class");
        string mySex = PlayerPrefs.GetString("Player_" + charName + "_Sex");
        string myRace = PlayerPrefs.GetString("Player_" + charName + "_Race");
        currentCharacterStats.classString = myClass;
        currentCharacterStats.sexString = mySex;
        currentCharacterStats.raceString = myRace;

        currentCharacterStats.battleChar = Resources.Load<BattleChar>("Prefabs/Players/PlayerOptions/" + myClass + "/" + mySex + myRace);

        if (currentCharacterStats.battleChar == null)
        {
            Debug.LogError($"Stored character info for {charName} did not match a known BattleChar, defaulting to human male");
            myClass = "Cleric";
            mySex = "M";
            myRace = "Human";
            currentCharacterStats.classString = myClass;
            currentCharacterStats.sexString = mySex;
            currentCharacterStats.raceString = myRace;
            currentCharacterStats.battleChar = Resources.Load<BattleChar>("Prefabs/Players/PlayerOptions/" + myClass + "/" + mySex + myRace);
        }
    }

    private static void LoadEquips(CharStats currentCharacterStats, string charKey)
    {
        currentCharacterStats.equippedWpn = PlayerPrefs.GetString("Player_" + charKey + "_EquippedWpn");
        currentCharacterStats.equippedArmr = PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmr");
        currentCharacterStats.equippedBodyArmr=PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrBody" );
        currentCharacterStats.equippedFeetArmr=PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrFeet" );
        currentCharacterStats.equippedHandArmr=PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrHand");
        currentCharacterStats.equippedLegsArmr=PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrLegs" );
        currentCharacterStats.equippedShieldArmr=PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrshield" );
        currentCharacterStats.equippedOtherArmr=PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrOther" );
    }
}

