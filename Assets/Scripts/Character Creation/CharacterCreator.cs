using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Bonehead Games

public class CharacterCreator : MonoBehaviour
{
    public Text raceText, classText, sexText;
    public Image portrait;
    public InputField nameField;

    public bool isMale = true;

    public List<Sprite> portraitList;
    public Sprite myPortrait;

    public Dictionary<CharacterAttributes.BaseAttributes, int> myAttributes;

    int baseValue = 10;

    public CharacterAttributes.Races myRace;
    public CharacterAttributes.Classes myClass;

    private int numAttributes = 6;

    public int myHP;
    public int myMP;

    CharacterAttributes.Races oldRace;
    CharacterAttributes.Classes oldClass;

    private void Start()
    {
        nameField = GameObject.Find("nameText").GetComponent<InputField>();
    }
    void InitializeAttributes()
    {
        myAttributes = new Dictionary<CharacterAttributes.BaseAttributes, int>();
        foreach (CharacterAttributes.BaseAttributes thisAttrib in System.Enum.GetValues(typeof(CharacterAttributes.BaseAttributes)))
        {
            if (myAttributes.ContainsKey(thisAttrib))
                myAttributes[thisAttrib] = baseValue;
            else

                myAttributes.Add(thisAttrib, baseValue);

        }

    }


    // Use this for initialization
    public void InitCharacterCreator()
    {

        //set initial attribute value
        InitializeAttributes();

        //set inital race
        myRace = CharacterAttributes.Races.Human;

        //set intial class
        myClass = CharacterAttributes.Classes.Cleric;


        CheckRace();
        CheckClass();



    }


    public void CheckRace()
    {
        InitializeAttributes();
        switch (myRace)
        {
            case CharacterAttributes.Races.Dwarf:
                myAttributes[CharacterAttributes.BaseAttributes.Constitution] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Strength] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 1;
                portraitList = GetComponent<PortraitHandler>().mDwarf;
                if (!isMale)
                    portraitList = GetComponent<PortraitHandler>().fDwarf;
                break;
            case CharacterAttributes.Races.Elf:
                myAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Intelligence] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Constitution] -= 1;
                portraitList = GetComponent<PortraitHandler>().mElf;
                if (!isMale)
                    portraitList = GetComponent<PortraitHandler>().fElf;
                break;
            case CharacterAttributes.Races.Half_Elf:
                myAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Wisdom] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 1;
                portraitList = GetComponent<PortraitHandler>().mHalf_Elf;
                if (!isMale)
                    portraitList = GetComponent<PortraitHandler>().fHalf_Elf;
                break;
            case CharacterAttributes.Races.Halfling:
                myAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Wisdom] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Strength] -= 1;
                portraitList = GetComponent<PortraitHandler>().mHalfling;
                if (!isMale)
                    portraitList = GetComponent<PortraitHandler>().fHalfling;
                break;
            case CharacterAttributes.Races.Half_Orc:
                myAttributes[CharacterAttributes.BaseAttributes.Constitution] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Strength] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 2;
                portraitList = GetComponent<PortraitHandler>().mHalf_Orc;
                if (!isMale)
                    portraitList = GetComponent<PortraitHandler>().fHalf_Orc;
                break;
            case CharacterAttributes.Races.Human:
                int ran = Random.Range(0, System.Enum.GetValues(typeof(CharacterAttributes.BaseAttributes)).Length);
                myAttributes[(CharacterAttributes.BaseAttributes)ran] += 2;
                portraitList = GetComponent<PortraitHandler>().mHuman;
                if (!isMale)
                    portraitList = GetComponent<PortraitHandler>().fHuman;
                break;

            default:
                Debug.Log("ERROR - Race Not Found");
                break;
        }
        myPortrait = portraitList[0];
        portrait.sprite = myPortrait;

    }

    public void CheckClass()
    {
        myHP = baseValue;
        myMP = 0;
        switch (myClass)
        {
            case CharacterAttributes.Classes.Cleric:
                myHP = 8 + myAttributes[CharacterAttributes.BaseAttributes.Constitution];
                myMP = 15 + myAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                myPortrait = portraitList[0];
                portrait.sprite = myPortrait;
                break;

            case CharacterAttributes.Classes.Druid:
                myHP = 8 + myAttributes[CharacterAttributes.BaseAttributes.Constitution];
                myMP = 15 + myAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                myPortrait = portraitList[1];
                portrait.sprite = myPortrait;
                break;

            case CharacterAttributes.Classes.Fighter:
                myHP = 10 + myAttributes[CharacterAttributes.BaseAttributes.Constitution];
                myPortrait = portraitList[2];
                portrait.sprite = myPortrait;
                break;

            case CharacterAttributes.Classes.Magic_User:
                myHP = 6 + myAttributes[CharacterAttributes.BaseAttributes.Constitution];
                myMP = 20 + myAttributes[CharacterAttributes.BaseAttributes.Intelligence];
                myPortrait = portraitList[3];
                portrait.sprite = myPortrait;
                break;

            case CharacterAttributes.Classes.Paladin:
                myHP = 10 + myAttributes[CharacterAttributes.BaseAttributes.Constitution];
                myMP = 10 + myAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                myPortrait = portraitList[4];
                portrait.sprite = myPortrait;
                break;

            case CharacterAttributes.Classes.Ranger:
                myHP = 10 + myAttributes[CharacterAttributes.BaseAttributes.Constitution];
                myMP = 15 + myAttributes[CharacterAttributes.BaseAttributes.Wisdom];
                myPortrait = portraitList[5];
                portrait.sprite = myPortrait;
                break;

            case CharacterAttributes.Classes.Thief:
                myHP = 6 + myAttributes[CharacterAttributes.BaseAttributes.Constitution];
                myPortrait = portraitList[6];
                portrait.sprite = myPortrait;
                break;
        }
    }


    public void NextRaceClicked()
    {
        bool foundIt = false;

        foreach (CharacterAttributes.Races thisRace in System.Enum.GetValues(typeof(CharacterAttributes.Races)))
        {
            if (foundIt)
            {
                //reset the race
                myRace = thisRace;
                foundIt = false;
                break;
            }
            else if (myRace == thisRace)
                foundIt = true;
        }

        if (foundIt)
            myRace = 0;

        raceText.text = myRace.ToString();
        CheckRace();

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
                    myClass = thisClass;
                    break;
                }
                else if (myClass == thisClass)
                {
                    foundIt = true;
                }
            }
            if (foundIt)
            {
                myClass = 0;
            }
        }
        else
        {
            int lastValue = System.Enum.GetValues(typeof(CharacterAttributes.Classes)).Length - 1;

            CharacterAttributes.Classes lastClass = (CharacterAttributes.Classes)lastValue;

            foreach (CharacterAttributes.Classes thisClass in System.Enum.GetValues(typeof(CharacterAttributes.Classes)))
            {
                if (myClass == thisClass)
                {
                    myClass = lastClass;
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
                    myRace = thisRace;
                    break;
                }
                else if (myRace == thisRace)
                {
                    foundIt = true;
                }
            }
            if (foundIt)
            {
                myRace = 0;

            }
        }
        else
        {
            int lastValue = System.Enum.GetValues(typeof(CharacterAttributes.Races)).Length - 1;

            CharacterAttributes.Races lastRace = (CharacterAttributes.Races)lastValue;

            foreach (CharacterAttributes.Races thisRace in System.Enum.GetValues(typeof(CharacterAttributes.Races)))
            {
                if (myRace == thisRace)
                {
                    myRace = lastRace;
                    break;
                }
                lastRace = thisRace;
            }
        }
        CheckRace();
    }



    public void NextPortraitClicked()
    {
        int maxIndex = portraitList.Count;
        int currentIndex = portraitList.IndexOf(myPortrait);


        currentIndex++;
        if (currentIndex == maxIndex)
        {
            currentIndex = 0;
        }
        myPortrait = portraitList[currentIndex];
        portrait.sprite = myPortrait;
    }

    public void PrevPortraitClicked()
    {
        int maxIndex = portraitList.Count;
        int currentIndex = portraitList.IndexOf(myPortrait);

        currentIndex--;
        if (currentIndex == -1)
        {
            currentIndex = maxIndex - 1;
        }
        myPortrait = portraitList[currentIndex];
        portrait.sprite = myPortrait;
    }

    public void SaveCharacterButtonPressed()
    {
        GameManager.instance.playerStats[0].charName = nameField.text.ToString();
        Debug.Log(nameField.text.ToString());
        GameManager.instance.playerStats[0].charIamge = myPortrait;
        GameManager.instance.playerStats[0].strength = myAttributes[CharacterAttributes.BaseAttributes.Strength];
        GameManager.instance.playerStats[0].currentHP = myHP;
        GameManager.instance.playerStats[0].maxHP = myHP;
        GameManager.instance.playerStats[0].currentMP = myMP;
        GameManager.instance.playerStats[0].maxMP = myMP;
        GameManager.instance.playerStats[0].playerLevel = 1;
        SceneManager.LoadScene("Calimere");
    }

}