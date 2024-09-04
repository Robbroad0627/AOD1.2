/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: EssentialsLoader.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private GameObject UIScreen = null;
    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject gameMan = null;
    [SerializeField] private GameObject audioMan = null;
    [SerializeField] private GameObject battleMan = null;
    [SerializeField] private GameObject boatMan = null;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    void Start ()
    {
		if (UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        if (PlayerController.Access == null)
        {
            Instantiate(player).GetComponent<PlayerController>();
        }

        if (GameManager.Access == null)
        {
            Instantiate(gameMan).GetComponent<GameManager>();
        }

        if (AudioManager.Access == null)
        {
            Instantiate(audioMan).GetComponent<AudioManager>();
        }

        if (BattleManager.Access == null)
        {
            Instantiate(battleMan).GetComponent<BattleManager>();
        }

        if (Boat.Access == null)
        {
            Instantiate(boatMan).GetComponent<Boat>();
            Boat.Access.gameObject.SetActive(false);
        }
    }
#pragma warning restore IDE0051

    #endregion
}