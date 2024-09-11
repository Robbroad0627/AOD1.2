/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: DialogManager.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    private static DialogManager mInstance;

    private void Singleton()
    {
        if (mInstance != null && mInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static DialogManager Access => mInstance;

    #endregion

    //VARIABLES
    #region Constant Variable Declarations and Initializations

    private const string NPC = "n-";
    private const string PLAYER = "Player";
    private const string INTERACT = "Fire1";

    #endregion
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Text dialogText = null;
    [SerializeField] private Text nameText = null;
    [SerializeField] private GameObject dialogBox = null;
    [SerializeField] private GameObject nameBox = null;
    [Header("Confirmation")]
    [SerializeField] private GameObject promptArea = null;
    [SerializeField] private Text yesButtonText = null;
    [SerializeField] private Text noButtonText = null;

    #endregion
    #region Private Variables

    private int mCurrentLine;
    private bool mJustStarted;
    private bool mShouldMarkQuest;
    private bool mMarkQuestComplete;
    private bool mAdvanceDialogOnClick;
    private string mQuestToMark;
    private string[] mDialogLines;
    private Action mNoAction;
    private Action mYesAction;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public GameObject GetDialogBox => dialogBox;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Awake() => Singleton();
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void Start()
    {
        mCurrentLine = 0;
        mQuestToMark = "";
        mDialogLines = null;
        mJustStarted = false;
        mShouldMarkQuest = false;
        mMarkQuestComplete = false;
        mAdvanceDialogOnClick = true;
    }
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update ()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp(INTERACT))
            {
                if (!mJustStarted)
                {
                    if (mAdvanceDialogOnClick)
                    {
                        AdvanceDialog();
                    }
                }
                else
                {
                    mJustStarted = false;
                } 
            }
        }
	}
#pragma warning restore IDE0051

    #endregion
    #region Private Functions/Methods

    private void AdvanceDialog()
    {
        mCurrentLine++;

        if (mCurrentLine >= mDialogLines.Length)
        {
            dialogBox.SetActive(false);

            GameManager.Access.SetDialogActive(false);

            if (mShouldMarkQuest)
            {
                mShouldMarkQuest = false;

                if (mMarkQuestComplete)
                {
                    QuestManager.instance.MarkQuestComplete(mQuestToMark);
                }
                else
                {
                    QuestManager.instance.MarkQuestIncomplete(mQuestToMark);
                }
            }
        }
        else
        {
            CheckIfName();

            dialogText.text = mDialogLines[mCurrentLine];
        }
    }

    private void CheckIfName()
    {
        if (mDialogLines[mCurrentLine].StartsWith(NPC))
        {
            nameText.text = mDialogLines[mCurrentLine].Replace(NPC, "");
            nameText.text = nameText.text == PLAYER ? GameManager.Access.GetPlayerName : nameText.text;
            mCurrentLine++;
        }
    }

    private void PromptHelper()
    {
        dialogBox.SetActive(true);
        promptArea.SetActive(true);
    }

    private void DismissDialog()
    {
        dialogBox.SetActive(false);
        promptArea.SetActive(false);
        mAdvanceDialogOnClick = true;
    }

    #endregion
    #region Public Functions/Methods

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        mDialogLines = newLines;
        mCurrentLine = 0;

        CheckIfName();

        dialogText.text = mDialogLines[mCurrentLine];
        dialogBox.SetActive(true);
        mJustStarted = true;
        nameBox.SetActive(isPerson);
        GameManager.Access.SetDialogActive(true);
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        mQuestToMark = questName;
        mMarkQuestComplete = markComplete;
        mShouldMarkQuest = true;
    }

    public void Prompt(string message, Action yesAction, Action noAction,string yesString = "Yes",string noString = "No",string title= "Confirm?")
    {
        mAdvanceDialogOnClick = false;
        dialogText.text = message;
        nameText.text = title;
        yesButtonText.text = yesString;
        noButtonText.text = noString;
        mYesAction = yesAction;
        mNoAction = noAction;

        //Note:This forces the prompt into the next frame, if you don't do this then you can't chain prompts.
        Invoke("PromptHelper", 0.01f);
    }
    
    // WHY IS THIS FUNCTION DOUBLED AND EXACTLY THE SAME AS THE Prompt FUNCTION? NO NEED FOR THIS JUST USE Prompt
    public void PortPrompt(string message, Action yesAction, Action noAction, string yesString, string noString, string title = "Boat Captian")
    {
        mAdvanceDialogOnClick = false;
        dialogText.text = message;
        nameText.text = title;
        yesButtonText.text = yesString;
        noButtonText.text = noString;
        mYesAction = yesAction;
        mNoAction = noAction;

        //Note:This forces the prompt into the next frame, if you don't do this then you can't chain prompts.
        Invoke("PromptHelper", 0.01f);
    }

    #endregion
    #region Actions

    public void ExecuteNoAction()
    {
        BoatCaptain captian = FindObjectOfType<BoatCaptain>();
        captian.boatTripConfirmed = false;
        mNoAction?.Invoke();
        promptArea.SetActive(false);
        DismissDialog();
    }

    public void ExecuteYesAction()
    {
        mYesAction?.Invoke();
        promptArea.SetActive(false);
        DismissDialog();
    }

    #endregion
}