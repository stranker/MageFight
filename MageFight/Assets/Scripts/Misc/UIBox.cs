using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBox : MonoBehaviour {

    public UIManager ui;
    private List<GameObject> players = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            players.Add(collision.gameObject);
            ui.Fade(0.3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            players.Remove(collision.gameObject);
            if (players.Count <= 0)
            {
                ui.Fade(1f);
            }
        }
    }

}
