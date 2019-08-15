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
    public RaycastHit2D onFloor;
    public int currentDirection = 1;
    public bool canJump = true;
    public bool jumping;
    public bool canFly = true;
    public bool flying;
    public Vector2 velocity;
    public Vector2 flyDirection;
    public Vector2 attackDirection;
    public InputManager input;
    public AttackBehavior attackBehavior;
    public Rigidbody2D rigidBody;
    public Transform feet;
    public LayerMask floorLayer;
    public Transform visual;

    // Use this for initialization
    void Start () {
        initialGravityScale = rigidBody.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
	}

    private void FixedUpdate()
    {
        GeneralMovement();
    }

    private void GeneralMovement()
    {
        CheckCanMove();
        if (canMove)
        {
            rigidBody.velocity = velocity;
            rigidBody.gravityScale = flying || attackBehavior.invoking ? 0 : initialGravityScale;
        }
        else
        {
            velocity = Vector2.zero;
            rigidBody.velocity = velocity;
            rigidBody.gravityScale = 0;
        }
    }

    private void CheckCanMove()
    {
        canMove = !attackBehavior.invoking;
    }

    private void GetInput()
    {
        onFloor = Physics2D.Raycast(feet.transform.position, Vector2.down, 1.3f, floorLayer);
        velocity.x = (flying ? flyDirection.normalized.x * flyAccelerationIncrement * flySpeed : (GetKeyboardXAxis() + GetDPadXAxis()) * floorSpeed) * Time.deltaTime;
        velocity.y = flying ? flyDirection.normalized.y * flyAccelerationIncrement * flySpeed * Time.deltaTime : rigidBody.velocity.y;
        jumping = Input.GetButtonDown(input.jumpButton) && onFloor;
        flying = GetFlyInput() && canFly;
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
        FacingDirectionCheck();
        JumpCheck();
        GetAimDirection();
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
            velocity.y = jumpSpeed;
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
}
