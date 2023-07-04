using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JSAM;

public class BloodController : MonoBehaviour
{
    private Enemy enemy;
    private Player player;
    public CardGameManager cardGameManager;
    public ParticleSystem enemybloodParticleSystem;
    public ParticleSystem playerbloodParticleSystem;
    public ParticleSystem enemywithblockParticleSystem;
    public ParticleSystem playerwithblockParticleSystem;
    public ParticleSystem ailmentinstantParticleSystem;
    public ParticleSystem playerblockParticleSystem, playerhealParticleSystem, playerskillParticleSystem;
    public ParticleSystem enemyblockParticleSystem, enemyhealParticleSystem, enemyskillParticleSystem;
    public GameObject playerfloorSpawner, playerchestParticleSpawner;
    public GameObject enemyfloorSpawner, enemychestParticleSpawner;

    public void Awake()
    {
        enemybloodParticleSystem.Stop();
    }
    public void PlayEnemyBloodParticleSystem()
    {
        if (cardGameManager.Enemy.BlockStack.Length < 1)
        {
            enemybloodParticleSystem.Play();
            JSAM.AudioManager.PlaySound(Sounds.Hit);
            Debug.Log("Enemy has less than 1 Block");
        }
        else if (cardGameManager.Enemy.BlockStack.Length > 1)
        {
            enemywithblockParticleSystem.Play();
            Debug.Log("Enemy has more than 1 Block");
        }

    }

    public void PlayPlayerBloodParticleSystem()
    {
        if (cardGameManager.Player.Block < 1)
        { 
        playerbloodParticleSystem.Play();
        JSAM.AudioManager.PlaySound(Sounds.Hit);
            Debug.Log("Player has less than 1 Block");
        }
        else if (cardGameManager.Player.Block > 1)
        {
            playerwithblockParticleSystem.Play();
            Debug.Log("Player has more than 1 Block");
        }
    }
}

