using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Bonehead Games

public class BattleManager : MonoBehaviour {

    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;
    private bool battlerReady;
    public GameObject uiButtonsHolder;

    public BattleMove[] movesList;
    public GameObject enemyAttackEffect;

    public DamageNumber theDamageNumber;

    public Text[] playerName, playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 35;
    private bool fleeing;

    public string gameOverScene;

    public int rewardXP;
    public string[] rewardItems;

    public bool cannotFlee;

    public Sprite backgroundSprite
    {
        set
        {
            backgroundSpriteRenderer.sprite = value;
        }
    }

    public SpriteRenderer backgroundSpriteRenderer;

    private int m_outsideBattleBGM;

    void Start () {
        instance = this;
        DontDestroyOnLoad(gameObject);
       
    activeBattlers = new List<BattleChar>();

        battlerReady = false;

    }


    void Update () {
		if(Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Eyeball"}, false);
        }

        if(battleActive)
        {
            if(turnWaiting)
            {
                if(activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                } else
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

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if(!battleActive)
        {
            playerPrefabs[0] = GameManager.instance.playerStats[0].battleChar;
            playerPrefabs[0].charName = GameManager.instance.playerStats[0].charName;

            cannotFlee = setCannotFlee;

            battleActive = true;

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
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {

                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);


                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].movesAvailable = thePlayer.GetAllowedMovesNames(movesList);
                            activeBattlers[i].currentHp = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defence = thePlayer.defence;
                            activeBattlers[i].wpnPower = thePlayer.wpnPwr;
                            activeBattlers[i].armrPower = thePlayer.armrPwr;

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
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i].Trim())
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

            if (activeBattlers[currentTurn].isPlayer)
            {
                Vector3 playerpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x - 1.5f, activeBattlers[currentTurn].gameObject.transform.position.y, activeBattlers[currentTurn].gameObject.transform.position.z);
                activeBattlers[currentTurn].transform.position = playerpos;
                activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                Vector3 newpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x + 1.5f, activeBattlers[currentTurn].gameObject.transform.position.y, activeBattlers[currentTurn].gameObject.transform.position.z);
                activeBattlers[currentTurn].transform.position = newpos;
                activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            UpdateUIStats();
        }
    }

    private void StartBattleMusic()
    {
        m_outsideBattleBGM = AudioManager.instance.bgmCurrentTrack;

        AudioManager.instance.PlayBGM(0);
    }

    public void NextTurn()
    {
        if (activeBattlers[currentTurn].isPlayer)
        {
            Vector3 playerpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x + 1.5f, activeBattlers[currentTurn].gameObject.transform.position.y, activeBattlers[currentTurn].gameObject.transform.position.z);
            activeBattlers[currentTurn].transform.position = playerpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            Vector3 newpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x - 1.5f, activeBattlers[currentTurn].gameObject.transform.position.y, activeBattlers[currentTurn].gameObject.transform.position.z);
            activeBattlers[currentTurn].transform.position = newpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        currentTurn++;

        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        if (activeBattlers[currentTurn].isPlayer)
        {
            Vector3 playerpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x - 1.5f, activeBattlers[currentTurn].gameObject.transform.position.y, activeBattlers[currentTurn].gameObject.transform.position.z);
            activeBattlers[currentTurn].transform.position = playerpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            Vector3 newpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x + 1.5f, activeBattlers[currentTurn].gameObject.transform.position.y, activeBattlers[currentTurn].gameObject.transform.position.z);
            activeBattlers[currentTurn].transform.position = newpos;
            activeBattlers[currentTurn].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].currentHp < 0)
            {
                activeBattlers[i].currentHp = 0;
            }

            if(activeBattlers[i].currentHp == 0)
            {
                //Handle dead battler
                if(activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                } else
                {
                    activeBattlers[i].EnemyFade();
                }

            } else
            {
                if(activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                } else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if(allEnemiesDead || allPlayersDead)
        {
            if(allEnemiesDead)
            {
                //end battle in victory
                StartCoroutine(EndBattleCo());
            } else
            {
                //end battle in failure
                StartCoroutine(GameOverCo());
            }

            /* battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false; */
        } else
        {
            while(activeBattlers[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        /*Vector3 newpos = new Vector3(activeBattlers[currentTurn].gameObject.transform.position.x + 0.5f, activeBattlers[currentTurn].gameObject.transform.position.y, activeBattlers[currentTurn].gameObject.transform.position.z);
        activeBattlers[currentTurn].transform.position = newpos;*/
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0)
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];

        //activeBattlers[selectedTarget].currentHp -= 30;

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for(int i = 0; i < movesList.Length; i++)
        {
            if(movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
        float defPwr = activeBattlers[target].defence + activeBattlers[target].armrPower;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

        activeBattlers[target].currentHp -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for (int i = 0; i < playerName.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;

                } else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            } else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        
        int movePower = 0;
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
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for(int i = 0; i < targetButtons.Length; i++)
        {
            if(Enemies.Count > i && activeBattlers[Enemies[i]].currentHp > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            } else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        if(activeBattlers[currentTurn].movesAvailable.Length < 1)
        {
            Debug.LogError("Prevented spell menu softlock add a close button and/or disable spells if move list is empty");
            return;
        }
        magicMenu.SetActive(true);

        for(int i = 0; i < magicButtons.Length; i++)
        {
            if(activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                for(int j = 0; j < movesList.Length; j++)
                {
                    if(movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }

            } else
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
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < chanceToFlee)
            {
                //end the battle
                //battleActive = false;
                //battleScene.SetActive(false);
                fleeing = true;
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
    void EndBattle()
    {
        StartCoroutine(EndBattleCo());
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();
        AudioManager.instance.PlayBGM(m_outsideBattleBGM);

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHp;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;
        if(fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        } else
        {
            BattleReward.instance.OpenRewardScreen(rewardXP, rewardItems);
        }

       // AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }
}
