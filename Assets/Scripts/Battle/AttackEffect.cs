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

    // The Proper way to instantiate variables to be edited in the editor
    // without exposing them outside of this script
    // [SerializeField] private int soundEffect;
    // [SerializeField] private float effectLength;
    public int soundEffect;
    public float effectLength;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

    void Start ()
    {
        AudioManager.instance.PlaySFX(soundEffect);
	}

    #endregion
    #region Implementation Functions/Methods

    void Update ()
    {
        Destroy(gameObject, effectLength);
	}

    #endregion
}