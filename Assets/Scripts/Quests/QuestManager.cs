/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: QuestManager.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 29, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using System;
using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public static HashSet<Action> onQuestsUpdated = new HashSet<Action>();

    public string[] questMarkerNames = new string[0];
    public bool[] questMarkersComplete = new bool[0];

    internal bool isReady;

    public string[] activeQuestNames => questMarkerNames;
    public string[] completedQuestNames
    {
        get
        {
            List<string> l = new List<string>();
            for (int i = 0; i < questMarkersComplete.Length; i++)
            {
                if (questMarkersComplete[i])
                {
                    l.Add(questMarkerNames[i]);
                }
            }

            return l.ToArray();
        }

        set
        {
            if (questMarkerNames == null) 
            {
                questMarkerNames = new string[0];
                Debug.LogWarning("There was no quest marker array so I initialized it to empty");
                return;
            }

            questMarkersComplete = new bool[questMarkerNames.Length];

            foreach (string s in value)
            {
                for (int i = 0; i < questMarkerNames.Length; i++)
                {
                    if (questMarkerNames[i] == s)
                    {
                        questMarkersComplete[i] = true;
                    }
                }
            }
        }
    }

	void Start ()
    {
        instance = this;
        questMarkersComplete = new bool[questMarkerNames.Length];
	}

    public string[] GetActiveQuestsNames()
    {
        List<string> q = new List<string>(questMarkerNames.Length);

        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            if (!questMarkersComplete[i])
            {
                q.Add(questMarkerNames[i]);
            }
        }

        return q.ToArray();
    }

    internal string[] GetActiveButNotCompleteQuestsNames()
    {
        return GetActiveQuestsNames();
    }

    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Q))
        {
#if UNITY_EDITOR
            //debug cheats
            Debug.Log(CheckIfComplete("quest test"));
            MarkQuestComplete("quest test");
            MarkQuestIncomplete("fight the demon");
            UpdateLocalQuestObjects();
#endif
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadQuestData();
        }
	}

    public int GetQuestNumber(string questToFind)
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            if (questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }

        return -1;
    }

    public bool CheckIfComplete(string questToCheck)
    {
        if (GetQuestNumber(questToCheck) >= 0)
        {
            return questMarkersComplete[GetQuestNumber(questToCheck)];
        }

        return false;
    }

    public void MarkQuestComplete(string questToMark)
    {
        int i = GetQuestNumber(questToMark);

        if (i < 0)
        {
            Debug.LogWarning($"Quest \"{questToMark}\" cannot be marked complete because it was not started.");
            return;
        }

        questMarkersComplete[i] = true;
        UpdateLocalQuestObjects();
    }

    public void MarkQuestIncomplete(string questToMark)
    {
        int i = GetQuestNumber(questToMark);

        if (i < 0)
        {
            Debug.LogError($"Quest {questToMark} cannot be marked incomplete. Because it is not registered.");
            return;
        }

        questMarkersComplete[GetQuestNumber(questToMark)] = false;
        UpdateLocalQuestObjects();
    }

    public void UpdateLocalQuestObjects()
    {
        foreach (var q in onQuestsUpdated) { q.Invoke(); }

        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if (questObjects.Length > 0)
        {
            for (int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

    public void SaveQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            if (questMarkersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0);
            }
        }
    }

    public void LoadQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            int valueToSet = 0;

            if (PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            if (valueToSet == 0)
            {
                questMarkersComplete[i] = false;
            }
            else
            {
                questMarkersComplete[i] = true;
            }
        }
    }
}