using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShuriken : RangeSpell
{
    public float travelTime;
    public float travelTimer = 0f;
    public bool onComeback = false;

    private void Update()
    {
        base.Update();
        if (invoked)
        {
            if (onComeback)
            {
                dir = (mageOwner.transform.position - transform.position).normalized;
                rd.velocity = travelVelocity * dir;
                if (Vector3.Distance(mageOwner.transform.position,transform.position)<1f)
                {
                    Kill();
                }
            }
            else
            {
                if (travelTimer < travelTime)
                {
                    travelTimer += Time.deltaTime;
                }
                else
                {
                    onComeback = true;
                    travelTimer = 0;
                }
            }
        }
        else
        {
            onComeback = false;
        }
    }

    public void SecondAttack()
    {
        onComeback = true;
    }
}
