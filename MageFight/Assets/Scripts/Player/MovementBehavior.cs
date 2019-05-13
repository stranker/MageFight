using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour {

    [Header("Movement stats")]
    public float floorSpeed;
    public float airSpeed;
    public float jumpForce;

    public Vector2 velocity;
    private InputManager input;
    private Rigidbody2D rd;
    private bool onFloor = false;
    private bool canMove = true;

    private bool knockback = false;
    public float timer;
    public float knockbackTime = 1;
    private Vector2 aimDirection;
    private bool dashing;
    public float dashSpeed;
    public float dashTimer;
    public float dashTotalTime = 2f;
    private float gravity;

    public ParticleSystem jumpParticles;

    // Use this for initialization
    void Start () {
        rd = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        dashTimer = 0;
        gravity = rd.gravityScale;
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

        if (Input.GetButtonDown(input.jumpButton) && onFloor && !dashing)
        {
            onFloor = !onFloor;
            rd.velocity = new Vector2(0,jumpForce);
            jumpParticles.Play();
        }
        if(Input.GetButtonDown(input.dodgeButton) || dashing)
        {
            canMove = false;
            if(!dashing)
            {
                int upDir = (int)Input.GetAxis(input.aimAxisY);
                int fwDir = Mathf.Abs(Input.GetAxis(input.movementAxisX)) > 0.1f ? (int)transform.localScale.x : 0;
                aimDirection = new Vector2(fwDir == 0 && upDir == 0 ? transform.localScale.x : fwDir, upDir);
            }
            Dash(aimDirection);
        }
    }

    private void Dash(Vector2 dir)
    {
        dashing = true;
        if(dashTimer < dashTotalTime)
        {
            rd.gravityScale = 0;
            rd.velocity = Vector2.zero;
            transform.position += new Vector3(dir.x * dashSpeed, dir.y * dashSpeed);
            dashTimer += Time.deltaTime;
        }
        else
        {
            rd.gravityScale = gravity;
            dashTimer = 0;
            dashing = false;
            canMove = true;
        }
    }

    private void Movement()
    {
        if (!knockback && !dashing)
        {            
            if (onFloor)
                rd.velocity = new Vector2(velocity.x * floorSpeed * Time.deltaTime, rd.velocity.y);
            else
                rd.velocity = new Vector2(velocity.x * airSpeed * Time.deltaTime, rd.velocity.y);
        }

        if (knockback)
        {
            timer += Time.deltaTime;
            if (timer >= knockbackTime)
            {
                timer = 0;
                knockback = !knockback;
            }
        }
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
        if (val)
            rd.bodyType = RigidbodyType2D.Dynamic;
        else
            rd.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onFloor = true;
        }
    }

    public void Push()
    {
        rd.velocity = new Vector2(-transform.localScale.x * 2, 20);
        knockback = !knockback;
    }


}
