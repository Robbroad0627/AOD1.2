/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleManager.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
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
    [SerializeField] private GameObject battleScene = null;
    [SerializeField] private Transform[] playerPositions = null;
    [SerializeField] private Transform[] enemyPositions = null;
    [SerializeField] private BattleChar[] playerPrefabs = null;
    [SerializeField] private BattleChar[] enemyPrefabs = null;
    [SerializeField] private List<BattleChar> activeBattlers = new List<BattleChar>();
    [SerializeField] private int currentTurn = 0;
    [SerializeField] private bool turnWaiting = false;
    [SerializeField] private GameObject uiButtonsHolder = null;
    [SerializeField] private BattleMove[] movesList = null;
    [SerializeField] private GameObject enemyAttackEffect = null;
    [SerializeField] private DamageNumber theDamageNumber = null;
    [SerializeField] private Text[] playerName = null;
    [SerializeField] private Text[] playerHP = null;
    [SerializeField] private Text[] playerMP = null;
    [SerializeField] private GameObject targetMenu = null;
    [SerializeField] private BattleTargetButton[] targetButtons = null;
    [SerializeField] private GameObject magicMenu = null;
    [SerializeField] private BattleMagicSelect[] magicButtons = null;
    [SerializeField] private BattleNotification battleNotice = null;
    [SerializeField] private int chanceToFlee = 35;
    [SerializeField] private string gameOverScene = "";
    [SerializeField] private int rewardXP = 0;
    [SerializeField] private string[] rewardItems = null;
    [SerializeField] private bool cannotFlee = false;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer = null;

    #endregion
    #region Private Variable Declarations Only

    private bool mIsFleeing;
    private bool mIsBattleActive;
    private bool mIsBattlerReady;
    private int mOutsideBattleBGM;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetCurrentTurn => currentTurn;
    public GameObject GetMagicMenu => magicMenu;
    public BattleNotification GetBattleNotice => battleNotice;
    public List<BattleChar> GetActiveBattlers => activeBattlers;

    #endregion
    #region Setters/Mutators

    public int SetRewardXP(int amount) => rewardXP = amount;
    public string[] SetRewardItems(string[] newItems) => rewardItems = newItems;
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
        mIsBattlerReady = false;
        activeBattlers = new List<BattleChar>();
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
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].GetIsPlayer)
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
        if (activeBattlers[currentTurn].GetIsPlayer)
        {
            Vector3 playerpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x + 1.5f,
                                            activeBattlers[currentTurn].gameObject.transform.position.y,
                                            activeBattlers[currentTurn].gameObject.transform.position.z);

            activeBattlers[currentTurn].transform.position = playerpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            Vector3 newpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x - 1.5f,
                                         activeBattlers[currentTurn].gameObject.transform.position.y,
                                         activeBattlers[currentTurn].gameObject.transform.position.z);

            activeBattlers[currentTurn].transform.position = newpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        currentTurn++;

        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        if (activeBattlers[currentTurn].GetIsPlayer)
        {
            Vector3 playerpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x - 1.5f,
                                            activeBattlers[currentTurn].gameObject.transform.position.y,
                                            activeBattlers[currentTurn].gameObject.transform.position.z);

            activeBattlers[currentTurn].transform.position = playerpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            Vector3 newpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x + 1.5f,
                                         activeBattlers[currentTurn].gameObject.transform.position.y,
                                         activeBattlers[currentTurn].gameObject.transform.position.z);

            activeBattlers[currentTurn].transform.position = newpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    private void UpdateBattle()
    {
        var allEnemiesDead = true;
        var allPlayersDead = true;

        for (int i = 0 ; i < activeBattlers.Count ; i++)
        {
            if (activeBattlers[i].GetCurrentHP < 0)
            {
                activeBattlers[i].SetCurrentHP(0);
            }

            if (activeBattlers[i].GetCurrentHP == 0)
            {
                //Handle dead battler
                if (activeBattlers[i].GetIsPlayer)
                {
                    activeBattlers[i].GetSprite.sprite = activeBattlers[i].GetDeadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].GetIsPlayer)
                {
                    allPlayersDead = false;

                    activeBattlers[i].GetSprite.sprite = activeBattlers[i].GetAliveSprite;
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
            while (activeBattlers[currentTurn].GetCurrentHP == 0)
            {
                currentTurn++;

                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    private void EnemyAttack()
    {
        List<int> players = new List<int>();

        for (int i = 0 ; i < activeBattlers.Count ; i++)
        {
            if (activeBattlers[i].GetIsPlayer && activeBattlers[i].GetCurrentHP > 0)
            {
                players.Add(i);
            }
        }

        var selectedTarget = players[Random.Range(0, players.Count)];
        var selectAttack = Random.Range(0, activeBattlers[currentTurn].GetMovesAvailable.Length);
        var movePower = 0;

        for (int i = 0 ; i < movesList.Length ; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].GetMovesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);
    }

    private void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].GetStrength + activeBattlers[currentTurn].GetWeaponPower;
        float defPwr = activeBattlers[target].GetDefence + activeBattlers[target].GetArmorPower;
        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].GetCharName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].GetCharName);

        activeBattlers[target].SetCurrentHP(activeBattlers[target].GetCurrentHP - damageToGive);

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, Quaternion.identity).SetDamage(damageToGive);
        UpdateUIStats();
    }

    private void UpdateUIStats()
    {
        for (int i = 0 ; i < playerName.Length ; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].GetIsPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

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
            playerPrefabs[0] = GameManager.instance.playerStats[0].battleChar;
            playerPrefabs[0].SetCharName(GameManager.instance.playerStats[0].charName);
            cannotFlee = setCannotFlee;
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
                        if (playerPrefabs[j].GetCharName == GameManager.instance.playerStats[i].charName)
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

                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];

                            activeBattlers[i].SetMovesAvailable(thePlayer.GetAllowedMovesNames(movesList));
                            activeBattlers[i].SetCurrentHP(thePlayer.currentHP);
                            activeBattlers[i].SetMaxHP(thePlayer.maxHP);
                            activeBattlers[i].SetCurrentMP(thePlayer.currentMP);
                            activeBattlers[i].SetMaxMP(thePlayer.maxMP);
                            activeBattlers[i].SetStrength(thePlayer.strength);
                            activeBattlers[i].SetDefense(thePlayer.defence);
                            activeBattlers[i].SetWeaponPower(thePlayer.wpnPwr);
                            activeBattlers[i].SetArmorPower(thePlayer.armrPwr);
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
                            activeBattlers.Add(newEnemy);
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

            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);

            if (activeBattlers[currentTurn].GetIsPlayer)
            {
                Vector3 playerpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x - 1.5f,
                                                activeBattlers[currentTurn].gameObject.transform.position.y,
                                                activeBattlers[currentTurn].gameObject.transform.position.z);

                activeBattlers[currentTurn].transform.position = playerpos;
                activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                Vector3 newpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x + 1.5f,
                                             activeBattlers[currentTurn].gameObject.transform.position.y,
                                             activeBattlers[currentTurn].gameObject.transform.position.z);

                activeBattlers[currentTurn].transform.position = newpos;
                activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
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
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].GetIsPlayer)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].GetCurrentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].GetCharName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        if (activeBattlers[currentTurn].GetMovesAvailable.Length < 1)
        {
            Debug.LogError("Prevented spell menu softlock add a close button and/or disable spells if move list is empty");
            return;
        }

        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].GetMovesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);
                magicButtons[i].SetSpellName(activeBattlers[currentTurn].GetMovesAvailable[i]);
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
        if (cannotFlee)
        {
            battleNotice.theText.text = "Can not flee this battle!";
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
                battleNotice.theText.text = "Couldn't escape!";
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
        turnWaiting = false;
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

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].GetIsPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].GetCharName == GameManager.instance.playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].GetCurrentHP;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].GetCurrentMP;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;

        if (mIsFleeing)
        {
            GameManager.instance.battleActive = false;
            mIsFleeing = false;
        }
        else
        {
            BattleReward.instance.OpenRewardScreen(rewardXP, rewardItems);
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