using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellExplosion : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= 2)
        {
            Destroy(this.gameObject);
        }
    }
}
