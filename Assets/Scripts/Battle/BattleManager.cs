/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleManager.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in BattleManager Prefab
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 28, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton - this Class has One and Only One Instance

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
    #region Private Variables/Fields Exposed to Inspector for Editing

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
    [FormerlySerializedAs("playerName")]
    [SerializeField] private Text[] PlayersNameText = null;
    [FormerlySerializedAs("playerHP")]
    [SerializeField] private Text[] PlayersHPText = null;
    [FormerlySerializedAs("playerMP")]
    [SerializeField] private Text[] PlayersMPText = null;
    [FormerlySerializedAs("playerPrefabs")]
    [SerializeField] private BattleChar[] PlayerCharacters = null;
    [FormerlySerializedAs("playerPositions")]
    [SerializeField] private Transform[] PlayerCharacterPositions = null;
    [Space(10)]
    [Header("ENEMY INFO >======================---")]
    [FormerlySerializedAs("enemyPrefabs")]
    [SerializeField] private BattleChar[] Enemies = null;
    [FormerlySerializedAs("enemyPositions")]
    [SerializeField] private Transform[] EnemyPositions = null;
    [FormerlySerializedAs("enemyAttackEffect")]
    [SerializeField] private GameObject EnemyAttackEffect = null;
    [Space(10)]
    [Header("BATTLE DATA >======================---")]
    [FormerlySerializedAs("battleNotice")]
    [SerializeField] private BattleNotification BattleNotice = null;
    [FormerlySerializedAs("chanceToFlee")]
    [SerializeField] private int FleeChance = 35;
    [FormerlySerializedAs("gameOverScene")]
    [SerializeField] private string GameOverSceneName = "";
    [FormerlySerializedAs("movesList")]
    [SerializeField] private BattleMove[] ListOfAttacks = null;
    [FormerlySerializedAs("theDamageNumber")]
    [SerializeField] private DamageNumber DamageNumberDisplay = null;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private bool mCanFlee;
    private int mRewardXP;
    private bool mIsFleeing;
    private int mBattleMusic;
    private int mCurrentTurn;
    private bool mTurnWaiting;
    private bool mIsBattleActive;
    private string[] mRewardItems;
    private List<BattleChar> mActiveBattlers;

    #endregion

    //GETTERS/SETTERS
    #region Public Getters/Accessors for use Outside of this Class Only

    public int GetCurrentTurn => mCurrentTurn;
    public GameObject GetMagicMenu => MagicCanvas;
    public BattleNotification GetBattleNotice => BattleNotice;
    public List<BattleChar> GetActiveBattlers => mActiveBattlers;

    #endregion
    #region Public Setters/Mutators for use Outside of this Class Only

    public int SetRewardXP(int amount) => mRewardXP = amount;
    public string[] SetRewardItems(string[] newItems) => mRewardItems = newItems;
    public Sprite SetBackgroundSprite(Sprite newSprite) => BackgroundSpriteRenderer.sprite = newSprite;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Awake() => Singleton();
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void Start() => InitializeVariables();
#pragma warning restore IDE0051

    private void InitializeVariables()
    {
        mRewardXP = 0;
        mCurrentTurn = 0;
        mBattleMusic = 0;
        mCanFlee = false;
        mIsFleeing = false;
        mRewardItems = null;
        mTurnWaiting = false;
        mIsBattleActive = false;
        mActiveBattlers = new List<BattleChar>();
    }

    #endregion
    #region Private Implementation Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Update ()
    {
        // for testing should be removed before compilation
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
                    StartCoroutine(EnemyMoveCo());
                }
            }

            // for testing should be removed before compilation
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
	}
#pragma warning restore IDE0051

    private void StartBattleMusic()
    {
        mBattleMusic = AudioManager.Access.GetMusicCurrentTrack;
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
                    mActiveBattlers[i].SetShouldFade(true);
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

        for (int i = 0 ; i < ListOfAttacks.Length ; i++)
        {
            if (ListOfAttacks[i].moveName == mActiveBattlers[mCurrentTurn].GetListOfAttacks[selectAttack])
            {
                Instantiate(ListOfAttacks[i].theEffect, mActiveBattlers[selectedTarget].transform.position, mActiveBattlers[selectedTarget].transform.rotation);
                movePower = ListOfAttacks[i].movePower;
            }
        }

        Instantiate(EnemyAttackEffect, mActiveBattlers[mCurrentTurn].transform.position, mActiveBattlers[mCurrentTurn].transform.rotation);
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

        Instantiate(DamageNumberDisplay, mActiveBattlers[target].transform.position, Quaternion.identity).SetDamage(damageToGive);
        UpdateUIStats();
    }

    private void UpdateUIStats()
    {
        for (int i = 0 ; i < PlayersNameText.Length ; i++)
        {
            if (mActiveBattlers.Count > i)
            {
                if (mActiveBattlers[i].GetIsAPlayer)
                {
                    BattleChar playerData = mActiveBattlers[i];

                    PlayersNameText[i].gameObject.SetActive(true);
                    PlayersNameText[i].text = playerData.GetName;
                    PlayersHPText[i].text = Mathf.Clamp(playerData.GetCurrentHP, 0, int.MaxValue) + "/" + playerData.GetMaxHP;
                    PlayersMPText[i].text = Mathf.Clamp(playerData.GetCurrentMP, 0, int.MaxValue) + "/" + playerData.GetMaxMP;
                }
                else
                {
                    PlayersNameText[i].gameObject.SetActive(false);
                }
            }
            else
            {
                PlayersNameText[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion
    #region Public Functions/Methods for use Outside of this Class

    public void BattleStart(string[] enemiesToSpawn, bool setFlee)
    {
        if (!mIsBattleActive)
        {
            PlayerCharacters[0] = GameManager.Access.GetCharacterStats[0].GetBattleCharacter;
            PlayerCharacters[0].SetName(GameManager.Access.GetCharacterStats[0].GetCharacterName);
            mCanFlee = setFlee;
            mIsBattleActive = true;
            GameManager.Access.SetBattleActive(true);
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            BattleScene.SetActive(true);

            StartBattleMusic();

            for (int i = 0; i < PlayerCharacterPositions.Length; i++)
            {
                if (GameManager.Access.GetCharacterStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < PlayerCharacters.Length; j++)
                    {
                        if (PlayerCharacters[j].GetName == GameManager.Access.GetCharacterStats[i].GetCharacterName)
                        {
                            BattleChar newPlayer = Instantiate(PlayerCharacters[j], PlayerCharacterPositions[i].position, PlayerCharacterPositions[i].rotation);
                            newPlayer.transform.parent = PlayerCharacterPositions[i];

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

                            mActiveBattlers[i].SetListOfAttacks(thePlayer.GetAllowedMovesNames(ListOfAttacks));
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

                    for (int j = 0; j < Enemies.Length; j++)
                    {
                        if (Enemies[j].GetName == enemiesToSpawn[i].Trim())
                        {
                            BattleChar newEnemy = Instantiate(Enemies[j], EnemyPositions[i].position, EnemyPositions[i].rotation);
                            newEnemy.transform.parent = EnemyPositions[i];
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

        for (int i = 0; i < ListOfAttacks.Length; i++)
        {
            if (ListOfAttacks[i].moveName == moveName)
            {
                Instantiate(ListOfAttacks[i].theEffect, mActiveBattlers[selectedTarget].transform.position, mActiveBattlers[selectedTarget].transform.rotation);
                movePower = ListOfAttacks[i].movePower;
            }
        }

        Instantiate(EnemyAttackEffect, mActiveBattlers[mCurrentTurn].transform.position, mActiveBattlers[mCurrentTurn].transform.rotation);
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

                for (int j = 0; j < ListOfAttacks.Length; j++)
                {
                    if (ListOfAttacks[j].moveName == PlayerSpellButtons[i].GetSpellName)
                    {
                        PlayerSpellButtons[i].SetSpellCost(ListOfAttacks[j].moveCost);
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
            BattleNotice.SetNotificationText("Can not flee this battle!");
            BattleNotice.Activate();
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);

            if (fleeSuccess < FleeChance)
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
                BattleNotice.SetNotificationText("Couldn't escape!");
                BattleNotice.Activate();
            }
        }
    }

    #endregion
    #region Private Coroutines continue until done without interuption

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
        AudioManager.Access.PlayMusic(mBattleMusic);

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
        SceneManager.LoadScene(GameOverSceneName);
    }

    #endregion
    #region Cheating Stuff

    [ContextMenu("Force End Battle")]
    public void EndBattle() => StartCoroutine(EndBattleCo());

    #endregion
}