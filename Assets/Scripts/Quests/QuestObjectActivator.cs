using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class QuestObjectActivator : MonoBehaviour {

    public GameObject objectToActivate;

    public string questToCheck;

    public bool activeIfComplete;

    private bool initialCheckDone;

	// Use this for initialization
	void Start () {
		if(QuestManager.instance.GetQuestNumber(questToCheck)<0)
        {

            //Debug.LogError($"The quest \"{questToCheck}\" is not in the quest database, so it can't be used as activaiton criteria. [Disabling Self]", this);
            enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(!initialCheckDone)
        {
            initialCheckDone=CheckCompletion();
        }
	}


    public bool CheckCompletion()
    {
        if(!QuestManager.instance || !QuestManager.instance.isReady)
        {
            return false;
        }
        if(QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
        return true;
    }
}
