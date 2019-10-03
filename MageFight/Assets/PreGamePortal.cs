using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGamePortal : MonoBehaviour
{
    public int playerCount = 0;
    public List<PlayerBehavior> players = new List<PlayerBehavior>(); 

    // Start is called before the first frame update
    void Start()
    {
        playerCount = CharactersSelected.Instance.playersConfirmed.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !players.Contains(collision.gameObject.GetComponent<PlayerBehavior>()))
        {
            players.Add(collision.gameObject.GetComponent<PlayerBehavior>());
            collision.gameObject.SetActive(false);
            CheckPlayers();
        }
    }

    public void CheckPlayers()
    {
        if (players.Count >= playerCount)
        {
            SceneManager.LoadScene("SampleLevel");
        }
    }

}
