using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieJarTutorial : MonoBehaviour
{

    public bool goToPortal = false;
    public float speed;
    private Rigidbody2D rigid;
    public GameObject portal;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goToPortal)
        {
            timer += Time.deltaTime;
            rigid.velocity = Vector2.right * (speed * Time.deltaTime + speed * timer); 
            if (Vector2.Distance(transform.position,portal.transform.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            goToPortal = true;
        }
    }

}
