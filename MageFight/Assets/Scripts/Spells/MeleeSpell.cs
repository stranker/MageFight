using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell {

    protected Vector2 velocity;
    protected Rigidbody2D rd;
    private float angle;

    void Start() {
        rd = GetComponent<Rigidbody2D>();
        rd.velocity = travelVelocity * dir;
    }

    public override void InvokeSpell(Vector3 direction, GameObject owner)
    {
        mageOwner = owner;
        invoked = !invoked;
        dir = direction;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}
