/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleStarter.cs
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
using System.Collections;

public class BattleStarter : MonoBehaviour
{
    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string PLAYER = "Player";
    private const string LEFT_RIGHT = "Horizontal";
    private const string UP_DOWN = "Vertical";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Sprite backgroundSprite = null;
    [SerializeField] private BattleType[] potentialBattles = null;
    [SerializeField] private float timeBetweenBattles = 10.0f;
    [SerializeField] private bool activateOnEnter = false;
    [SerializeField] private bool activateOnExit = false;
    [SerializeField] private bool cannotFlee = false;
    [SerializeField] private bool deactivateAfterStarting = false;
    [SerializeField] private bool shouldCompleteQuest = false;
    [SerializeField] private string QuestToComplete = "";

    #endregion
    #region Private Variables

    private bool mIsPlayerInArea;
    private float mBattleCountdownTimer;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

#pragma warning disable IDE0051
    private void Start ()
    {
        mIsPlayerInArea = false;
        mBattleCountdownTimer = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
	}
#pragma warning restore IDE0051

    #endregion
    #region Physics Functions/Methods

#pragma warning disable IDE0051
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            if (activateOnEnter)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                mIsPlayerInArea = true;
            }
        }
    }
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))
        {
            if (activateOnExit)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                mIsPlayerInArea = false;
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update ()
    {
		if (mIsPlayerInArea && PlayerController.instance.GetCanMove)
        {
            if (Input.GetAxisRaw(LEFT_RIGHT) != 0 || Input.GetAxisRaw(UP_DOWN) != 0)
            {
                mBattleCountdownTimer -= Time.deltaTime;
            }

            if (mBattleCountdownTimer <= 0)
            {
                mBattleCountdownTimer = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);

                StartCoroutine(StartBattleCo());
            }
        }
	}
#pragma warning restore IDE0051

    #endregion
    #region Coroutines

    public IEnumerator StartBattleCo()
    {
        UIFade.instance.FadeToBlack();

        if (backgroundSprite != null)
        {
            BattleManager.instance.SetBackgroundSprite(backgroundSprite);
        }

        GameManager.instance.SetBattleActive(true);
        
        int selectedBattle = Random.Range(0, potentialBattles.Length);
        BattleManager.instance.SetRewardItems(potentialBattles[selectedBattle].rewardItems);
        BattleManager.instance.SetRewardXP(potentialBattles[selectedBattle].rewardXP);

        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee);
        
        UIFade.instance.FadeFromBlack();

        if (deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }

        BattleReward.instance.SetCompletesQuest(shouldCompleteQuest);
        BattleReward.instance.SetQuestToComplete(QuestToComplete);
    }

    #endregion
}