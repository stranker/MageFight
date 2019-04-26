using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour {

    [Header("Movement stats")]
    public float floorSpeed;
    public float airSpeed;
    public float jumpForce;

    private Vector2 velocity;
    private InputManager input;
    private Rigidbody2D rd;
    private bool onFloor = false;
    private bool canMove = true;

	// Use this for initialization
	void Start () {
        rd = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
	}
	
    private void FixedUpdate()
    {
        GetInput();
        Movement();
    }

    private void GetInput()
    {
        if (canMove)
            velocity.x = Input.GetAxis(input.movementAxisX);
        else
            velocity.x = 0;

        if (Input.GetButtonDown(input.jumpButton) && onFloor)
        {
            onFloor = !onFloor;
            rd.velocity = new Vector2(0,jumpForce);
        }
    }

    private void Movement()
    {
        if (onFloor)
            rd.velocity = new Vector2(velocity.x * floorSpeed * Time.deltaTime, rd.velocity.y);
        else
            rd.velocity = new Vector2(velocity.x * airSpeed * Time.deltaTime, rd.velocity.y);
        CheckFlipDirection();
    }

    private void CheckFlipDirection()
    {
        if (velocity.x > 0)
            transform.localScale = new Vector2(1, 1);
        else if (velocity.x < 0)
            transform.localScale = new Vector2(-1, 1);
    }

    public void SetCanMove(bool val)
    {
        canMove = val;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onFloor = true;
        }
    }

}
