using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDisplay : MonoBehaviour
{
    public UnityEngine.UI.Text[] questTitleText;
    // Start is called before the first frame update
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
        if(null == QuestManager.instance)
        {
            Debug.LogWarning("Requested update to quest display but quest manager was not born yet", this);
            return;
        }
        Debug.Log("Quest display updated");
        string[] nl = QuestManager.instance.GetActiveButNotCompleteQuestsNames();

        int textIndex = 0;
        foreach( var s in nl)
        {
            if(textIndex>=questTitleText.Length)
            {
                Debug.Log("Out of space to show quest titles,truncating",this);
                break;
            }
            else
            {
                questTitleText[textIndex].text = s;
                textIndex++;
            }
        }
    }
}
