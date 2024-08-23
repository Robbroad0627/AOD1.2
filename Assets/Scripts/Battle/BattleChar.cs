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
    // The Proper way to expose variables to the editor
    // without exposing them outside of this script
    // [SerializeField] private variable variableName = initialvalue;
    // Example:
    // [SerializeField] private float health = 10.0f;
    public bool isPlayer;
    public string[] movesAvailable;
    public string charName="";
    public int currentHp, maxHP, currentMP, maxMP, strength, defence, wpnPower, armrPower;
    public bool hasDied;
    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;
    public float fadeSpeed = 1f;

    #endregion
    #region Private Variable Declarations Only

    private bool shouldFade;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public bool isDead => currentHp <= 0;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

    private void Start()
    {
        theSprite.sprite = aliveSprite;
    }

    #endregion
    #region Implementation Private Methods/Functions

    private void Update()
    {
        if (shouldFade)
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

    #endregion
    #region Public Functions/Methods

    public void EnemyFade()
    {
        shouldFade = true;
    }

    public static implicit operator BattleChar(CharStats v)
    {
        throw new NotImplementedException();
    }

    #endregion
}