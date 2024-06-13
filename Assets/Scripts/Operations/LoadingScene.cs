using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games

public class LoadingScene : MonoBehaviour {
    public float waitToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
            waitToLoad = 0;
                GameManager.instance.LoadData();
                QuestManager.instance.LoadQuestData();
                SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));

            }
        
	}
}
