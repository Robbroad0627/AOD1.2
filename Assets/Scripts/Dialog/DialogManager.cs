using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Bonehead Games

public class DialogManager : MonoBehaviour {

    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;

    [Header("Confirmation")]
    public GameObject promptArea;
    public Text yesButtonText, noButtonText;

    [TextArea]
    public string[] dialogLines;

    public int currentLine;

    private bool advanceDialogOnClick = true;

    private bool justStarted;

    public static DialogManager instance;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;
    
    private Action yesAction;
    private Action noAction;

    // Use this for initialization
    void Start () {
        instance = this;

        //dialogText.text = dialogLines[currentLine];
	}
	
	// Update is called once per frame
	void Update () {
		
        if(dialogBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                if (!justStarted)
                {
                    if(advanceDialogOnClick)
                    AdvanceDialog();
                }
                else
                {
                    justStarted = false;
                }

                
            }
        }

	}

    private void AdvanceDialog()
    {
        currentLine++;

        if (currentLine >= dialogLines.Length)
        {
            dialogBox.SetActive(false);

            GameManager.instance.dialogActive = false;

            if (shouldMarkQuest)
            {
                shouldMarkQuest = false;
                if (markQuestComplete)
                {
                    QuestManager.instance.MarkQuestComplete(questToMark);
                }
                else
                {
                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                }
            }
        }
        else
        {
            CheckIfName();

            dialogText.text = dialogLines[currentLine];
        }
    }

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;

        currentLine = 0;

        CheckIfName();

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;

        nameBox.SetActive(isPerson);

        GameManager.instance.dialogActive = true;
    }

    public void CheckIfName()
    {
        if(dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            nameText.text = nameText.text == "Player" ? GameManager.instance.playerName : nameText.text;
            currentLine++;
        }
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }

    public void Prompt(string message, Action yesAction, Action noAction,string yesString = "Yes",string noString = "No",string title= "Confirm?")
    {
        advanceDialogOnClick = false;
        dialogText.text = message;
        nameText.text = title;
        yesButtonText.text = yesString;
        noButtonText.text = noString;

        this.yesAction = yesAction;
        this.noAction = noAction;

        //Note:This forces the prompt into the next frame, if you don't do this then you can't chain prompts.
        Invoke("PromptHelper", 0.01f);

    }
    
    public void PortPrompt(string message, Action yesAction, Action noAction, string yesString, string noString, string title = "Boat Captian")
    {
        advanceDialogOnClick = false;
        dialogText.text = message;
        nameText.text = title;
        yesButtonText.text = yesString;
        noButtonText.text = noString;

        this.yesAction = yesAction;
        this.noAction = noAction;

        //Note:This forces the prompt into the next frame, if you don't do this then you can't chain prompts.
        Invoke("PromptHelper", 0.01f);
    }

    private void  PromptHelper()
    {
        dialogBox.SetActive(true);
        //GameManager.instance.dialogActive = true;
        promptArea.SetActive(true);
    }

    public void DismissDialog()
    {
        dialogBox.SetActive(false);
        //GameManager.instance.dialogActive = false;
        promptArea.SetActive(false);
        advanceDialogOnClick = true;
    }

    public void ExecuteNoAction()
    {
        var captian = BoatCaptian.FindObjectOfType<BoatCaptian>();
        captian.boatTripConfirmed = false;
        noAction?.Invoke();
        promptArea.SetActive(false);
        DismissDialog();
    }

    public void ExecuteYesAction()
    {
        yesAction?.Invoke();
        promptArea.SetActive(false);
        DismissDialog();
    }
}
