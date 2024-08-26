/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleReward.cs
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
using UnityEngine.UI;

public class BattleReward : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    public static BattleReward instance;

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
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private GameObject rewardScreen;
    [SerializeField] private Text xpText;
    [SerializeField] private Text itemText;

    #endregion
    #region Private Variables

    private int mXPEarned;
    private bool mCompletesQuest;
    private string[] mRewardItems;
    private string mQuestToComplete;

    #endregion

    //GETTERS/SETTERS
    #region Setters/Mutators

    public bool SetCompletesQuest(bool yesNo) => mCompletesQuest = yesNo;
    public string SetQuestToComplete(string quest) => mQuestToComplete = quest;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

    private void Awake ()
    {
        Singleton();
    }

    private void Start()
    {
        mXPEarned = 0;
        mRewardItems = null;
        mQuestToComplete = "";
        mCompletesQuest = false;
    }

    #endregion
    #region Implementation Functions/Methods

    private void Update ()
    {
        // For testing only remove before game is published
		if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(54, new string[] { "Short sword", "Leather Armor" });
        }
	}

    #endregion
    #region Public Functions/Methods

    public void OpenRewardScreen(int earnedXP, string[] rewardedItems)
    {
        mXPEarned = earnedXP;
        mRewardItems = rewardedItems;

        xpText.text = "Everyone earned " + mXPEarned + " xp!";
        itemText.text = "";

        for (int i = 0; i < mRewardItems.Length; i++)
        {
            itemText.text += rewardedItems[i] + "\n";
        }

        rewardScreen.SetActive(true);
    }

    public void CloseRewardScreen()
    {
        for (int i = 0; i < GameManager.instance.GetCharacterStats.Length; i++)
        {
            if(GameManager.instance.GetCharacterStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.GetCharacterStats[i].AddExp(mXPEarned);
            }
        }

        for (int i = 0; i < mRewardItems.Length; i++)
        {
            GameManager.instance.AddItem(mRewardItems[i]);
        }

        rewardScreen.SetActive(false);
        GameManager.instance.SetBattleActive(false);

        if (mCompletesQuest)
        {
            QuestManager.instance.MarkQuestComplete(mQuestToComplete);
        }
    }

    #endregion
}