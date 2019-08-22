using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float floorSpeed;
    public float jumpSpeed;
    public float flySpeed;
    public float flyAcceleration;
    private float flyAccelerationIncrement = 1;
    public float initialGravityScale;
    public bool canMove = true;
    public bool onFloor;
    public RaycastHit2D LeftFootRaycast;
    public RaycastHit2D RightFootRaycast;
    public int currentDirection = 1;
    public bool canJump = true;
    public bool jumping;
    public bool canFly = true;
    public bool flying;
    public bool knockback = false;
    public float flyStamina;
    public float flyMaxStamina;
    public float flyConsumption;
    public Vector2 velocity;
    public Vector2 flyDirection;
    public Vector2 attackDirection;
    public InputManager input;
    public AttackBehavior attackBehavior;
    public Rigidbody2D rigidBody;
    public Transform rightFoot;
    public Transform leftFoot;
    public LayerMask floorLayer;
    public Transform visual;
    public ParticleSystem jumpParticles;
    private float allPurposeTimer;
    private bool onPlayerRestoreHit = false;
    private float playerRestoreOnHitTimer = 0;
    public float playerRestoreOnHitTime;

    // Use this for initialization
    void Start () {
        initialGravityScale = rigidBody.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        CheckRestoreOnPlayerHit();
	}

    public void SetMotion(int i)
    {
        if (i == 0) // No puedo moverme
        {
            SetCanMove(false);
        }
        else if(i == 1)
        {
            SetCanMove(true);
        }
    }

    private void CheckRestoreOnPlayerHit()
    {
        if (onPlayerRestoreHit)
        {
            //playerRestoreOnHitTimer += Time.deltaTime;
            //if (playerRestoreOnHitTimer >= playerRestoreOnHitTime)
            //{
            //    playerRestoreOnHitTimer = 0;
            //    onPlayerRestoreHit = false;
            //    knockback = false;
            //    canMove = !onPlayerRestoreHit;
            //}
        }
    }

    private void FixedUpdate()
    {
        GeneralMovement();
    }

    private void GeneralMovement()
    {
        if (canMove && !IsPlayerInvoking())
        {
            rigidBody.velocity = velocity;
            rigidBody.gravityScale = flying || attackBehavior.invoking ? 0 : initialGravityScale;
        }
    }

    private bool IsPlayerInvoking()
    {
        return attackBehavior.invoking;
    }

    private void GetInput()
    {
        RightFootRaycast = Physics2D.Raycast(rightFoot.transform.position, Vector2.down, 1.1f, floorLayer);
        LeftFootRaycast = Physics2D.Raycast(leftFoot.transform.position, Vector2.down, 1.1f, floorLayer);
        onFloor = RightFootRaycast || LeftFootRaycast;
        velocity.x = (flying ? flyDirection.normalized.x * flyAccelerationIncrement * flySpeed : (GetKeyboardXAxis() + GetDPadXAxis()) * floorSpeed) * Time.deltaTime;
        velocity.y = flying ? flyDirection.normalized.y * flyAccelerationIncrement * flySpeed * Time.deltaTime : rigidBody.velocity.y;
        jumping = Input.GetButtonDown(input.jumpButton) && onFloor;
        flying = GetFlyInput() && flyStamina > 0 && canFly;
        FlyAccelerationCheck();
        FlyStaminaCheck();
        FacingDirectionCheck();
        JumpCheck();
        GetAimDirection();
    }

    private void FlyStaminaCheck()
    {
        if (flying)
        {
            flyStamina -= flyConsumption * Time.deltaTime;
        }
        else
        {
            if (onFloor)
                flyStamina += flyConsumption * Time.deltaTime * 2;
        }
        flyStamina = Mathf.Clamp(flyStamina, 0, flyMaxStamina);
    }

    private void FlyAccelerationCheck()
    {
        if (flying)
            flyAccelerationIncrement += Time.deltaTime;
        else
        {
            if (flyAccelerationIncrement > 0)
                flyAccelerationIncrement -= Time.deltaTime * 5;
            else
                flyAccelerationIncrement = 0;
        }
        flyAccelerationIncrement = Mathf.Clamp(flyAccelerationIncrement, 1, flyAcceleration);
    }

    private void GetAimDirection()
    {
        int yDir = (int)GetKeyboardYAxis() + (int)GetDPadYAxis();
        int xDir = (int)GetKeyboardXAxis() + (int)GetDPadXAxis();
        flyDirection = new Vector2(xDir, yDir);
        attackDirection = new Vector2(currentDirection, yDir);
    }

    private void JumpCheck()
    {
        if (jumping && canJump)
        {
            velocity.y = jumpSpeed;
            jumpParticles.Play();
        }
    }

    private void FacingDirectionCheck()
    {
        float xDir = GetKeyboardXAxis() + GetDPadXAxis();
        if (xDir < -0.1f)
            currentDirection = -1;
        else if (xDir > 0.1f)
            currentDirection = 1;
        visual.localScale = new Vector2(currentDirection, 1);
    }

    public void SetCanMove(bool v)
    {
        canMove = v;
        if (!canMove)
        {
            velocity = Vector2.zero;
            rigidBody.velocity = velocity;
            rigidBody.gravityScale = 0;
        }
    }

    public void SetCanFly(bool v)
    {
        canFly = v;
    }

    public void KnockOut(Vector2 dir, Vector2 knockForce)
    {
        //canMove = false;
        //knockback = true;
        //rigidBody.velocity = dir * new Vector2(knockForce.x, knockForce.y);
        //onPlayerRestoreHit = !canMove;
    }

    public void FallFast(float v)
    {
        canMove = false;
        rigidBody.velocity = new Vector2(0,v);
        onPlayerRestoreHit = !canMove;
    }

    private bool GetFlyInput()
    {
        return Input.GetAxis(input.flyButton) + Input.GetAxis(input.flyButtonKeyboard) > 0;
    }

    private float GetKeyboardXAxis()
    {
        return Input.GetAxis(input.movementAxisX) + Input.GetAxis(input.AxisXKeyboard);
    }

    private float GetDPadXAxis()
    {
        return Input.GetAxis(input.DPadX);
    }

    private float GetKeyboardYAxis()
    {
        return Input.GetAxis(input.aimAxisY) + Input.GetAxis(input.AxisYKeyboard);
    }

    private float GetDPadYAxis()
    {
        return Input.GetAxis(input.DPadY);
    }

    public void Knockback(Vector2 pos)
    {
        //knockback = true;
        allPurposeTimer = 0;
        Vector2 knockDirection = ((Vector2)transform.position - pos).normalized;
        knockDirection.x *= 5;
        knockDirection.y = 10;
        rigidBody.velocity = knockDirection;
    }

    public void Pull(Vector2 pos)
    {
        Debug.Log("Pulled");
        allPurposeTimer = 0;
        if (transform.position.x > pos.x)
        {
            pos.x += 0.5f;
        }
        else
        {
            pos.x -= 0.5f;
        }
        pos = pos - (Vector2)transform.position;
        pos.Normalize();
        pos *= 15;
        rigidBody.velocity = pos;
    }

    public void Drag(Vector2 pos)
    {
        allPurposeTimer = 0;
        pos = pos - (Vector2)transform.position;
        rigidBody.velocity = pos;
    }

    public void Throw(float force)
    {
        allPurposeTimer = 0;
        rigidBody.velocity = Vector2.up * force;
    }

}
