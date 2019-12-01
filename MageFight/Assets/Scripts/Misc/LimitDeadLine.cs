using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitDeadLine : MonoBehaviour {

    public ParticleSystem deathParticles;
    public Transform particlesTrans;
    public Transform mapCenter;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<WizardBehavior>().TakeDamage(9999, Vector2.zero, true);
            particlesTrans.position = collision.transform.position;
            Vector2 dir = (mapCenter.position - particlesTrans.position).normalized;
            float particlesAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            particlesTrans.transform.rotation = Quaternion.Euler(0, 0, particlesAngle);
            deathParticles.Play();
            CameraController.Get().CameraShake(1, 0.4f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<WizardBehavior>().TakeDamage(9999, Vector2.zero);
            particlesTrans.position = collision.transform.position;
            Vector2 dir = (mapCenter.position - particlesTrans.position).normalized;
            float particlesAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            particlesTrans.transform.rotation = Quaternion.Euler(0, 0, particlesAngle);
            deathParticles.Play();
            CameraController.Get().CameraShake(1, 0.4f);
        }
    }

}
