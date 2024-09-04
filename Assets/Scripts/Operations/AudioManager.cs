/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AudioManager.cs
 * Date Created: November 11, 2020
 * Created By: Rob Broad
 * Description: Deals with Audio for the Game
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 28, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton - this Class has One and Only One Instance

    private static AudioManager mInstance;

    private void InitializeSingleton()
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
    #region Private Variables/Fields Exposed to Inspector for Editing

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [FormerlySerializedAs("sfx")]
    [SerializeField] private AudioSource[] SoundFXList = null;
    [FormerlySerializedAs("bgm")]
    [SerializeField] private AudioSource[] MusicList = null;

    #endregion
    #region Private Variables/Fields used in this Class Only

    private int mMusicCurrentTrack;

    #endregion

    //GETTERS/SETTERS
    #region Public Getters/Accessors for use Outside of this Class Only

    public int GetMusicCurrentTrack => mMusicCurrentTrack;

    #endregion

    //FUNCTIONS
    #region Private Initialization Functions/Methods used in this Class Only

#pragma warning disable IDE0051
    private void Awake() => InitializeSingleton();
#pragma warning restore IDE0051

#pragma warning disable IDE0051
    private void Start() => InitializeVariables();
#pragma warning restore IDE0051

    private void InitializeVariables() => mMusicCurrentTrack = -1;

    #endregion
    #region Public Functions/Methods for use Outside of this Class

    public void PlaySoundFX(int soundToPlay)
    {
        if (soundToPlay < SoundFXList.Length)
        {
            SoundFXList[soundToPlay].Play();
        }
    }

    public void PlayMusic(int musicToPlay)
    {
        mMusicCurrentTrack = musicToPlay;

        if ((musicToPlay < 0) || (musicToPlay > MusicList.Length))
        {
            Debug.LogWarning("Requested track does not exist, or restoring to state where no BGM, halting BGM.",this);
            StopMusic();
            return;
        }
        
        if (!MusicList[musicToPlay].isPlaying)
        {
            StopMusic();

            if (musicToPlay < MusicList.Length)
            {
                MusicList[musicToPlay].Play();
            }
        }
    }

    public void StopMusic()
    {
        for (int i = 0; i < MusicList.Length; i++)
        {
            MusicList[i].Stop();
        }
    }

    #endregion
}