using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGamePortal : MonoBehaviour
{
    public int playerCount = 0;
    public List<WizardBehavior> players = new List<WizardBehavior>();
    public TutorialUI tut;

    // Start is called before the first frame update
    void Start()
    {
        playerCount = CharactersSelected.Instance.playersConfirmed.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !players.Contains(collision.gameObject.GetComponent<WizardBehavior>()))
        {
            AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.Player_Collision_Portal.ToString()], this.gameObject);
            players.Add(collision.gameObject.GetComponent<WizardBehavior>());
            collision.gameObject.SetActive(false);
            CheckPlayers();
        }
    }

    public void CheckPlayers()
    {
        if (players.Count >= playerCount)
            tut.AllPlayers();
        else
            tut.PlayerEnterPortal();
    }

}
