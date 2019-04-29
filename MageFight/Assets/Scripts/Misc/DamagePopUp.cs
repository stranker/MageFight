using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp : MonoBehaviour {

    public TextMesh tm;

    private void Start()
    {
        tm = GetComponentInChildren<TextMesh>();
    }

    public void SetDamage(int damage)
    {
        tm.text = damage.ToString();
    }

    public void EndAnimation()
    {
        Destroy(gameObject);
    }

}
