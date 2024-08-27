﻿/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AudioManager.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Deals with Audio for the Game
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    private static AudioManager mInstance;

    private void Singleton()
    {
        if (mInstance != null && mInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static AudioManager Access => mInstance;

    #endregion

    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private AudioSource[] sfx = null;
    [SerializeField] private AudioSource[] bgm = null;

    #endregion
    #region Private Variables

    private int mBGMCurrentTrack;

    #endregion

    //GETTERS/SETTERS
    #region Getters/Accessors

    public int GetCurrentBackgroundMusic => mBGMCurrentTrack;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

#pragma warning disable IDE0051
    private void Awake() => Singleton();
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void Start() => mBGMCurrentTrack = -1;
#pragma warning restore IDE0051

    #endregion
    #region Public Methods/Functions useable outside class

    public void PlaySFX(int soundToPlay)
    {
        if (soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }

    public void PlayBGM(int musicToPlay)
    {
        mBGMCurrentTrack = musicToPlay;

        if ((musicToPlay < 0) || (musicToPlay > bgm.Length))
        {
            Debug.LogWarning("Requested track does not exist, or restoring to state where no BGM, halting BGM.",this);
            StopMusic();
            return;
        }
        
        if (!bgm[musicToPlay].isPlaying)
        {
            StopMusic();

            if (musicToPlay < bgm.Length)
            {
                bgm[musicToPlay].Play();
            }
        }
    }

    public void StopMusic()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    #endregion
}