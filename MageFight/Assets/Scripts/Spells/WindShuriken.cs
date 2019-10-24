using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShuriken : RangeSpell
{
    public float travelTime;
    public float travelTimer = 0f;
    public bool comeback = false;

    private void Update()
    {
        base.Update();
        if (invoked)
        {
            if (!comeback)
            {
                SpellMovement();
                travelTimer += Time.deltaTime;
                if (travelTimer > travelTime)
                {
                    comeback = true;
                }
            }
            else
            {
                ComebackBehavior();
            }
        }
        else
        {
            comeback = false;
            Kill();
            travelTimer = 0;
            StopSpell();
        }
    }

    public void ComebackBehavior()
    {
        dir = (mageOwner.transform.position - transform.position).normalized;
        rd.velocity = speed * dir;
        if (Vector2.Distance(transform.position,mageOwner.transform.position) < 1)
        {
            Kill();
        }
    }

}
