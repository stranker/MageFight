using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShuriken : RangeSpell
{
    public float travelTime;
    private float travelTimer = 0f;

    private void Update()
    {
        base.Update();
        if (invoked)
        {
            if (travelTimer < travelTime)
            {
                travelTimer += Time.deltaTime;
            }
            else
            {
                rd.velocity = travelVelocity * -dir;
                travelTimer = 0;
            }
        }
    }

}
