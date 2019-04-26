using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {

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

    private bool onAttackMode = false;

    private InputManager input;
    private MovementBehavior movement;
    public Transform crosshair;

    // Use this for initialization
    void Start () {
        input = GetComponent<InputManager>();
        movement = GetComponent<MovementBehavior>();
    }

    // Update is called once per frame
    void Update () {
        GetInput();
        if (onAttackMode)
        {
            int upDir = (int)Input.GetAxis(input.aimAxisY);
            int fwDir = (int)Input.GetAxis(input.movementAxisX);
            Vector2 aimDirection = new Vector2(fwDir,upDir);
            float crossAngle = Vector2.SignedAngle(Vector2.right, aimDirection);
            crosshair.eulerAngles = new Vector3(crosshair.eulerAngles.x, transform.localScale.x == 1f ? 0 : 180, transform.localScale.x == 1f ? crossAngle : -crossAngle);
        }
    }

    private void GetInput()
    {
        if (Input.GetButtonDown(input.firstSkillButton))
        {
            InvokeSpell(inventorySpells[0]);
        }
        if (Input.GetButtonUp(input.firstSkillButton))
        {
            ThrowSpell(inventorySpells[0]);
        }
    }

    private void ThrowSpell(GameObject gameObject)
    {
        onAttackMode = !onAttackMode;
        movement.SetCanMove(true);
        print("Throwing spell");
    }

    private void InvokeSpell(GameObject spell)
    {
        onAttackMode = !onAttackMode;
        movement.SetCanMove(false);
        print("Invoking spell");
    }
}
