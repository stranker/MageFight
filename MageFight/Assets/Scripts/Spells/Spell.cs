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
    public GameObject mageOwner;

    protected bool invoked = false;
    protected Vector3 dir;
    protected float timer = 0f;

    public abstract void InvokeSpell(Vector3 direction, GameObject owner);
    public CastType GetCastType() { return castType; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != mageOwner)
        {
            collision.GetComponent<PlayerBehavior>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
