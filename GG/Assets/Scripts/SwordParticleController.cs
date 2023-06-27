using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordParticleController : MonoBehaviour
{
    public ParticleSystem swordParticleSystem;

    public void Awake()
    {
        swordParticleSystem.Stop();
    }
    public void PlayswordParticleSystem()
    {
        swordParticleSystem.Play();
    }
}

