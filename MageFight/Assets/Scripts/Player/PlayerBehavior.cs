using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    [Header("Player stats")]
    public int health;
    public int maxHealth;


    private void Start()
    {
    }

    // Update is called once per frame
    void Update () {
	}


    public void TakeDamage(int val)
    {
        health -= val;
    }

}
