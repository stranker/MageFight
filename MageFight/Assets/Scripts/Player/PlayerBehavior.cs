using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    public int health;
    public int maxHealth;
    private bool blocking;
    private InputManager input;
    private MovementBehavior movement;
    private SpriteRenderer sr;


    private void Start()
    {
        input = GetComponent<InputManager>();
        movement = GetComponent<MovementBehavior>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        GetInput();
	}

    private void GetInput()
    {
        blocking = Input.GetButton(input.blockButton);
        movement.SetCanMove(!blocking);
        if (blocking)
            sr.color = Color.red;
        else
            sr.color = Color.white;
    }

    public void TakeDamage(int val)
    {
        if (!blocking && health > 0)
        {
            health -= val;
        }
    }

}
