﻿/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleManager.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in BattleManager Prefab
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
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    private static BattleManager mInstance;

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

    public static BattleManager Access => mInstance;

    #endregion

    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [Space(10)]
    [Header("SCENE INFO >======================---")]
    [FormerlySerializedAs("backgroundSpriteRenderer")]
    [SerializeField] private SpriteRenderer BackgroundSpriteRenderer = null;
    [FormerlySerializedAs("battleScene")]
    [SerializeField] private GameObject BattleScene = null;
    [FormerlySerializedAs("uiButtonsHolder")]
    [SerializeField] private GameObject MenuCanvas = null;
    [Space(10)]
    [Header("TARGETING MENU >======================---")]
    [FormerlySerializedAs("targetMenu")]
    [SerializeField] private GameObject TargetCanvas = null;
    [FormerlySerializedAs("targetButtons")]
    [SerializeField] private BattleTargetButton[] EnemyTargetButtons = null;
    [Space(10)]
    [Header("MAGIC MENU >======================---")]
    [FormerlySerializedAs("magicMenu")]
    [SerializeField] private GameObject MagicCanvas = null;
    [FormerlySerializedAs("magicButtons")]
    [SerializeField] private BattleMagicSelect[] PlayerSpellButtons = null;
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
    private int mRewardXP;
    private int mCurrentTurn;
    private int mOutsideBattleBGM;
    private string[] mRewardItems;
    private List<BattleChar> mActiveBattlers;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetCurrentTurn => mCurrentTurn;
    public GameObject GetMagicMenu => MagicCanvas;
    public BattleNotification GetBattleNotice => battleNotice;
    public List<BattleChar> GetActiveBattlers => mActiveBattlers;

    #endregion
    #region Setters/Mutators

    public int SetRewardXP(int amount) => mRewardXP = amount;
    public string[] SetRewardItems(string[] newItems) => mRewardItems = newItems;
    public Sprite SetBackgroundSprite(Sprite newSprite) => BackgroundSpriteRenderer.sprite = newSprite;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

#pragma warning disable IDE0051
    private void Awake() => Singleton();
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void Start ()
    {
        mRewardXP = 0;
        mCurrentTurn = 0;
        mCanFlee = false;
        mRewardItems = null;
        mTurnWaiting = false;
        mActiveBattlers = new List<BattleChar>();
    }
#pragma warning restore IDE0051

    #endregion
    #region Implementation Private Methods/Functions

#pragma warning disable IDE0051
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
                if (mActiveBattlers[mCurrentTurn].GetIsAPlayer)
                {
                    MenuCanvas.SetActive(true);
                }
                else
                {
                    MenuCanvas.SetActive(false);

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
#pragma warning restore IDE0051

    private void StartBattleMusic()
    {
        mOutsideBattleBGM = AudioManager.Access.GetMusicCurrentTrack;

        AudioManager.Access.PlayMusic(0);
    }

    private void NextTurn()
    {
        if (mActiveBattlers[mCurrentTurn].GetIsAPlayer)
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

        if (mActiveBattlers[mCurrentTurn].GetIsAPlayer)
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
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0 ; i < mActiveBattlers.Count ; i++)
        {
            if (mActiveBattlers[i].GetCurrentHP < 0)
            {
                mActiveBattlers[i].SetCurrentHP(0);
            }

            if (mActiveBattlers[i].GetCurrentHP == 0)
            {
                //Handle dead battler
                if (mActiveBattlers[i].GetIsAPlayer)
                {
                    mActiveBattlers[i].GetSpriteRenderer.sprite = mActiveBattlers[i].GetDeadSprite;
                }
                else
                {
                    mActiveBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (mActiveBattlers[i].GetIsAPlayer)
                {
                    allPlayersDead = false;

                    mActiveBattlers[i].GetSpriteRenderer.sprite = mActiveBattlers[i].GetAliveSprite;
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
            if (mActiveBattlers[i].GetIsAPlayer && mActiveBattlers[i].GetCurrentHP > 0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];
        int selectAttack = Random.Range(0, mActiveBattlers[mCurrentTurn].GetListOfAttacks.Length);
        int movePower = 0;

        for (int i = 0 ; i < movesList.Length ; i++)
        {
            if (movesList[i].moveName == mActiveBattlers[mCurrentTurn].GetListOfAttacks[selectAttack])
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
        float damageCalc = atkPwr / defPwr * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(mActiveBattlers[mCurrentTurn].GetName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + mActiveBattlers[target].GetName);

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
                if (mActiveBattlers[i].GetIsAPlayer)
                {
                    BattleChar playerData = mActiveBattlers[i];

                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.GetName;
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
            playerPrefabs[0] = GameManager.Access.GetCharacterStats[0].GetBattleCharacter;
            playerPrefabs[0].SetName(GameManager.Access.GetCharacterStats[0].GetCharacterName);
            mCanFlee = setCannotFlee;
            mIsBattleActive = true;
            GameManager.Access.SetBattleActive(true);
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            BattleScene.SetActive(true);

            StartBattleMusic();

            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (GameManager.Access.GetCharacterStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].GetName == GameManager.Access.GetCharacterStats[i].GetCharacterName)
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

                            CharStats thePlayer = GameManager.Access.GetCharacterStats[i];

                            mActiveBattlers[i].SetListOfAttacks(thePlayer.GetAllowedMovesNames(movesList));
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
                    bool found = false;

                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].GetName == enemiesToSpawn[i].Trim())
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

            if (mActiveBattlers[mCurrentTurn].GetIsAPlayer)
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
        int movePower = 0;

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

        MenuCanvas.SetActive(false);
        TargetCanvas.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        TargetCanvas.SetActive(true);

        List<int> Enemies = new List<int>();

        for (int i = 0; i < mActiveBattlers.Count; i++)
        {
            if (!mActiveBattlers[i].GetIsAPlayer)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < EnemyTargetButtons.Length; i++)
        {
            if (Enemies.Count > i && mActiveBattlers[Enemies[i]].GetCurrentHP > 0)
            {
                EnemyTargetButtons[i].gameObject.SetActive(true);
                EnemyTargetButtons[i].SetMoveName(moveName);
                EnemyTargetButtons[i].SetTarget(Enemies[i]);
                EnemyTargetButtons[i].SetTargetName(mActiveBattlers[Enemies[i]].GetName);
            }
            else
            {
                EnemyTargetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        if (mActiveBattlers[mCurrentTurn].GetListOfAttacks.Length < 1)
        {
            Debug.LogError("Prevented spell menu softlock add a close button and/or disable spells if move list is empty");
            return;
        }

        MagicCanvas.SetActive(true);

        for (int i = 0; i < PlayerSpellButtons.Length; i++)
        {
            if (mActiveBattlers[mCurrentTurn].GetListOfAttacks.Length > i)
            {
                PlayerSpellButtons[i].gameObject.SetActive(true);
                PlayerSpellButtons[i].SetSpellName(mActiveBattlers[mCurrentTurn].GetListOfAttacks[i]);
                PlayerSpellButtons[i].GetSpellNameText.text = PlayerSpellButtons[i].GetSpellName;

                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == PlayerSpellButtons[i].GetSpellName)
                    {
                        PlayerSpellButtons[i].SetSpellCost(movesList[j].moveCost);
                        PlayerSpellButtons[i].GetSpellCostText.text = PlayerSpellButtons[i].GetSpellCost.ToString();
                    }
                }
            }
            else
            {
                PlayerSpellButtons[i].gameObject.SetActive(false);
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
            int fleeSuccess = Random.Range(0, 100);

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
    public void EndBattle() => StartCoroutine(EndBattleCo());

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
        MenuCanvas.SetActive(false);
        TargetCanvas.SetActive(false);
        MagicCanvas.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();
        AudioManager.Access.PlayMusic(mOutsideBattleBGM);

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < mActiveBattlers.Count; i++)
        {
            if (mActiveBattlers[i].GetIsAPlayer)
            {
                for (int j = 0; j < GameManager.Access.GetCharacterStats.Length; j++)
                {
                    if (mActiveBattlers[i].GetName == GameManager.Access.GetCharacterStats[j].GetCharacterName)
                    {
                        GameManager.Access.GetCharacterStats[j].SetCurrentHP(mActiveBattlers[i].GetCurrentHP);
                        GameManager.Access.GetCharacterStats[j].SetCurrentMP(mActiveBattlers[i].GetCurrentMP);
                    }
                }
            }

            Destroy(mActiveBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        BattleScene.SetActive(false);
        mActiveBattlers.Clear();
        mCurrentTurn = 0;

        if (mIsFleeing)
        {
            GameManager.Access.SetBattleActive(false);
            mIsFleeing = false;
        }
        else
        {
            BattleReward.Access.OpenRewardScreen(mRewardXP, mRewardItems);
        }
    }

    private IEnumerator GameOverCo()
    {
        mIsBattleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        BattleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }

    #endregion
}