using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class AudioManager : MonoBehaviour {

    public AudioSource[] sfx;
    public AudioSource[] bgm;

    public int bgmCurrentTrack = -1;

    public static AudioManager instance;

   
    void Start () {

        if(null == instance)
        {
            instance = this;
        }

        if(instance != this)
        {
            Destroy(this);
        }
        

        DontDestroyOnLoad(this.gameObject);
	}
	
	
	void Update () {
	}

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
        if (musicToPlay<0 || musicToPlay > bgm.Length)
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
}
