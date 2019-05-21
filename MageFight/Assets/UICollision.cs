using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollision : MonoBehaviour {

    private int playersInside = 0;

    private void Update()
    {
        if (playersInside>0)
        {
            UIManager.Get().Fade(0.4f);
        }
        else
        {
            UIManager.Get().Fade(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInside++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playersInside--;
    }
}
