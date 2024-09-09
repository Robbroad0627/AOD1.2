/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AttackEffect.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Used in all Attack and Spell Prefabs
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 28, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;

public class AttackEffect : MonoBehaviour
{
    //VARIABLES
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    [FormerlySerializedAs("soundEffect")]
    [SerializeField] private int SoundToPlay = 0;
    [FormerlySerializedAs("effectLength")]
    [SerializeField] private float AnimationDuration = 1.75f;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only

    #pragma warning disable IDE0051
    private void Start () => AudioManager.Access.PlaySoundFX(SoundToPlay);
    #pragma warning restore IDE0051

    #endregion
    #region Private Implementation Functions/Methods used in this Class Only

    #pragma warning disable IDE0051
    private void Update () => Destroy(gameObject, AnimationDuration);
    #pragma warning restore IDE0051

    #endregion
}