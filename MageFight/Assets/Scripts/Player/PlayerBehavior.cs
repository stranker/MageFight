using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    [Header("Player stats")]
    public int health;
    public int maxHealth;

    [Header("Player spells")]
    private const int amountOfSpells = 3;
    public GameObject[] inventorySpells = new GameObject[amountOfSpells];
    void OnValidate()
    {
        if (inventorySpells.Length != amountOfSpells)
        {
            Debug.LogWarning("THE MAX AMOUNT OF SPELLS IS 3 (TRI)");
            Array.Resize(ref inventorySpells, amountOfSpells);
        }
    }

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
        if (Input.GetButtonDown(input.firstSkillButton))
        {
            InvokeSpell(inventorySpells[0]);
        }
        if (Input.GetButtonDown(input.secondSkillButton))
        {
            InvokeSpell(inventorySpells[1]);
        }
        if (Input.GetButtonDown(input.thirdSkillButton))
        {
            InvokeSpell(inventorySpells[2]);
        }
    }

    private void InvokeSpell(GameObject gameObject)
    {
        print("piuu piuu");
    }

    public void TakeDamage(int val)
    {
        if (!blocking && health > 0)
        {
            health -= val;
        }
    }

}
