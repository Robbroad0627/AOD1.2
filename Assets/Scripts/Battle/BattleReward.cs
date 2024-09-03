/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleReward.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Used in UI Canvas Prefab
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
    #region Singleton - this Class has One and Only One Instance

    private static BattleReward mInstance;

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

    public static BattleReward Access => mInstance;

    #endregion

    //VARIABLES
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private GameObject rewardScreen;
    [SerializeField] private Text xpText;
    [SerializeField] private Text itemText;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private int mXPEarned;
    private bool mCompletesQuest;
    private string[] mRewardItems;
    private string mQuestToComplete;

    #endregion

    //GETTERS/SETTERS
    #region Public Setters/Mutators for use Outside of this Class Only

    public bool SetCompletesQuest(bool yesNo) => mCompletesQuest = yesNo;
    public string SetQuestToComplete(string quest) => mQuestToComplete = quest;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Awake () => Singleton();
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void Start()
    {
        mXPEarned = 0;
        mRewardItems = null;
        mQuestToComplete = "";
        mCompletesQuest = false;
    }
#pragma warning restore IDE0051

    #endregion
    #region Private Implementation Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Update ()
    {
        // For testing only remove before game is published
		if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(54, new string[] { "Short sword", "Leather Armor" });
        }
	}
#pragma warning restore IDE0051

    #endregion
    #region Public Functions/Methods for use Outside of this Class

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

    #endregion
    #region Public Functions/Methods for use with Buttons

    public void CloseRewardScreen()
    {
        for (int i = 0; i < GameManager.Access.GetCharacterStats.Length; i++)
        {
            if(GameManager.Access.GetCharacterStats[i].gameObject.activeInHierarchy)
            {
                GameManager.Access.GetCharacterStats[i].AddExp(mXPEarned);
            }
        }

        for (int i = 0; i < mRewardItems.Length; i++)
        {
            GameManager.Access.AddItem(mRewardItems[i]);
        }

        rewardScreen.SetActive(false);
        GameManager.Access.SetBattleActive(false);

        if (mCompletesQuest)
        {
            QuestManager.instance.MarkQuestComplete(mQuestToComplete);
        }
    }

    #endregion
}