using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBox : MonoBehaviour {

    public UIManager ui;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ui.Fade(0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ui.Fade(1f);
        }
    }

}
