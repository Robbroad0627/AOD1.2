using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Bonehead Games


public class BoatCaptian : MonoBehaviour
{
    public string areaToLoad;
    public string areaTransitionName;
    public int goldCost = 1;

    public float waitToLoad = 1f;

    public PortController portController;

    public bool canOpen;

    private bool shouldLoadAfterFade;
    private bool shouldRunAnimationBeforeFade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove)
        {
            GameManager.instance.ModalPromptBoatTrip(goldCost);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = true;
        }
    }
}
