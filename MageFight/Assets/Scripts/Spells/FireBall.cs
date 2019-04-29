using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Spell {

    // Update is called once per frame
    void Update () {
        if(invoked)
        {
            if(timer < lifeTime)
                timer += Time.deltaTime;
            else
                Destroy(gameObject);
        }
	}

    public override void InvokeSpell(Vector3 direction)
    {
        if(!invoked)
        {
            dir = direction;
            invoked = true;
            transform.rotation = Quaternion.Euler(dir);
        }
    }
}
