using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    public ParticleSystem bloodParticleSystem;

    public void Awake()
    {
        bloodParticleSystem.Stop();
    }
    public void PlayBloodParticleSystem()
    {
        bloodParticleSystem.Play();
    }
}

