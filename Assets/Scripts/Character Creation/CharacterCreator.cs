/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: CharacterCreator.cs
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
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterCreator : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Image portrait;
    [SerializeField] private InputField nameField;

    #endregion
    #region Private Variables

    private bool mIsMale;
    private int mPlayerHP;
    private int mPlayerMP;
    private int mBaseValue;
    private string mPlayerSex;
    private Sprite mPlayerPortrait;
    private int mNumberOfAttributes;
    private List<Sprite> mPortraitList;
    private CharacterAttributes.Races mPlayerRace;
    private CharacterAttributes.Classes mPlayerClass;
    private Dictionary<CharacterAttributes.BaseAttributes, int> mPlayerAttributes;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public bool GetIsMale => mIsMale;
    public int GetPlayerHP => mPlayerHP;
    public int GetPlayerMP => mPlayerMP;
    public CharacterAttributes.Races GetPlayerRace => mPlayerRace;
    public CharacterAttributes.Classes GetPlayerClass => mPlayerClass;
    public Dictionary<CharacterAttributes.BaseAttributes, int> GetPlayerAttributes => mPlayerAttributes;

    #endregion
    #region Setters/Mutators

    public bool SetIsMale(bool male) => mIsMale = male;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

    private void Awake()
    {
        mIsMale = true;
        mBaseValue = 10;
        mNumberOfAttributes = 6;
        nameField = GameObject.Find("nameText").GetComponent<InputField>();
    }

    #endregion
    #region Public Functions/Methods

    public void CheckRace()
    {
        InitializeAttributes();

        switch (mPlayerRace)
        {
            case CharacterAttributes.Races.Dwarf:
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Strength] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 1;
                mPortraitList = mIsMale == true ? GetComponent<PortraitHandler>().mDwarf : GetComponent<PortraitHandler>().fDwarf;
                mPlayerSex = mIsMale == true ? "M" : "F";
                break;

            case CharacterAttributes.Races.Elf:
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Intelligence] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution] -= 1;
                mPortraitList = mIsMale == true ? GetComponent<PortraitHandler>().mElf : GetComponent<PortraitHandler>().fElf;
                mPlayerSex = mIsMale == true ? "M" : "F";
                break;

            case CharacterAttributes.Races.HalfElf:
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Wisdom] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 1;
                mPortraitList = mIsMale == true ? GetComponent<PortraitHandler>().mHalf_Elf : GetComponent<PortraitHandler>().fHalf_Elf;
                mPlayerSex = mIsMale == true ? "M" : "F";
                break;

            case CharacterAttributes.Races.Halfling:
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Wisdom] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Strength] -= 1;
                mPortraitList = mIsMale == true ? GetComponent<PortraitHandler>().mHalfling : GetComponent<PortraitHandler>().fHalfling;
                mPlayerSex = mIsMale == true ? "M" : "F";
                break;

            case CharacterAttributes.Races.HalfOrc:
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Strength] += 1;
                mPlayerAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 2;
                mPortraitList = mIsMale == true ? GetComponent<PortraitHandler>().mHalf_Orc : GetComponent<PortraitHandler>().fHalf_Orc;
                mPlayerSex = mIsMale == true ? "M" : "F";
                break;

            case CharacterAttributes.Races.Human:
                int ran = Random.Range(0, System.Enum.GetValues(typeof(CharacterAttributes.BaseAttributes)).Length);
                mPlayerAttributes[(CharacterAttributes.BaseAttributes)ran] += 2;
                mPortraitList = mIsMale == true ? GetComponent<PortraitHandler>().mHuman : GetComponent<PortraitHandler>().fHuman;
                mPlayerSex = mIsMale == true ? "M" : "F";
                break;

            default:
                Debug.Log("ERROR - Race Not Found");
                break;
        }

        mPlayerPortrait = mPortraitList[0];
        portrait.sprite = mPlayerPortrait;
    }

    public void CheckClass()
    {
        mPlayerHP = mBaseValue;
        mPlayerMP = 0;

        switch (mPlayerClass)
        {
            case CharacterAttributes.Classes.Cleric:
                mPlayerHP = 8 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution];
                mPlayerMP = 15 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                mPlayerPortrait = mPortraitList[0];
                portrait.sprite = mPlayerPortrait;
                break;

            case CharacterAttributes.Classes.Druid:
                mPlayerHP = 8 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution];
                mPlayerMP = 15 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                mPlayerPortrait = mPortraitList[1];
                portrait.sprite = mPlayerPortrait;
                break;

            case CharacterAttributes.Classes.Fighter:
                mPlayerHP = 10 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution];
                mPlayerPortrait = mPortraitList[2];
                portrait.sprite = mPlayerPortrait;
                break;

            case CharacterAttributes.Classes.MagicUser:
                mPlayerHP = 6 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution];
                mPlayerMP = 20 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Intelligence];
                mPlayerPortrait = mPortraitList[3];
                portrait.sprite = mPlayerPortrait;
                break;

            case CharacterAttributes.Classes.Paladin:
                mPlayerHP = 10 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution];
                mPlayerMP = 10 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                mPlayerPortrait = mPortraitList[4];
                portrait.sprite = mPlayerPortrait;
                break;

            case CharacterAttributes.Classes.Ranger:
                mPlayerHP = 10 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution];
                mPlayerMP = 15 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                mPlayerPortrait = mPortraitList[5];
                portrait.sprite = mPlayerPortrait;
                break;

            case CharacterAttributes.Classes.Thief:
                mPlayerHP = 6 + mPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution];
                mPlayerPortrait = mPortraitList[6];
                portrait.sprite = mPlayerPortrait;
                break;
        }
    }

    public void ChangeClass(bool goNext)
    {
        bool foundIt = false;

        if (goNext)
        {
            foreach (CharacterAttributes.Classes thisClass in System.Enum.GetValues(typeof(CharacterAttributes.Classes)))
            {
                if (foundIt)
                {
                    foundIt = false;
                    mPlayerClass = thisClass;

                    break;
                }
                else if (mPlayerClass == thisClass)
                {
                    foundIt = true;
                }
            }

            if (foundIt)
            {
                mPlayerClass = 0;
            }
        }
        else
        {
            int lastValue = System.Enum.GetValues(typeof(CharacterAttributes.Classes)).Length - 1;

            CharacterAttributes.Classes lastClass = (CharacterAttributes.Classes)lastValue;

            foreach (CharacterAttributes.Classes thisClass in System.Enum.GetValues(typeof(CharacterAttributes.Classes)))
            {
                if (mPlayerClass == thisClass)
                {
                    mPlayerClass = lastClass;
                    break;
                }

                lastClass = thisClass;
            }
        }

        CheckClass();
    }

    public void ChangeRace(bool goNext)
    {
        bool foundIt = false;

        if (goNext)
        {
            foreach (CharacterAttributes.Races thisRace in System.Enum.GetValues(typeof(CharacterAttributes.Races)))
            {
                if (foundIt)
                {
                    foundIt = false;
                    mPlayerRace = thisRace;
                    break;
                }
                else if (mPlayerRace == thisRace)
                {
                    foundIt = true;
                }
            }

            if (foundIt)
            {
                mPlayerRace = 0;
            }
        }
        else
        {
            int lastValue = System.Enum.GetValues(typeof(CharacterAttributes.Races)).Length - 1;

            CharacterAttributes.Races lastRace = (CharacterAttributes.Races)lastValue;

            foreach (CharacterAttributes.Races thisRace in System.Enum.GetValues(typeof(CharacterAttributes.Races)))
            {
                if (mPlayerRace == thisRace)
                {
                    mPlayerRace = lastRace;
                    break;
                }

                lastRace = thisRace;
            }
        }

        CheckRace();
    }

    public void InitCharacterCreator()
    {
        //set initial attribute value
        InitializeAttributes();

        //set inital race
        mPlayerRace = CharacterAttributes.Races.Human;

        //set intial class
        mPlayerClass = CharacterAttributes.Classes.Cleric;

        CheckRace();
        CheckClass();
    }

    #endregion
    #region Private Functions/Methods

    private void InitializeAttributes()
    {
        mPlayerAttributes = new Dictionary<CharacterAttributes.BaseAttributes, int>();

        foreach (CharacterAttributes.BaseAttributes thisAttrib in System.Enum.GetValues(typeof(CharacterAttributes.BaseAttributes)))
        {
            if (mPlayerAttributes.ContainsKey(thisAttrib))
            {
                mPlayerAttributes[thisAttrib] = mBaseValue;
            }
            else
            {
                mPlayerAttributes.Add(thisAttrib, mBaseValue);
            }
        }
    }

    #endregion
    #region Buttons

    public void NextRaceClicked()
    {
        bool foundIt = false;

        foreach (CharacterAttributes.Races thisRace in System.Enum.GetValues(typeof(CharacterAttributes.Races)))
        {
            if (foundIt)
            {
                //reset the race
                mPlayerRace = thisRace;
                foundIt = false;
                break;
            }
            else if (mPlayerRace == thisRace)
            {
                foundIt = true;
            }
        }

        if (foundIt)
        {
            mPlayerRace = 0;
        }

        CheckRace();

    }

    public void NextPortraitClicked()
    {
        int maxIndex = mPortraitList.Count;
        int currentIndex = mPortraitList.IndexOf(mPlayerPortrait);
        currentIndex++;

        if (currentIndex == maxIndex)
        {
            currentIndex = 0;
        }

        mPlayerPortrait = mPortraitList[currentIndex];
        portrait.sprite = mPlayerPortrait;
    }

    public void PrevPortraitClicked()
    {
        int maxIndex = mPortraitList.Count;
        int currentIndex = mPortraitList.IndexOf(mPlayerPortrait);
        currentIndex--;

        if (currentIndex == -1)
        {
            currentIndex = maxIndex - 1;
        }

        mPlayerPortrait = mPortraitList[currentIndex];
        portrait.sprite = mPlayerPortrait;
    }

    public void SaveCharacterButtonPressed()
    {
        GameManager.instance.playerStats[0].SetCharacterName(nameField.text);
        GameManager.instance.playerStats[0].SetSprite(mPlayerPortrait);
        GameManager.instance.playerStats[0].SetStrength(mPlayerAttributes[CharacterAttributes.BaseAttributes.Strength]);
        GameManager.instance.playerStats[0].SetCurrentHP(mPlayerHP);
        GameManager.instance.playerStats[0].SetMaxHP(mPlayerHP);
        GameManager.instance.playerStats[0].SetCurrentMP(mPlayerMP);
        GameManager.instance.playerStats[0].SetMaxMP(mPlayerMP);
        GameManager.instance.playerStats[0].SetLevel(1);
        GameManager.instance.playerStats[0].SetBattleCharacter(Resources.Load<BattleChar>("Prefabs/Players/PlayerOptions/" + mPlayerClass + "/" + mPlayerSex + mPlayerRace));
        GameManager.instance.playerStats[0].GetBattleCharacter.SetIsPlayer(true);
        GameManager.instance.playerStats[0].SetClass(mPlayerClass.ToString());
        GameManager.instance.playerStats[0].SetSex(mPlayerSex);
        GameManager.instance.playerStats[0].SetRace(mPlayerRace.ToString());

        Debug.Log("Prefabs/Players/PlayerOptions/" + mPlayerClass + "/" + mPlayerSex + mPlayerRace);
        SceneManager.LoadScene("Calimere");
    }

    #endregion
}