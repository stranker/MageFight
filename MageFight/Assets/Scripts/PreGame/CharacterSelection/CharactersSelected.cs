using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSelected : MonoBehaviour
{

    private static CharactersSelected instance;
    public static CharactersSelected Instance
    {
        get
        {
            instance = FindObjectOfType<CharactersSelected>();
            if (instance == null)
            {
                GameObject go = new GameObject("CharactersSelected");
                instance = go.AddComponent<CharactersSelected>();
            }
            return instance;

        }
    }

    public List<Player> playersConfirmed = new List<Player>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetPlayersConfirmed(List<Player> players)
    {
        playersConfirmed = players;
    }

}
