/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: QuestObjectActivator.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 29, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;
    public string questToCheck;
    public bool activeIfComplete;
    private bool initialCheckDone;

	void Start ()
    {
		if (QuestManager.instance.GetQuestNumber(questToCheck)<0)
        {
            enabled = false;
        }
	}
	
	void Update ()
    {
		if (!initialCheckDone)
        {
            initialCheckDone=CheckCompletion();
        }
	}

    public bool CheckCompletion()
    {
        if (!QuestManager.instance || !QuestManager.instance.isReady)
        {
            return false;
        }

        if (QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }

        return true;
    }
}