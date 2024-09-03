/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: UIFade.cs
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
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;

    public Image fadeScreen;
    public float fadeOutSpeed;
    public float fadeInSpeed;
    public bool shouldFadeToBlack;
    public bool shouldFadeFromBlack;

	void Start ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
    {

        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeOutSpeed * Time.deltaTime));

            if (Mathf.Approximately(fadeScreen.color.a, 1f))
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeInSpeed * Time.deltaTime));

            if (Mathf.Approximately(fadeScreen.color.a, 0f))
            {
                shouldFadeFromBlack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}