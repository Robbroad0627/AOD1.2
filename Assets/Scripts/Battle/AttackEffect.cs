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
    // The Proper way to instantiate variables to be edited in the editor
    // without exposing them outside of this script
    // [SerializeField] private variable variableName = initialvalue;
    // Example:
    // [SerializeField] private float health = 10.0f;
    public int soundEffect;
    public float effectLength;

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