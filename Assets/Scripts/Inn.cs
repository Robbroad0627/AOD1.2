using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inn : MonoBehaviour
{
    public bool canOpen;
    public int goldCost = 1;

    public AreaEntrance downstairsEntrance;

    public static string s_downstairsTransitionName;
    public static Vector3? s_downstairsTransitionPosition;
    public static string s_downstairsSceneName;
    public static int s_goldCost;
    public static bool isUpstairs = false;

    const string kUpstairsSceneName ="Inn Upper";

    // Use this for initialization
    void Start () {
		if(null == downstairsEntrance)
        {
            var e = FindObjectOfType<AreaEntrance>();
            if(null == e)
            {
                Debug.LogError("downstairsEntrance not set", this);
            }
            else
            {
                Debug.LogWarning($"downstairsEntrance not set using first available entrance {e.transitionName}", this);
            }
            s_downstairsTransitionName = e.transitionName;
        }
	}
	
	// Update is called once per frame
	void Update () {

        //canOpen = !DialogManager.instance.dialogActive && canOpen;

        if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            s_downstairsTransitionName = downstairsEntrance?.transitionName;
            s_downstairsTransitionPosition = downstairsEntrance?.transform.position;
            s_downstairsSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            s_goldCost = goldCost;
            GameManager.instance.ModalPromptInn(goldCost);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }
    }

    public static void WarpUpstairs()
    {
        GameManager.instance.currentGold -= s_goldCost;
        PlayerController.instance.areaTransitionName = "Inn-Upper";
        SceneManager.LoadScene(kUpstairsSceneName);
        isUpstairs = true;
    }
}
