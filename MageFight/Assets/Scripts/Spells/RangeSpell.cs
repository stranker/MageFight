﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSpell : Spell {

    protected Vector2 velocity;
    protected Rigidbody2D rd;

    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        rd.velocity = travelVelocity * dir;
    }

    public override void InvokeSpell(Vector3 direction)
    {
        if(!invoked)
        {
            dir = direction;
            invoked = !invoked;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void Update()
    {
        if (invoked)
        {
            if (timer < lifeTime)
                timer += Time.deltaTime;
            else
                Destroy(gameObject);
        }
    }

}