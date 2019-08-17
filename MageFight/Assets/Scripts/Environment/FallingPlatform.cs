using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {

    public float fallSpeed;
    private Vector2 velocity;
    public Rigidbody2D rig;

    private void Start()
    {
        velocity.y = -fallSpeed;
    }

    private void FixedUpdate()
    {
        rig.velocity = velocity;
    }

}
