using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public enum MOVEMENT_STATES
    {
        IDLE,
        RUN,
        JUMP,
        FLY
    }
    public float floorSpeed;
    public float jumpSpeed;
    public float flySpeed;
    public float initialGravityScale;
    public bool canMove = true;
    public RaycastHit2D onFloor;
    public int currentDirection = 1;
    public bool canJump = true;
    public bool jumping;
    public bool canFly = true;
    public bool flying;
    public MOVEMENT_STATES movementState;
    public Vector2 velocity;
    public Vector2 aimDirection;
    public Vector2 attackDirection;
    public InputManager input;
    public AttackBehavior attackBehavior;
    public Rigidbody2D rigidBody;
    public Transform feet;
    public LayerMask floorLayer;
    public Transform visual;

    // Use this for initialization
    void Start () {
        movementState = MOVEMENT_STATES.IDLE;
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
            rigidBody.velocity = Vector2.zero;
            velocity = Vector2.zero;
        }
    }

    private void CheckCanMove()
    {
        canMove = !attackBehavior.invoking;
    }

    private void GetInput()
    {
        onFloor = Physics2D.Raycast(feet.transform.position, Vector2.down, 1.2f, floorLayer);
        velocity.x = (flying ? aimDirection.normalized.x * flySpeed : (GetKeyboardXAxis() + GetDPadXAxis()) * floorSpeed) * Time.deltaTime;
        velocity.y = flying ? aimDirection.normalized.y * flySpeed * Time.deltaTime : rigidBody.velocity.y;
        jumping = Input.GetButtonDown(input.jumpButton) && onFloor;
        flying = GetFlyInput() && canFly;
        FacingDirectionCheck();
        JumpCheck();
        GetAimDirection();
    }

    private void GetAimDirection()
    {
        int yDir = (int)GetKeyboardYAxis() + (int)GetDPadYAxis();
        int xDir = (int)GetKeyboardXAxis() + (int)GetDPadXAxis();
        aimDirection = new Vector2(currentDirection, yDir);
        attackDirection = new Vector2(xDir, yDir);
    }

    private void JumpCheck()
    {
        if (jumping && canJump)
            velocity.y = jumpSpeed;
    }

    private void FacingDirectionCheck()
    {
        if (GetKeyboardXAxis() + GetDPadXAxis() < 0)
            currentDirection = -1;
        else if (GetKeyboardXAxis() + GetDPadXAxis() > 0)
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
