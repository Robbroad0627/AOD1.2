/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: QuestDisplay.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 27, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    [SerializeField] private Text[] questTitleText;

    void Start()
    {
        QuestManager.onQuestsUpdated.Add(UpdateQuests);
        UpdateQuests();
    }

    private void OnDestroy()
    {
        QuestManager.onQuestsUpdated.Remove(UpdateQuests);
    }

    public void UpdateQuests()
    {
        string[] questNames = QuestManager.instance.GetActiveButNotCompleteQuestsNames();

        int textIndex = 0;

        foreach ( string quest in questNames)
        {
            if (textIndex >= questTitleText.Length)
            {
                Debug.Log("Out of space to show quest titles,truncating",this);
                break;
            }
            else
            {
                questTitleText[textIndex].text = quest;
                textIndex++;
            }
        }
    }
}