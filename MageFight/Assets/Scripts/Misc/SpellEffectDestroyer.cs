using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectDestroyer : MonoBehaviour {

    [SerializeField] private String effectName;
    private float timer;
    public float destroyTime;
    public bool isActivated = false;
    [SerializeField] private Animator anim;

    private void Start()
    {
        if (destroyTime != 0)
        {
            timer = destroyTime;
            isActivated = true;
        }
        anim.SetTrigger(effectName);
    }

    // Update is called once per frame
    void Update () {
        if (isActivated)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isActivated = !isActivated;
                Destroy(gameObject);
            }
            var parent = transform.parent.GetComponent<WizardBehavior>();
            if (parent)
            {
                if (!parent.isAlive)
                {
                    Destroy(this.gameObject);

                }
            }
        }
	}

    public void SetTimer(float time)
    {
        timer = time;
        isActivated = !isActivated;
    }
}
