using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class AttackEffect : MonoBehaviour {

    public float effectLength;
    public int soundEffect;

	
	void Start () {
        AudioManager.instance.PlaySFX(soundEffect);
	}
	
	
	void Update () {
        Destroy(gameObject, effectLength);
	}
}
