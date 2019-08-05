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
    public float quickRechargeStaminaTime = 1f;
    private float staminaTimer = 0f;

    private float changuiTimer = 0f;
    public float changuiTime;
    private bool canJump = false;
    private bool onChangui = false;

    public ParticleSystem jumpParticles;
    public TrailRenderer dashTrail;
    public TrailRenderer selfTrail;

    public SpriteRenderer leftFoot;
    public SpriteRenderer rightFoot;
    public LayerMask floorLayer;
    public float rayCastFloorLenght = 0.25f;

    private float immobilizeTime = 0.5f;
    private float immobilizeTimer = 0;
    private bool immobilized = false;

    // Use this for initialization
    void Start () {
        rd = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        gravity = rd.gravityScale;
		flyStamina = flyMaxStamina;
	}

    private void Update()
    {
        if (immobilized)
        {
            immobilizeTimer += Time.deltaTime;
            rd.velocity = Vector2.zero;
            if (immobilizeTimer >= immobilizeTime)
            {
                immobilizeTimer = 0;
                Immobilize(0, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!immobilized)
        {
            GetInput();
            Movement();
        }
    }

    private void GetInput()
    {
        if(!immobilized)
        {
            if(canMove)
                if(Input.GetAxis(input.AxisXKeyboard) > 0 || Input.GetAxis(input.AxisXKeyboard) < 0)
                    velocity.x = Input.GetAxis(input.AxisXKeyboard);
                else if (Input.GetAxis(input.movementAxisX) > 0 || Input.GetAxis(input.movementAxisX) < 0)
                    velocity.x = Input.GetAxis(input.movementAxisX);
                else if (Input.GetAxis(input.DPadX) > 0 || Input.GetAxis(input.DPadX) < 0)
                    velocity.x = Input.GetAxis(input.DPadX);
                else
                    velocity.x = 0;

            if(Input.GetButtonDown(input.jumpButton) && canJump && !flying)
            {
                rd.velocity = new Vector2(0, jumpForce);
                jumpParticles.Play();
                canJump = !canJump;
            }

            int upDir = 0;
            if(Input.GetAxis(input.AxisYKeyboard) > 0 || Input.GetAxis(input.AxisYKeyboard) < 0)
                upDir = (int)Input.GetAxis(input.AxisYKeyboard);
            else if(Input.GetAxis(input.aimAxisY) > 0 || Input.GetAxis(input.aimAxisY) < 0)
                upDir = (int)Input.GetAxis(input.aimAxisY);
            else if(Input.GetAxis(input.DPadY) > 0 || Input.GetAxis(input.DPadY) < 0)
                upDir = (int)Input.GetAxis(input.DPadY);

            int fwDir = (Mathf.Abs(Input.GetAxis(input.movementAxisX)) > 0.1f || Mathf.Abs(Input.GetAxis(input.DPadX)) > 0 || (Mathf.Abs(Input.GetAxis(input.AxisXKeyboard)) > 0)) ? (int)transform.localScale.x : 0;
            aimDirection = new Vector2(fwDir == 0 && upDir == 0 ? transform.localScale.x : fwDir, upDir);
            dashTrail.emitting = flying;
            flying = ((Input.GetAxis(input.dodgeButton) > 0 )|| Input.GetButton(input.dodgeButtonKeyboard)) && canFly && flyStamina > 0;
            if(flying)
            {
                Fly(aimDirection);
            }
            else
            {
                flying = false;
                canMove = true;
                rd.gravityScale = gravity;
                if(onFloor && flyStamina < flyMaxStamina)
                {
                    canFly = true;
                    if(velocity.x == 0)
                        staminaTimer += Time.deltaTime;
                    else
                        staminaTimer = 0f;
                    if(staminaTimer >= quickRechargeStaminaTime)
                        flyStamina += Time.deltaTime * flyConsumptionStamina * 2.5f;
                    else
                        flyStamina += Time.deltaTime * flyConsumptionStamina * 0.5f;

                    if(flyStamina >= flyMaxStamina)
                        flyStamina = flyMaxStamina;
                }
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
            if (rd.bodyType == RigidbodyType2D.Dynamic)
            {
                if (onFloor)
                    rd.velocity = new Vector2(velocity.x * floorSpeed * Time.deltaTime, rd.velocity.y);
                else
                    rd.velocity = new Vector2(velocity.x * airSpeed * Time.deltaTime, rd.velocity.y);
            }

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

    public void Immobilize(float dur, bool val)
    {
        immobilized = val;
        canMove = !val;
        canFly = !val;
        immobilizeTime = val ? dur : 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && flying)
        {
            rd.gravityScale = gravity;
            flying = false;
            canMove = true && !immobilized;
        }
    }

    public void Knockback(Vector2 pos)
    {
        knockback = !knockback;
        Vector2 knockDirection = ((Vector2)transform.position - pos).normalized;
        knockDirection.x *= 5;
        knockDirection.y = 10;
        rd.velocity = knockDirection;
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
    public void StopRigidbody()
    {
        onFloor = true;
        velocity.x = 0;
        velocity.y = 0;
        flying = false;
        rd.bodyType = RigidbodyType2D.Static;
    }
    public void StartRigidbody()
    {
        rd.bodyType = RigidbodyType2D.Dynamic;
    }

}
