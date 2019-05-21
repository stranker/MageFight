using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectDestroyer : MonoBehaviour {

    private float timer;
    public float time;
    public bool isActivated = false;
	
	// Update is called once per frame
	void Update () {
        if (isActivated)
        {
            if (time != 0 && timer != 0)
            {
                timer = time;
            }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isActivated = !isActivated;
                Destroy(gameObject);
            }
        }
	}

    internal void SetTimer(float time)
    {
        timer = time;
        isActivated = !isActivated;
    }
}
