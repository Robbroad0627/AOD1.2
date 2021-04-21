using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Bonehead Games

public class CharacterCreator : MonoBehaviour
{
    public CharacterAttributes.Races myRace;
    public CharacterAttributes.Classes myClass;
    
    public Image portrait;
    public Text sexText;

    private bool isMale = true;

    public List<Sprite> portraitList;
    public Sprite myPortrait;

    public Dictionary<CharacterAttributes.BaseAttributes, int> myAttributes;
    int baseValue = 10;
    
    public void InitCharacterCreator()
    {
        myRace = CharacterAttributes.Races.Human;
        myClass = CharacterAttributes.Classes.Fighter;

        //set default portrait
        myPortrait = portraitList[0];
        //update update portrait image on UI
        portrait.sprite = myPortrait;

        //update default gender
        sexText.text= "Male";
        if (!isMale)
            sexText.text = "Female";

        
        myAttributes = new Dictionary<CharacterAttributes.BaseAttributes, int>();
        InitializeAttributes();
        CheckRace();
    }
    void InitializeAttributes()
    {
        foreach (CharacterAttributes.BaseAttributes thisAttrib in System.Enum.GetValues(typeof(CharacterAttributes.BaseAttributes)))
        {
            if (myAttributes.ContainsKey(thisAttrib))
                myAttributes[thisAttrib] = baseValue;
        else
            {
                myAttributes.Add(thisAttrib, baseValue);
            }
        }
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
                break;
            case CharacterAttributes.Races.Elf:
                myAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Intelligence] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Constitution] -= 1;
                break;
            case CharacterAttributes.Races.Half_Elf:
                myAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Wisdom] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 1;
                break;
            case CharacterAttributes.Races.Halfling:
                myAttributes[CharacterAttributes.BaseAttributes.Dexterity] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Wisdom] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Strength] -= 1;
                break;
            case CharacterAttributes.Races.Half_Orc:
                myAttributes[CharacterAttributes.BaseAttributes.Constitution] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Strength] += 1;
                myAttributes[CharacterAttributes.BaseAttributes.Charisma] -= 2;
                break;
            case CharacterAttributes.Races.Human:
                int ran = Random.Range(0, System.Enum.GetValues(typeof(CharacterAttributes.BaseAttributes)).Length);
                myAttributes[(CharacterAttributes.BaseAttributes)ran] += 2;
                break;

            default:
                Debug.Log("ERROR - Race Not Found");
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

            CharacterAttributes.Classes lastClass = (CharacterAttributes.Classes) lastValue;

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
            currentIndex = maxIndex -1;
        }
        myPortrait = portraitList[currentIndex];
        portrait.sprite = myPortrait;
    }

    public void SexButtonClicked()
    {
        if (isMale)
        {
            isMale = false;
            sexText.text = "Female";
        }
        else
        {
            isMale = true;
            sexText.text = "Male";
        }
    }
}
