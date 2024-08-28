/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleChar.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in Resources/Prefabs/Players to give initial stats
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 28, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleChar : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [FormerlySerializedAs("charName")]
    [SerializeField] private string Name = "";
    [FormerlySerializedAs("isPlayer")]
    [SerializeField] private bool IsAPlayer = false;
    [FormerlySerializedAs("theSprite")]
    [SerializeField] private SpriteRenderer MySpriteRenderer = null;
    [FormerlySerializedAs("deadSprite")]
    [SerializeField] private Sprite MyDeadSprite = null;
    [FormerlySerializedAs("aliveSprite")]
    [SerializeField] private Sprite MyAliveSprite = null;
    [FormerlySerializedAs("maxHP")]
    [SerializeField] private int MaximumHP = 0;
    [FormerlySerializedAs("maxMP")]
    [SerializeField] private int MaximumMP = 0;
    [FormerlySerializedAs("strength")]
    [SerializeField] private int Strength = 0;
    [FormerlySerializedAs("defence")]
    [SerializeField] private int Defence = 0;
    [FormerlySerializedAs("wpnPower")]
    [SerializeField] private int WeaponPower = 0;
    [FormerlySerializedAs("armrPower")]
    [SerializeField] private int ArmorPower = 0;
    [FormerlySerializedAs("movesAvailable")]
    [SerializeField] private string[] ListOfAttacks = null;
    [FormerlySerializedAs("fadeSpeed")]
    [SerializeField] private float DeathFadeSpeed = 1.0f;

    #endregion
    #region Private Variable Declarations Only

    private int mCurrentHP;
    private int mCurrentMP;
    private bool mShouldFade;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public string GetName => Name;
    public int GetMaxHP => MaximumHP;
    public int GetMaxMP => MaximumMP;
    public int GetDefence => Defence;
    public int GetStrength => Strength;
    public bool GetIsAPlayer => IsAPlayer;
    public int GetCurrentHP => mCurrentHP;
    public int GetCurrentMP => mCurrentMP;
    public int GetArmorPower => ArmorPower;
    public int GetWeaponPower => WeaponPower;
    public Sprite GetDeadSprite => MyDeadSprite;
    public Sprite GetAliveSprite => MyAliveSprite;
    public string[] GetListOfAttacks => ListOfAttacks;
    public SpriteRenderer GetSpriteRenderer => MySpriteRenderer;

    #endregion
    #region Setters/Mutators

    public string SetName(string name) => Name = name;
    public int SetMaxMP(int amount) => MaximumMP = amount;
    public int SetMaxHP(int amount) => MaximumHP = amount;
    public int SetDefense(int amount) => Defence = amount;
    public int SetStrength(int amount) => Strength = amount;
    public bool SetIsAPlayer(bool yesNo) => IsAPlayer = yesNo;
    public int SetCurrentHP(int amount) => mCurrentHP = amount;
    public int SetCurrentMP(int amount) => mCurrentMP = amount;
    public int SetArmorPower(int amount) => ArmorPower = amount;
    public int SetWeaponPower(int amount) => WeaponPower = amount;
    public string[] SetListOfAttacks(string[] moves) => ListOfAttacks = moves;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

#pragma warning disable IDE0051
    private void Start() => InitializeVariables();
#pragma warning restore IDE0051

    private void InitializeVariables()
    {
        mShouldFade = false;
        mCurrentHP = MaximumHP;
        mCurrentMP = MaximumMP;
        MySpriteRenderer.sprite = MyAliveSprite;
    }

    #endregion
    #region Implementation Private Methods/Functions

#pragma warning disable IDE0051
    private void Update()
    {
        if (mShouldFade)
        {
            MySpriteRenderer.color = new Color(Mathf.MoveTowards(MySpriteRenderer.color.r, 1f, DeathFadeSpeed * Time.deltaTime), 
                                        Mathf.MoveTowards(MySpriteRenderer.color.g, 0f, DeathFadeSpeed * Time.deltaTime), 
                                        Mathf.MoveTowards(MySpriteRenderer.color.b, 0f, DeathFadeSpeed * Time.deltaTime), 
                                        Mathf.MoveTowards(MySpriteRenderer.color.a, 0f, DeathFadeSpeed * Time.deltaTime));

            if (MySpriteRenderer.color.a <= 0)
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