using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickParticles : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _system;
    public Color colorParticles;

    internal void PlayParticles(PlayerBehavior playerBehavior)
    {
        ParticleSystem.MainModule main = _system.main;
        main.startColor = playerBehavior.playerColor;
        _system.Play();
    }
}
