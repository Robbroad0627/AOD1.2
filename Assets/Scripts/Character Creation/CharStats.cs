using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class CharStats : MonoBehaviour {

    public string charName;
    public BattleChar battleChar;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;

    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLvlBonus;
    public int strength;
    public int defence;
    public int wpnPwr;
    public int armrPwr;
    public string equippedWpn;
    public string equippedArmr;
    public Sprite charIamge;
    internal string equippedHeadArmr="";
    internal string equippedBodyArmr="";
    internal string equippedHandArmr="";
    internal string equippedLegsArmr="";
    internal string equippedFeetArmr="";
    internal string equippedOtherArmr="";
    internal string equippedShieldArmr="";


    // Use this for initialization
    void Start () {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
	}



    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //determine whether to add to str or def based on odd or even level
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                maxMP += mpLvlBonus[playerLevel];
                currentMP = maxMP;
            }
        }

        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
