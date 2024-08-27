/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleChar.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using System;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private string[] movesAvailable = null;
    [SerializeField] private string charName = "";
    [SerializeField] private int currentHp = 0;
    [SerializeField] private int maxHP = 0;
    [SerializeField] private int currentMP = 0;
    [SerializeField] private int maxMP = 0;
    [SerializeField] private int strength = 0;
    [SerializeField] private int defence = 0;
    [SerializeField] private int wpnPower = 0;
    [SerializeField] private int armrPower = 0;
    [SerializeField] private SpriteRenderer theSprite = null;
    [SerializeField] private Sprite deadSprite = null;
    [SerializeField] private Sprite aliveSprite = null;
    [SerializeField] private float fadeSpeed = 1.0f;

    #endregion
    #region Private Variable Declarations Only

    private bool mShouldFade;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetMaxHP => maxHP;
    public int GetMaxMP => maxMP;
    public int GetDefence => defence;
    public int GetStrength => strength;
    public bool GetIsPlayer => isPlayer;
    public int GetCurrentHP => currentHp;
    public int GetCurrentMP => currentMP;
    public int GetWeaponPower => wpnPower;
    public int GetArmorPower => armrPower;
    public string GetCharName => charName;
    public Sprite GetDeadSprite => deadSprite;
    public Sprite GetAliveSprite => aliveSprite;
    public SpriteRenderer GetSprite => theSprite;
    public string[] GetMovesAvailable => movesAvailable;

    #endregion
    #region Setters/Mutators

    public int SetMaxMP(int amount) => maxMP = amount;
    public int SetMaxHP(int amount) => maxHP = amount;
    public int SetDefense(int amount) => defence = amount;
    public bool SetIsPlayer(bool yesNo) => isPlayer = yesNo;
    public int SetStrength(int amount) => strength = amount;
    public int SetCurrentHP(int amount) => currentHp = amount;
    public string SetCharName(string name) => charName = name;
    public int SetCurrentMP(int amount) => currentMP = amount;
    public int SetArmorPower(int amount) => armrPower = amount;
    public int SetWeaponPower(int amount) => wpnPower = amount;
    public string[] SetMovesAvailable(string[] moves) => movesAvailable = moves;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

#pragma warning disable IDE0051
    private void Start()
    {
        mShouldFade = false;
        theSprite.sprite = aliveSprite;
    }
#pragma warning restore IDE0051

    #endregion
    #region Implementation Private Methods/Functions

#pragma warning disable IDE0051
    private void Update()
    {
        if (mShouldFade)
        {
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), 
                                        Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), 
                                        Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), 
                                        Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
#pragma warning restore IDE0051

    #endregion
    #region Public Functions/Methods

    public void EnemyFade() => mShouldFade = true;

    public static implicit operator BattleChar(CharStats v)
    {
        throw new NotImplementedException();
    }

    #endregion
}