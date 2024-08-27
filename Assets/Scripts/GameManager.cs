/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: GameManager.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    public static GameManager instance;

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

    private const string PLAYER = "Player";
    private const string PLAYER_PREFERENCE_KEY = "!!!Special:Player";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private CharStats[] playerStats = null;
    [SerializeField] private bool gameMenuOpen = false;
    [SerializeField] private bool dialogActive = false;
    [SerializeField] private bool fadingBetweenAreas = false;
    [SerializeField] private bool shopActive = false;
    [SerializeField] private bool battleActive = false;
    [SerializeField] private string[] itemsHeld = null;
    [SerializeField] private int[] numberOfItems = null;
    [SerializeField] private Item[] referenceItems = null;
    [SerializeField] private int currentGold = 0;
    [SerializeField] private bool haveBoat = false;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public bool GetHasBoat => haveBoat;
    public int GetCurrentGold => currentGold;
    public string[] GetItemsHeld => itemsHeld;
    public bool GetBattleActive => battleActive;
    public int[] GetNumberOfItems => numberOfItems;
    public CharStats[] GetCharacterStats => playerStats;
    public string GetPlayerName => playerStats[0].GetCharacterName;

    #endregion
    #region Setters/Mutators

    public bool SetShopActive(bool yesNo) => shopActive = yesNo;
    public int SetCurrentGold(int amount) => currentGold = amount;
    public bool SetBattleActive(bool yesNo) => battleActive = yesNo;
    public bool SetDialogActive(bool yesNo) => dialogActive = yesNo;
    public bool SetGameMenuOpen(bool yesNo) => gameMenuOpen = yesNo;
    public bool SetFadingBetweenAreas(bool yesNo) => fadingBetweenAreas = yesNo;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

    private void Awake() => Singleton();

    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SortItems();
    }

    #endregion
    #region Implementation Functions/Methods

    private void Update()
    {
        if (GameObject.Find(PLAYER))
        {
            if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive || Boat.isPlayerOnBoat)
            {
                PlayerController.instance.SetCanMove(false);
            }
            else
            {
                PlayerController.instance.SetCanMove(true);
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

    #endregion
    #region Public Functions/Methods

    public Item GetItemDetails(string itemToGrab)
    {
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].GetName == itemToGrab)
            {
                return referenceItems[i];
            }
        }

        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;

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
                        itemAfterSpace = true;
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
        if (itemToAdd == null)
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
                if (referenceItems[i].GetName == itemToAdd)
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

    public void ModalPromptSaveGame() => DialogManager.instance.Prompt("Do you want to save the game now?", SaveData, null);

    public void ModalPromptBoatTrip(int goldCost)
    {
        DialogManager dialogManager = DialogManager.instance;

        if (currentGold >= goldCost)
        {
            dialogManager.Prompt($"Do you want to travel by boat? It will cost {goldCost}g.", PortChoice, null, "Yes", "No", "Boat Captian");
        }
    }

    public void ModalPromptInn(int goldCost)
    {
        DialogManager dialogManager = DialogManager.instance;

        if (currentGold >= goldCost)
        {
            dialogManager.Prompt($"Do you want to stay the night? It will cost {goldCost}g.", InnSequence, null);
        }
    }

    public void SaveData()
    {
        if (Inn.GetIsPlayerUpstairs)
        {
            if (Inn.GetDownstairsLocation != null)
            {
                StorePlayerScene(Inn.GetDownstairsSceneName);
                StorePlayerPosition((Vector3)Inn.GetDownstairsLocation);
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

        CharStats playerCharacter = playerStats[0];

        StoreCharacter(playerCharacter, PLAYER_PREFERENCE_KEY);
        SaveNonCustomCharacters();

        //store inventory data
        for (int i = 0 ; i < itemsHeld.Length ; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }

        //Store quests
        QuestManager questManager = QuestManager.instance;
        questManager.isReady = false;
        PlayerPrefs.SetString("QuestsAvailable", JsonUtility.ToJson(questManager.activeQuestNames));
        PlayerPrefs.SetString("QuestsComplete", JsonUtility.ToJson(questManager.completedQuestNames));
        questManager.isReady = true;
    }

    public void LoadData()
    {
        QuestManager questManager = QuestManager.instance;

        if (questManager == null)
        {
            Invoke("LoadData", 0.2f);
            return;
        }

        questManager.isReady = false;

        PlayerController playerCharacter = PlayerController.instance;

        if (playerCharacter == null)
        {
            Invoke("LoadData", 0.1f);
            return;
        }

        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        //Load the player
        LoadCharacterByName(playerStats[0], PLAYER_PREFERENCE_KEY);
        LoadAppearance(playerStats[0], PLAYER_PREFERENCE_KEY); //Only the player's appearance is custom for now

        LoadNonCustomCharacters(); // Load everyone else assuming hardcoded names

        //Load inventory
        for (int i = 0 ; i < itemsHeld.Length ; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }

        //Load quests
        //NOTE order is important
        questManager.questMarkerNames = JsonUtility.FromJson<string[]>(PlayerPrefs.GetString("QuestsAvailable"));
        questManager.completedQuestNames = JsonUtility.FromJson<string[]>(PlayerPrefs.GetString("QuestsComplete"));
        questManager.isReady = true;
    }

    #endregion
    #region Private Funtions/Methods

    private void FullRestoreParty()
    {
        foreach (CharStats character in playerStats)
        {
            character.SetCurrentHP(character.GetMaxHP);
            character.SetCurrentMP(character.GetMaxMP);
        }
    }
    
    private void PortChoice()
    {
        DialogManager dialogManager = DialogManager.instance;
        BoatCaptain captian = FindObjectOfType<BoatCaptain>();
        captian.boatTripConfirmed = true;
        var nextContinent = captian.nextContinent;
        var preContinent = captian.previousContinent;
        var nextAreatoLoad = captian.nextAreaToLoad;
        var preAreatoLoad = captian.previousAreaToLoad;
        dialogManager.PortPrompt($"Where would you like to go?", NextPortChoice, PreviousPortChoice, $"{nextContinent} - {nextAreatoLoad}", $"{preContinent} - {preAreatoLoad}", "Boat Captian");
    }

    private void NextPortChoice()
    {
        BoatCaptain captian = FindObjectOfType<BoatCaptain>();
        captian.boatTripConfirmed = false;
        captian.boatDestinationConfirmedNext = true;        
    }

    private void PreviousPortChoice()
    {
        BoatCaptain captian = FindObjectOfType<BoatCaptain>();
        captian.boatTripConfirmed = false;
        captian.boatDestinationConfirmedPre = true;
    }

    private void InnSequence()
    {
        UIFade.instance.FadeToBlack();
        fadingBetweenAreas = false;
        Invoke("InnPostFade", 1.5f);
    }

    private void InnPostFade()
    {
        FullRestoreParty();
        Inn.WarpUpstairs();
        UIFade.instance.FadeFromBlack();
        fadingBetweenAreas = false;
        ModalPromptSaveGame();
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

    private static void StoreCharacter(CharStats currentCharacterStats, string charKey=null)
    {
        string charName = charKey ?? currentCharacterStats.GetCharacterName;
        //NOTE this should be bool, maintaining for backward compatability
        if (currentCharacterStats.gameObject.activeInHierarchy)
        {
            PlayerPrefs.SetInt("Player_" + charName + "_active", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Player_" + charName + "_active", 0);
        }

        PlayerPrefs.SetString("Player_" + charName + "_CharName", currentCharacterStats.GetCharacterName);
        PlayerPrefs.SetInt("Player_" + charName + "_Level", currentCharacterStats.GetLevel);
        PlayerPrefs.SetInt("Player_" + charName + "_CurrentExp", currentCharacterStats.GetCurrentXP);
        PlayerPrefs.SetInt("Player_" + charName + "_CurrentHP", currentCharacterStats.GetCurrentHP);
        PlayerPrefs.SetInt("Player_" + charName + "_MaxHP", currentCharacterStats.GetMaxHP);
        PlayerPrefs.SetInt("Player_" + charName + "_CurrentMP", currentCharacterStats.GetCurrentMP);
        PlayerPrefs.SetInt("Player_" + charName + "_MaxMP", currentCharacterStats.GetMaxMP);
        PlayerPrefs.SetInt("Player_" + charName + "_Strength", currentCharacterStats.GetStrength);
        PlayerPrefs.SetInt("Player_" + charName + "_Defence", currentCharacterStats.GetDefence);
        PlayerPrefs.SetInt("Player_" + charName + "_WpnPwr", currentCharacterStats.GetWeaponPower);
        PlayerPrefs.SetInt("Player_" + charName + "_ArmrPwr", currentCharacterStats.GetArmorPower);
        StoreEquips(currentCharacterStats, charName);

        //Store appearence
        PlayerPrefs.SetString("Player_" + charName + "_Class", currentCharacterStats.GetClass.ToString());
        PlayerPrefs.SetString("Player_" + charName + "_Sex", currentCharacterStats.GetSex);
        PlayerPrefs.SetString("Player_" + charName + "_Race", currentCharacterStats.GetRace);
    }

    private static void StoreEquips(CharStats currentCharacterStats, string charKey)
    {
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedWpn", currentCharacterStats.GetEquippedWeapon);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmr", currentCharacterStats.GetEquippedArmor);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrBody", currentCharacterStats.GetEquippedBodyArmor);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrFeet", currentCharacterStats.GetEquippedFootArmor);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrHand", currentCharacterStats.GetEquippedHandArmor);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrLegs", currentCharacterStats.GetEquippedLegArmor);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrshield", currentCharacterStats.GetEquippedShield);
        PlayerPrefs.SetString("Player_" + charKey + "_EquippedArmrOther", currentCharacterStats.GetEquippedOtherArmor);
    }

    private static void StorePlayerScene(string sceneName) => PlayerPrefs.SetString("Current_Scene", sceneName );

    private static void StorePlayerPosition(Vector3 pos)
    {
        PlayerPrefs.SetFloat("Player_Position_x", pos.x);
        PlayerPrefs.SetFloat("Player_Position_y", pos.y);
        PlayerPrefs.SetFloat("Player_Position_z", pos.z);
    }

    private void LoadNonCustomCharacters()
    {
        for (int i = 1; i < playerStats.Length; i++)
        {
            CharStats currentCharacterStats = playerStats[i];
            string charName = currentCharacterStats.GetCharacterName;
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

        currentCharacterStats.SetCharacterName(PlayerPrefs.GetString("Player_" + charName + "_CharName") ?? currentCharacterStats.GetCharacterName);
        currentCharacterStats.SetLevel(PlayerPrefs.GetInt("Player_" + charName + "_Level"));
        currentCharacterStats.SetCurrentXP(PlayerPrefs.GetInt("Player_" + charName + "_CurrentExp"));
        currentCharacterStats.SetCurrentHP(PlayerPrefs.GetInt("Player_" + charName + "_CurrentHP"));
        currentCharacterStats.SetMaxHP(PlayerPrefs.GetInt("Player_" + charName + "_MaxHP"));
        currentCharacterStats.SetCurrentMP(PlayerPrefs.GetInt("Player_" + charName + "_CurrentMP"));
        currentCharacterStats.SetMaxMP(PlayerPrefs.GetInt("Player_" + charName + "_MaxMP"));
        currentCharacterStats.SetStrength(PlayerPrefs.GetInt("Player_" + charName + "_Strength"));
        currentCharacterStats.SetDefense(PlayerPrefs.GetInt("Player_" + charName + "_Defence"));
        currentCharacterStats.SetWeaponPower(PlayerPrefs.GetInt("Player_" + charName + "_WpnPwr"));
        currentCharacterStats.SetArmorPower(PlayerPrefs.GetInt("Player_" + charName + "_ArmrPwr"));
        LoadEquips(currentCharacterStats, charName);
    }

    private static void LoadAppearance(CharStats currentCharacterStats, string charName)
    {
        string myClass = PlayerPrefs.GetString("Player_" + charName + "_Class");
        string mySex = PlayerPrefs.GetString("Player_" + charName + "_Sex");
        string myRace = PlayerPrefs.GetString("Player_" + charName + "_Race");
        currentCharacterStats.SetClass(myClass);
        currentCharacterStats.SetSex(mySex);
        currentCharacterStats.SetRace(myRace);

        currentCharacterStats.SetBattleCharacter(Resources.Load<BattleChar>("Prefabs/Players/PlayerOptions/" + myClass + "/" + mySex + myRace));

        if (currentCharacterStats.GetBattleCharacter == null)
        {
            Debug.LogError($"Stored character info for {charName} did not match a known BattleChar, defaulting to human male");
            myClass = "Cleric";
            mySex = "M";
            myRace = "Human";
            currentCharacterStats.SetClass(myClass);
            currentCharacterStats.SetSex(mySex);
            currentCharacterStats.SetRace(myRace);
            currentCharacterStats.SetBattleCharacter(Resources.Load<BattleChar>("Prefabs/Players/PlayerOptions/" + myClass + "/" + mySex + myRace));
        }
    }

    private static void LoadEquips(CharStats currentCharacterStats, string charKey)
    {
        currentCharacterStats.SetEquippedWeapon(PlayerPrefs.GetString("Player_" + charKey + "_EquippedWpn"));
        currentCharacterStats.SetEquippedArmor(PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmr"));
        currentCharacterStats.SetEquippedBodyArmor(PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrBody"));
        currentCharacterStats.SetEquippedFootArmor(PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrFeet"));
        currentCharacterStats.SetEquippedHandArmor(PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrHand"));
        currentCharacterStats.SetEquippedLegArmor(PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrLegs"));
        currentCharacterStats.SetEquippedShield(PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrshield"));
        currentCharacterStats.SetEquippedOtherArmor(PlayerPrefs.GetString("Player_" + charKey + "_EquippedArmrOther"));
    }

    #endregion
}