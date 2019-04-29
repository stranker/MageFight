using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour {

    public enum CastType 
    {
        OneTap,
        Hold
    }
    [Header("Spell Stats")]
    public float travelVelocity;
    public float castVelocity;
    public int damage;
    public float cooldown;
    public float lifeTime;
    public CastType castType;
    private bool activated = false;

    protected bool invoked = false;
    protected Vector3 dir;
    protected float timer = 0f;

    public abstract void InvokeSpell(Vector3 direction);
    public CastType GetCastType() { return castType; }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !activated)
        {
            activated = !activated;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && activated)
        {
            collision.GetComponent<PlayerBehavior>().TakeDamage(damage);
        }
    }

}
