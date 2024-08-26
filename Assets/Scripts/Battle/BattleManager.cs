/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleManager.cs
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    public static BattleManager instance;

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
    [Space(10)]
    [Header("SCENE INFO >======================---")]
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer = null;
    [SerializeField] private GameObject battleScene = null;
    [SerializeField] private GameObject uiButtonsHolder = null;
    [Space(10)]
    [Header("TARGETING MENU >======================---")]
    [SerializeField] private GameObject targetMenu = null;
    [SerializeField] private BattleTargetButton[] targetButtons = null;
    [Space(10)]
    [Header("MAGIC MENU >======================---")]
    [SerializeField] private GameObject magicMenu = null;
    [SerializeField] private BattleMagicSelect[] magicButtons = null;
    [Space(10)]
    [Header("PLAYER INFO >======================---")]
    [SerializeField] private Text[] playerName = null;
    [SerializeField] private Text[] playerHP = null;
    [SerializeField] private Text[] playerMP = null;
    [SerializeField] private BattleChar[] playerPrefabs = null;
    [SerializeField] private Transform[] playerPositions = null;
    [Space(10)]
    [Header("ENEMY INFO >======================---")]
    [SerializeField] private BattleChar[] enemyPrefabs = null;
    [SerializeField] private Transform[] enemyPositions = null;
    [SerializeField] private GameObject enemyAttackEffect = null;
    [Space(10)]
    [Header("BATTLE DATA >======================---")]
    [SerializeField] private BattleNotification battleNotice = null;
    [SerializeField] private int chanceToFlee = 35;
    [SerializeField] private string gameOverScene = "";
    [SerializeField] private BattleMove[] movesList = null;
    [SerializeField] private DamageNumber theDamageNumber = null;

    #endregion
    #region Private Variable Declarations Only

    private bool mCanFlee;
    private bool mIsFleeing;
    private bool mTurnWaiting;
    private bool mIsBattleActive;
    private bool mIsBattlerReady;
    private int mRewardXP;
    private int mCurrentTurn;
    private int mOutsideBattleBGM;
    private string[] mRewardItems;
    private List<BattleChar> mActiveBattlers;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetCurrentTurn => mCurrentTurn;
    public GameObject GetMagicMenu => magicMenu;
    public BattleNotification GetBattleNotice => battleNotice;
    public List<BattleChar> GetActiveBattlers => mActiveBattlers;

    #endregion
    #region Setters/Mutators

    public int SetRewardXP(int amount) => mRewardXP = amount;
    public string[] SetRewardItems(string[] newItems) => mRewardItems = newItems;
    public Sprite SetBackgroundSprite(Sprite newSprite) => backgroundSpriteRenderer.sprite = newSprite;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

    private void Awake()
    {
        Singleton();
    }

    private void Start ()
    {
        mRewardXP = 0;
        mCurrentTurn = 0;
        mCanFlee = false;
        mRewardItems = null;
        mTurnWaiting = false;
        mIsBattlerReady = false;
        mActiveBattlers = new List<BattleChar>();
    }

    #endregion
    #region Implementation Private Methods/Functions

    private void Update ()
    {
		if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Eyeball"}, false);
        }

        if (mIsBattleActive)
        {
            if (mTurnWaiting)
            {
                if (mActiveBattlers[mCurrentTurn].GetIsPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    //enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
	}

    private void StartBattleMusic()
    {
        mOutsideBattleBGM = AudioManager.instance.GetCurrentBackgroundMusic;

        AudioManager.instance.PlayBGM(0);
    }

    private void NextTurn()
    {
        if (mActiveBattlers[mCurrentTurn].GetIsPlayer)
        {
            Vector3 playerpos = new Vector3(mActiveBattlers[mCurrentTurn].gameObject.transform.position.x + 1.5f,
                                            mActiveBattlers[mCurrentTurn].gameObject.transform.position.y,
                                            mActiveBattlers[mCurrentTurn].gameObject.transform.position.z);

            mActiveBattlers[mCurrentTurn].transform.position = playerpos;
            mActiveBattlers[mCurrentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            Vector3 newpos = new Vector3(mActiveBattlers[mCurrentTurn].gameObject.transform.position.x - 1.5f,
                                         mActiveBattlers[mCurrentTurn].gameObject.transform.position.y,
                                         mActiveBattlers[mCurrentTurn].gameObject.transform.position.z);

            mActiveBattlers[mCurrentTurn].transform.position = newpos;
            mActiveBattlers[mCurrentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        mCurrentTurn++;

        if (mCurrentTurn >= mActiveBattlers.Count)
        {
            mCurrentTurn = 0;
        }

        if (mActiveBattlers[mCurrentTurn].GetIsPlayer)
        {
            Vector3 playerpos = new Vector3(mActiveBattlers[mCurrentTurn].gameObject.transform.position.x - 1.5f,
                                            mActiveBattlers[mCurrentTurn].gameObject.transform.position.y,
                                            mActiveBattlers[mCurrentTurn].gameObject.transform.position.z);

            mActiveBattlers[mCurrentTurn].transform.position = playerpos;
            mActiveBattlers[mCurrentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            Vector3 newpos = new Vector3(mActiveBattlers[mCurrentTurn].gameObject.transform.position.x + 1.5f,
                                         mActiveBattlers[mCurrentTurn].gameObject.transform.position.y,
                                         mActiveBattlers[mCurrentTurn].gameObject.transform.position.z);

            mActiveBattlers[mCurrentTurn].transform.position = newpos;
            mActiveBattlers[mCurrentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        mTurnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    private void UpdateBattle()
    {
        var allEnemiesDead = true;
        var allPlayersDead = true;

        for (int i = 0 ; i < mActiveBattlers.Count ; i++)
        {
            if (mActiveBattlers[i].GetCurrentHP < 0)
            {
                mActiveBattlers[i].SetCurrentHP(0);
            }

            if (mActiveBattlers[i].GetCurrentHP == 0)
            {
                //Handle dead battler
                if (mActiveBattlers[i].GetIsPlayer)
                {
                    mActiveBattlers[i].GetSprite.sprite = mActiveBattlers[i].GetDeadSprite;
                }
                else
                {
                    mActiveBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (mActiveBattlers[i].GetIsPlayer)
                {
                    allPlayersDead = false;

                    mActiveBattlers[i].GetSprite.sprite = mActiveBattlers[i].GetAliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                StartCoroutine(GameOverCo());
            }
        }
        else
        {
            while (mActiveBattlers[mCurrentTurn].GetCurrentHP == 0)
            {
                mCurrentTurn++;

                if (mCurrentTurn >= mActiveBattlers.Count)
                {
                    mCurrentTurn = 0;
                }
            }
        }
    }

    private void EnemyAttack()
    {
        List<int> players = new List<int>();

        for (int i = 0 ; i < mActiveBattlers.Count ; i++)
        {
            if (mActiveBattlers[i].GetIsPlayer && mActiveBattlers[i].GetCurrentHP > 0)
            {
                players.Add(i);
            }
        }

        var selectedTarget = players[Random.Range(0, players.Count)];
        var selectAttack = Random.Range(0, mActiveBattlers[mCurrentTurn].GetMovesAvailable.Length);
        var movePower = 0;

        for (int i = 0 ; i < movesList.Length ; i++)
        {
            if (movesList[i].moveName == mActiveBattlers[mCurrentTurn].GetMovesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, mActiveBattlers[selectedTarget].transform.position, mActiveBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, mActiveBattlers[mCurrentTurn].transform.position, mActiveBattlers[mCurrentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);
    }

    private void DealDamage(int target, int movePower)
    {
        float atkPwr = mActiveBattlers[mCurrentTurn].GetStrength + mActiveBattlers[mCurrentTurn].GetWeaponPower;
        float defPwr = mActiveBattlers[target].GetDefence + mActiveBattlers[target].GetArmorPower;
        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(mActiveBattlers[mCurrentTurn].GetCharName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + mActiveBattlers[target].GetCharName);

        mActiveBattlers[target].SetCurrentHP(mActiveBattlers[target].GetCurrentHP - damageToGive);

        Instantiate(theDamageNumber, mActiveBattlers[target].transform.position, Quaternion.identity).SetDamage(damageToGive);
        UpdateUIStats();
    }

    private void UpdateUIStats()
    {
        for (int i = 0 ; i < playerName.Length ; i++)
        {
            if (mActiveBattlers.Count > i)
            {
                if (mActiveBattlers[i].GetIsPlayer)
                {
                    BattleChar playerData = mActiveBattlers[i];

                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.GetCharName;
                    playerHP[i].text = Mathf.Clamp(playerData.GetCurrentHP, 0, int.MaxValue) + "/" + playerData.GetMaxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.GetCurrentMP, 0, int.MaxValue) + "/" + playerData.GetMaxMP;
                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion
    #region Public Functions/Methods

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if (!mIsBattleActive)
        {
            playerPrefabs[0] = GameManager.instance.playerStats[0].GetBattleCharacter;
            playerPrefabs[0].SetCharName(GameManager.instance.playerStats[0].GetCharacterName);
            mCanFlee = setCannotFlee;
            mIsBattleActive = true;
            GameManager.instance.battleActive = true;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            StartBattleMusic();

            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].GetCharName == GameManager.instance.playerStats[i].GetCharacterName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];

                            if (i == 0)
                            {
                                newPlayer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                            }
                            else if (i == 1)
                            {
                                newPlayer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                            }
                            else if (i == 2)
                            {
                                newPlayer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                            }

                            mActiveBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];

                            mActiveBattlers[i].SetMovesAvailable(thePlayer.GetAllowedMovesNames(movesList));
                            mActiveBattlers[i].SetCurrentHP(thePlayer.GetCurrentHP);
                            mActiveBattlers[i].SetMaxHP(thePlayer.GetMaxHP);
                            mActiveBattlers[i].SetCurrentMP(thePlayer.GetCurrentMP);
                            mActiveBattlers[i].SetMaxMP(thePlayer.GetMaxMP);
                            mActiveBattlers[i].SetStrength(thePlayer.GetStrength);
                            mActiveBattlers[i].SetDefense(thePlayer.GetDefence);
                            mActiveBattlers[i].SetWeaponPower(thePlayer.GetWeaponPower);
                            mActiveBattlers[i].SetArmorPower(thePlayer.GetArmorPower);
                        }
                    }
                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    var found = false;

                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].GetCharName == enemiesToSpawn[i].Trim())
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            mActiveBattlers.Add(newEnemy);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Debug.LogError($"Could not find enemy prefab for <{enemiesToSpawn[i].Trim()}> is it in the BattleManagerList and spelled correctly?", this);
                    }

                }
            }

            mTurnWaiting = true;
            mCurrentTurn = Random.Range(0, mActiveBattlers.Count);

            if (mActiveBattlers[mCurrentTurn].GetIsPlayer)
            {
                Vector3 playerpos = new Vector3(mActiveBattlers[mCurrentTurn].gameObject.transform.position.x - 1.5f,
                                                mActiveBattlers[mCurrentTurn].gameObject.transform.position.y,
                                                mActiveBattlers[mCurrentTurn].gameObject.transform.position.z);

                mActiveBattlers[mCurrentTurn].transform.position = playerpos;
                mActiveBattlers[mCurrentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                Vector3 newpos = new Vector3(mActiveBattlers[mCurrentTurn].gameObject.transform.position.x + 1.5f,
                                             mActiveBattlers[mCurrentTurn].gameObject.transform.position.y,
                                             mActiveBattlers[mCurrentTurn].gameObject.transform.position.z);

                mActiveBattlers[mCurrentTurn].transform.position = newpos;
                mActiveBattlers[mCurrentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            UpdateUIStats();
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        var movePower = 0;

        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, mActiveBattlers[selectedTarget].transform.position, mActiveBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, mActiveBattlers[mCurrentTurn].transform.position, mActiveBattlers[mCurrentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();

        for (int i = 0; i < mActiveBattlers.Count; i++)
        {
            if (!mActiveBattlers[i].GetIsPlayer)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (Enemies.Count > i && mActiveBattlers[Enemies[i]].GetCurrentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].SetMoveName(moveName);
                targetButtons[i].SetTarget(Enemies[i]);
                targetButtons[i].SetTargetName(mActiveBattlers[Enemies[i]].GetCharName);
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        if (mActiveBattlers[mCurrentTurn].GetMovesAvailable.Length < 1)
        {
            Debug.LogError("Prevented spell menu softlock add a close button and/or disable spells if move list is empty");
            return;
        }

        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (mActiveBattlers[mCurrentTurn].GetMovesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);
                magicButtons[i].SetSpellName(mActiveBattlers[mCurrentTurn].GetMovesAvailable[i]);
                magicButtons[i].GetNameText.text = magicButtons[i].GetSpellName;

                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].GetSpellName)
                    {
                        magicButtons[i].SetSpellCost(movesList[j].moveCost);
                        magicButtons[i].GetCostText.text = magicButtons[i].GetSpellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if (mCanFlee)
        {
            battleNotice.SetNotificationText("Can not flee this battle!");
            battleNotice.Activate();
        }
        else
        {
            var fleeSuccess = Random.Range(0, 100);

            if (fleeSuccess < chanceToFlee)
            {
                //end the battle
                //battleActive = false;
                //battleScene.SetActive(false);
                mIsFleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                NextTurn();
                battleNotice.SetNotificationText("Couldn't escape!");
                battleNotice.Activate();
            }
        }
    }

    [ContextMenu("Force End Battle")]
    public void EndBattle()
    {
        StartCoroutine(EndBattleCo());
    }

    #endregion
    #region Coroutines

    private IEnumerator EnemyMoveCo()
    {
        mTurnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    private IEnumerator EndBattleCo()
    {
        mIsBattleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();
        AudioManager.instance.PlayBGM(mOutsideBattleBGM);

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < mActiveBattlers.Count; i++)
        {
            if (mActiveBattlers[i].GetIsPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (mActiveBattlers[i].GetCharName == GameManager.instance.playerStats[j].GetCharacterName)
                    {
                        GameManager.instance.playerStats[j].SetCurrentHP(mActiveBattlers[i].GetCurrentHP);
                        GameManager.instance.playerStats[j].SetCurrentMP(mActiveBattlers[i].GetCurrentMP);
                    }
                }
            }

            Destroy(mActiveBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        mActiveBattlers.Clear();
        mCurrentTurn = 0;

        if (mIsFleeing)
        {
            GameManager.instance.battleActive = false;
            mIsFleeing = false;
        }
        else
        {
            BattleReward.instance.OpenRewardScreen(mRewardXP, mRewardItems);
        }
    }

    private IEnumerator GameOverCo()
    {
        mIsBattleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }

    #endregion
}