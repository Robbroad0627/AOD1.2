using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Bonehead Games

public class UIHandler : MonoBehaviour
{
    CharacterCreator myCharCreator;

    public Text raceText;
    public Text classText;
    public Text strValue;
    public Text dexValue;
    public Text conValue;
    public Text intValue;
    public Text wisValue;
    public Text chaValue;
    public Text hpValue;
    public Text mpValue;
    public Text acValue;
    public Text sexText;



    // Start is called before the first frame update
    void Start()
    {
        myCharCreator = GetComponent<CharacterCreator>();
        myCharCreator.InitCharacterCreator();

        UpdateUI();
    }

    void UpdateUI()
    {
        raceText.text = myCharCreator.GetPlayerRace.ToString();
        classText.text = myCharCreator.GetPlayerClass.ToString();
        myCharCreator.CheckRace();
        myCharCreator.CheckClass();
        if (myCharCreator.GetIsMale)
            sexText.text = "Male";
        else
            sexText.text = "Female";

    
       

        strValue.text = myCharCreator.GetPlayerAttributes[CharacterAttributes.BaseAttributes.Strength].ToString();
        dexValue.text = myCharCreator.GetPlayerAttributes[CharacterAttributes.BaseAttributes.Dexterity].ToString();
        conValue.text = myCharCreator.GetPlayerAttributes[CharacterAttributes.BaseAttributes.Constitution].ToString();
        intValue.text = myCharCreator.GetPlayerAttributes[CharacterAttributes.BaseAttributes.Intelligence].ToString();
        wisValue.text = myCharCreator.GetPlayerAttributes[CharacterAttributes.BaseAttributes.Wisdom].ToString();
        chaValue.text = myCharCreator.GetPlayerAttributes[CharacterAttributes.BaseAttributes.Charisma].ToString();
        hpValue.text = myCharCreator.GetPlayerHP.ToString();
        mpValue.text = myCharCreator.GetPlayerMP.ToString();


    }

    public void NextRaceClicked()
    {
        myCharCreator.ChangeRace(true);
        UpdateUI();
    }

    public void PrevRaceClicked()
    {
        myCharCreator.ChangeRace(false);
        UpdateUI();
    }

   public void NextClassClicked()
    {
        myCharCreator.ChangeClass(true);
        UpdateUI();
    }

    public void PrevClassClicked()
    {
        myCharCreator.ChangeClass(false);
        UpdateUI();
    }

    public void SexButtonClicked()
    {
        myCharCreator.SetIsMale(!myCharCreator.GetIsMale);
        myCharCreator.CheckRace();
        UpdateUI();
    }
}
