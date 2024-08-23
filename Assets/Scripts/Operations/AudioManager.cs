/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: AudioManager.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description: Deals with Audio for the Game
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 23, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //SINGLETON
    #region Singleton

    public static AudioManager instance;

    private void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion

    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    // The Proper way to expose variables to the editor
    // without exposing them outside of this script
    // [SerializeField] private AudioSource[] sfx = null;
    // [SerializeField] private AudioSource[] bgm = null;
    // [SerializeField] private int bgmCurrentTrack = -1;
    public AudioSource[] sfx = null;
    public AudioSource[] bgm = null;
    public int bgmCurrentTrack = -1;

    #endregion

    //FUNCTIONS
    #region Initialization Methods/Functions

    private void Awake()
    {
        Singleton();
    }

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
        bgmCurrentTrack = musicToPlay;

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