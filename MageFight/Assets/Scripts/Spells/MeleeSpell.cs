using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell {

    protected Rigidbody2D rd;
    public float lifeTime;
    public float lifeTimer = 0;

    void Start() {
        rd = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (invoked)
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                Kill();
                lifeTimer = 0;
            }
        }
    }

    public override void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner)
    {
        invoked = true;
        mageOwner = owner;
        transform.position = startPos;
        dir = direction;
        rd.velocity = travelVelocity * dir;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        timer = cooldown;
    }
}
