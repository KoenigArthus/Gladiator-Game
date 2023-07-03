using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class BloodController : MonoBehaviour
{
    public ParticleSystem bloodParticleSystem;
    public ParticleSystem blockParticleSystem, healParticleSystem, skillParticleSystem;
    public GameObject floorSpawner, chestParticleSpawner;

    public void Awake()
    {
        bloodParticleSystem.Stop();
    }
    public void PlayBloodParticleSystem()
    {
        bloodParticleSystem.Play();
        JSAM.AudioManager.PlaySound(Sounds.Hit);
    }
}

