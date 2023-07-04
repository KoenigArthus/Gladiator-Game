using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class CardAnimationsEnemy : MonoBehaviour
{
    public Animator animator;
    public Player player;
    public Enemy enemy;
    public Player Player => player;

    public GameObject enemygladius;
    public GameObject enemytrident;
    public GameObject enemyscutum;
    public GameObject enemypugiu;
    public GameObject enemydoru;
    public GameObject enemyparmula;
    public GameObject enemyscindo;
    public GameObject enemyrete;
    public GameObject enemycassis;
    public GameObject enemygalerus;
    public GameObject enemyocrea;

    public GameObject enemygladiusleft;
    public GameObject enemytridentleft;
    public GameObject enemyscutumleft;
    public GameObject enemypugiuleft;
    public GameObject enemydoruleft;
    public GameObject enemyparmulaleft;
    public GameObject enemyscindoleft;
    public GameObject enemyreteleft;

    public CardSet enemyhead;
    public CardSet enemyshoulder;
    public CardSet enemyleg;
    public CardSet enemyleftHand;
    public CardSet enemyrightHand;

    public EnemyType enemyType;
    private Focus focus;


    public void Awake()
    {
        focus = FindObjectOfType<Focus>();

        if (enemyType == EnemyType.Tutorialgladiator)
        {
            enemyparmulaleft.SetActive(true);
            enemygladius.SetActive(true);
            animator.SetBool("EWalkingSwordRightandShieldLeft", true);
        }



    }   
        

    public void PlayTutorialGladiatorAnimation()
    {
            focus.DecideFocus("enemyattack");
            animator.SetTrigger("ESwordAttack"); 
    }

}

