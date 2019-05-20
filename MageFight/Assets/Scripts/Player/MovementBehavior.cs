using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour {

    [Header("Movement stats")]
    public float floorSpeed;
    public float airSpeed;
    public float jumpForce;
	private float gravity;

    public Vector2 velocity;
    private InputManager input;
    public Rigidbody2D rd;
    public bool onFloor = false;
    public bool canMove = true;

    private bool knockback = false;
    public float timer;
    public float knockbackTime = 1;
    private Vector2 aimDirection;

    public bool canFly = true;
    public bool flying;
    public float flySpeed = 100;
    public float flyMaxStamina = 100;
	public float flyStamina = 100;
    public float flyConsumptionStamina = 10;

    private float changuiTimer = 0f;
    public float changuiTime;
    private bool canJump = true;
    private bool onChangui = false;

    public ParticleSystem jumpParticles;
    public TrailRenderer dashTrail;
    public TrailRenderer selfTrail;

    public SpriteRenderer leftFoot;
    public SpriteRenderer rightFoot;
    public LayerMask floorLayer;
    public float rayCastFloorLenght = 0.25f;

    // Use this for initialization
    void Start () {
        rd = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        gravity = rd.gravityScale;
		flyStamina = flyMaxStamina;
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

        if (Input.GetButtonDown(input.jumpButton) && canJump && !flying)
        {
            rd.velocity = new Vector2(0,jumpForce);
            jumpParticles.Play();
            canJump = !canJump;
        }
		int upDir = (int)Input.GetAxis(input.aimAxisY);
        int fwDir = Mathf.Abs(Input.GetAxis(input.movementAxisX)) > 0.1f ? (int)transform.localScale.x : 0;
        aimDirection = new Vector2(fwDir == 0 && upDir == 0 ? transform.localScale.x : fwDir, upDir);
        dashTrail.emitting = flying;
        flying = Input.GetButton(input.dodgeButton) && canFly && flyStamina > 0;
        if (flying)
        {
            Fly(aimDirection);
        }
        else
        {
            flying = false;
            canMove = true;
            rd.gravityScale = gravity;
            if (onFloor && flyStamina < flyMaxStamina)
            {
                canFly = true;
                flyStamina += Time.deltaTime * flyConsumptionStamina;
                if (flyStamina >= flyMaxStamina)
                    flyStamina = flyMaxStamina;
            }
        }
    }

    private void Fly(Vector2 dir)
    {
        flying = true;
        flyStamina -= Time.deltaTime * flyConsumptionStamina;
        rd.gravityScale = 0;
        rd.velocity = new Vector2();
        transform.position += new Vector3(dir.normalized.x * flySpeed, dir.normalized.y * flySpeed) * Time.deltaTime;
    }

    private void Movement()
    {
        GroundControl();
        if (!knockback && !flying)
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
        if (onChangui)
        {
            changuiTimer += Time.deltaTime;
            if (changuiTimer >= changuiTime)
            {
                onChangui = false;
                canJump = false;
                changuiTimer = 0;
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

    public void Immobilize(bool val)
    {
        canMove = val;
        canFly = val;
        if (val)
            rd.bodyType = RigidbodyType2D.Dynamic;
        else
            rd.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && flying)
        {
            rd.gravityScale = gravity;
            flying = false;
            canMove = true;
        }
    }

    public void Knockback()
    {
        knockback = !knockback;
        rd.velocity = new Vector2(-transform.localScale.x * 2, 7);
    }

    public void GroundControl()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(rightFoot.transform.position, Vector2.down, rayCastFloorLenght, floorLayer);
        Debug.DrawLine(leftFoot.transform.position, new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y + -rayCastFloorLenght));

        Debug.DrawLine(rightFoot.transform.position, new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y + -rayCastFloorLenght));
        RaycastHit2D hit2 = Physics2D.Raycast(leftFoot.transform.position, Vector2.down, rayCastFloorLenght, floorLayer);

        if((hit1 || hit2) && !flying)
        {
            onFloor = true;
            canJump = onFloor;
            changuiTimer = 0;
        }
        else
        {
            onFloor = false;
            onChangui = true;

            changuiTimer += Time.deltaTime;
            if(changuiTimer > changuiTime)
            {
                onFloor = false;
            }
        }
    }
}
