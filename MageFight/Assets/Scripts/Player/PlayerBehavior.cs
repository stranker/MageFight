using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {


    public int health;
    public int maxHealth;
    public bool isAlive = true;
    public Transform headPos;
    public DamagePopUp popText;
    private int PlayerID;

    private void Start(){
        PlayerID = GameManager.Instance.RegisterPlayerID();
        Debug.Log(gameObject + " ID: " + PlayerID);
    }
    public void TakeDamage(int val)
    {
        if (isAlive)
        {
            health -= val;
            GameObject pop = Instantiate(popText.gameObject, headPos.position, Quaternion.identity, transform.parent);
            pop.GetComponent<DamagePopUp>().SetDamage(val);
            if (health <= 0)
            {
                isAlive = !isAlive;
                GetComponent<AttackBehavior>().enabled = false;
                GetComponent<MovementBehavior>().enabled = false;
                GetComponentInChildren<SpriteRenderer>().enabled = false;
                GameManager.Instance.PlayerDeath(PlayerID);
            }
        }
    }

}
