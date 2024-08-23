/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AttackEffect.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Plays a sound effect
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    [SerializeField] private int soundEffect = 0;
    [SerializeField] private float effectLength = 0.0f;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

    private void Start ()
    {
        AudioManager.instance.PlaySFX(soundEffect);
	}

    #endregion
    #region Implementation Functions/Methods

    private void Update ()
    {
        Destroy(gameObject, effectLength);
	}

    #endregion
}