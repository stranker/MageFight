using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour {

    public float floorSpeed;
    public float jumpSpeed;
    public float flySpeed;
    public float flyAcceleration;
    private float flyAccelerationIncrement = 1;
    public float initialGravityScale;
    public bool canMove = true;
    public bool stuned = false;
    public bool onFloor;
    public RaycastHit2D LeftFootRaycast;
    public RaycastHit2D RightFootRaycast;
    public int currentDirection = 1;
    public bool canJump = true;
    public bool canFly = true;
    public bool flying;
    public bool knockback = false;
    public float flyStamina;
    public float flyMaxStamina;
    public float flyConsumption;
    public Vector2 velocity;
    public Vector2 aimDirection;
    public InputManager input;
    public AttackBehavior attackBehavior;
    public WizardBehavior wizardBehavior;
    public Rigidbody2D rigidBody;
    public Transform rightFoot;
    public Transform leftFoot;
    public LayerMask floorLayer;

    public Transform visual;
    public ParticleSystem jumpParticles;
    public ParticleSystem flyParticles;
    private bool onPlayerRestoreHit = false;
    private float playerRestoreOnHitTimer = 0;
    public float playerRestoreOnHitTime;

    private bool spellInvoked = false;
    private float timerSpellInvoked;
    private float timeSpellInvoked = 2f;

    private float timerCanMove;
    private float timeCanMoveException = 0.8f;

    public bool doubleJump = true;

    // Use this for initialization
    void Start () {
        initialGravityScale = rigidBody.gravityScale;
        wizardBehavior = GetComponent<WizardBehavior>();
    }


    // Update is called once per frame
    void Update () {
        if (wizardBehavior.isAlive)
        {
            GetInput();
            if (canMove && !onPlayerRestoreHit)
            {
                GeneralMovement();
            }
            CheckRestoreOnPlayerHit();
            CheckOutbounds();
            TimersCheck();
        }
	}

    private void TimersCheck()
    {
        if (spellInvoked)
        {
            timerSpellInvoked += Time.deltaTime;
            if (timerSpellInvoked >= timeSpellInvoked)
            {
                timerSpellInvoked = 0;
                spellInvoked = false;
                SetCanMove(true);
            }
        }
        if (!canMove)
        {
            timerCanMove += Time.deltaTime;
            if (timerCanMove > timeCanMoveException)
            {
                timerCanMove = 0;
                SetCanMove(true);
            }
        }
    }

    private void CheckOutbounds()
    {
        if (transform.position.y < -50)
        {
            wizardBehavior.TakeDamage(500, Vector2.zero);
        }
    }


    public void SetMotion(int i)
    {
        SetCanMove(i == 1);
    }

    private void CheckRestoreOnPlayerHit()
    {
        if (onPlayerRestoreHit)
        {
            playerRestoreOnHitTimer += Time.deltaTime;
            if (playerRestoreOnHitTimer >= playerRestoreOnHitTime)
            {
                playerRestoreOnHitTimer = 0;
                onPlayerRestoreHit = false;
            }
        }
    }

    private void GeneralMovement()
    {
        RightFootRaycast = Physics2D.Raycast(rightFoot.transform.position, Vector2.down, 1.1f, floorLayer);
        LeftFootRaycast = Physics2D.Raycast(leftFoot.transform.position, Vector2.down, 1.1f, floorLayer);
        if(!onFloor && (RightFootRaycast || LeftFootRaycast))
            AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.Player_On_Ground.ToString()], this.gameObject);
        onFloor = RightFootRaycast || LeftFootRaycast;
        canJump = onFloor;
        doubleJump = !doubleJump ? canJump : true;
        FlyAccelerationCheck();
        FlyStaminaCheck();
        GetAimDirection();
        JumpCheck();
        if (!onPlayerRestoreHit && !stuned)
            rigidBody.velocity = canMove ? velocity : Vector2.zero;
        FacingDirectionCheck();
        rigidBody.gravityScale = flying ? 0 : initialGravityScale;
    }

    public void SpellInvoked(int val)
    {
        spellInvoked = val == 0;
        SetCanMove(false);
    }

    private void GetInput()
    {
        flying = GetFlyInput() && flyStamina > 0 && canFly;
        velocity.x = flying ? aimDirection.normalized.x * flyAccelerationIncrement * flySpeed * Time.deltaTime : GetXAxis() * floorSpeed * Time.deltaTime;
        velocity.y = flying ? aimDirection.normalized.y * flyAccelerationIncrement * flySpeed * Time.deltaTime : rigidBody.velocity.y;
    }

    private void FlyStaminaCheck()
    {
        if (flying)
        {
            flyStamina -= flyConsumption * Time.deltaTime;
            flyParticles.Play();
        }
        else
        {
            flyParticles.Stop();
            if (onFloor)
                flyStamina += flyConsumption * Time.deltaTime * 2;
        }
        flyStamina = Mathf.Clamp(flyStamina, 0, flyMaxStamina);
    }

    private void FlyAccelerationCheck()
    {
        if (flying)
        {
            flyAccelerationIncrement += Time.deltaTime;
        }
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
        int yDir = (int)GetYAxis();
        int xDir = (int)GetXAxis();
        aimDirection = new Vector2(xDir, yDir);
    }

    private void JumpCheck()
    {
        if (GetJumpInput() && onFloor && canJump)
        {
            AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.Player_Jump.ToString()], this.gameObject);
            canJump = false;
            velocity.y = jumpSpeed;
            jumpParticles.Play();
        }
        if (GetJumpInput() && !onFloor && doubleJump)
        {
            velocity.y = jumpSpeed;
            jumpParticles.Play();
            doubleJump = false;
        }
    }

    private void FacingDirectionCheck()
    {
        if (canMove)
        {
            float xDir = GetXAxis();
            if (xDir < -0.1f)
                currentDirection = -1;
            else if (xDir > 0.1f)
                currentDirection = 1;
            visual.localScale = new Vector2(currentDirection, 1);
        }
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
        if (wizardBehavior.isAlive)
        {
            onPlayerRestoreHit = true;
            rigidBody.velocity = dir * new Vector2(knockForce.x, knockForce.y);
        }
    }

    private bool GetJumpInput()
    {
        return Input.GetButtonDown(input.jumpButton);
    }

    private bool GetFlyInput()
    {
        return Input.GetAxis(input.GetFlyButton()) > 0;
    }

    private float GetXAxis()
    {
        return Input.GetAxis(input.GetXAxis());
    }

    private float GetYAxis()
    {
        return Input.GetAxis(input.GetYAxis());
    }

    public void Pull(Vector2 pos)
    {
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
        pos = pos - (Vector2)transform.position;
        rigidBody.velocity = pos;
        canFly = false;
    }

    public void Throw(float force)
    {
        rigidBody.velocity = Vector2.up * force;
    }

    public void SetActive(bool v)
    {
        velocity = Vector2.zero;
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = v ? 6 : 0;
        flyStamina = flyMaxStamina;
        flying = false;
        enabled = v;
    }


    public void CheckFacingToCenter()
    {
        currentDirection = transform.position.x > 0 ? - 1 : 1;
        visual.localScale = new Vector2(currentDirection, 1);
    }

}
