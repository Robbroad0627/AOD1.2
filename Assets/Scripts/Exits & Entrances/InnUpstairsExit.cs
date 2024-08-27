﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InnUpstairsExit :MonoBehaviour
{
    [HideInInspector]
    public string areaToLoad => Inn.GetDownstairsSceneName;
    [HideInInspector]
    public string areaTransitionName => Inn.GetDownstairsTransitionName;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    public bool needBoat = false;

    void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
                Inn.SetIsPlayerUpstairs(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (needBoat && !GameManager.instance.GetHasBoat)
            {
                //Cant use boat area without one.
                Debug.Log("Area needs boat but GameManager.haveBoat == false");
                return;
            }
            //PlayerController.instance.areaTransitionName = Inn.s_downstairsTransitionName;
            this.enabled = true;//Be sure we are enabled or we won't get updates and the next scene will never load.
            shouldLoadAfterFade = true;
            GameManager.instance.SetFadingBetweenAreas(true);

            UIFade.instance.FadeToBlack();

            
        }
    }
}
