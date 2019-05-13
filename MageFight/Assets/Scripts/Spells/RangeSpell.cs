using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSpell : Spell {

    protected Rigidbody2D rd;

    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
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
