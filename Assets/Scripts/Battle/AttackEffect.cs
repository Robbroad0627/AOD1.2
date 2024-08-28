/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AttackEffect.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Used in all Attack and Spell Prefabs
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;

public class AttackEffect : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    [FormerlySerializedAs("soundEffect")]
    [SerializeField] private int SoundToPlay = 0;
    [FormerlySerializedAs("effectLength")]
    [SerializeField] private float AnimationDuration = 1.75f;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Start () => AudioManager.Access.PlaySoundFX(SoundToPlay);
#pragma warning restore IDE0051

    #endregion
    #region Implementation Functions/Methods

#pragma warning disable IDE0051
    private void Update () => Destroy(gameObject, AnimationDuration);
#pragma warning restore IDE0051

    #endregion
}