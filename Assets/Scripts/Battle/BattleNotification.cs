/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: BattleNotification.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour
{

    public float awakeTime;
    public Text theText;

    private float awakeCounter;
	
	void Update ()
    {
		if(awakeCounter > 0)
        {
            awakeCounter -= Time.deltaTime;

            if(awakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

    public void Activate()
    {
        gameObject.SetActive(true);
        awakeCounter = awakeTime;
    }
}
