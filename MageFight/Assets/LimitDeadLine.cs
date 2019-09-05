using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitDeadLine : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerBehavior>().TakeDamage(9999, Vector2.zero);
        }
    }

}
