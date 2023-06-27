using System;
using System.Linq;
using UnityEngine;

public class CardAnimations : MonoBehaviour
{
    public Animator animator;
    public Player player;
    public Player Player => player;

    public GameObject gladius;
    public GameObject trident;
    public GameObject scutum;
    public GameObject pugiu;
    public GameObject spartha;
    public GameObject doru;
    public GameObject pilum;
    public GameObject parmula;
    public GameObject scindo;
    public GameObject cestus;
    public GameObject rete;
    public GameObject cassis;
    public GameObject galerus;
    public GameObject ocrea;

    public GameObject gladiusleft;
    public GameObject tridentleft;
    public GameObject scutumleft;
    public GameObject pugiuleft;
    public GameObject sparthaleft;
    public GameObject doruleft;
    public GameObject pilumleft;
    public GameObject parmulaleft;
    public GameObject scindoleft;
    public GameObject cestusleft;
    public GameObject reteleft;

    public CardSet head;
    public CardSet shoulder;
    public CardSet leg;
    public CardSet leftHand;
    public CardSet rightHand;

    public void Awake()
    {
        //load equipped CardSets
        string[] equipped = new string[5];
        UserFile.SaveGame.Equipped.ToArray().CopyTo(equipped, 0);
        head = Enum.TryParse(equipped[0], out CardSet headParsed) ? headParsed : CardSet.None;
        shoulder = Enum.TryParse(equipped[1], out CardSet shoulderParsed) ? shoulderParsed : CardSet.None;
        leg = Enum.TryParse(equipped[2], out CardSet legParsed) ? legParsed : CardSet.None;
        leftHand = Enum.TryParse(equipped[3], out CardSet leftParsed) ? leftParsed : CardSet.None;
        rightHand = Enum.TryParse(equipped[4], out CardSet rightParsed) ? rightParsed : CardSet.None;



        // player = GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("player found");
        }

        if ((leftHand == CardSet.None) && (rightHand == CardSet.None) && (head == CardSet.None) && (shoulder == CardSet.None) && (leg == CardSet.None))
        {
            animator.SetBool("KampfIdle", true);           
        }

        if ((leftHand == CardSet.Gladius) || (rightHand == CardSet.Gladius))
        {
            animator.SetBool("GladiusIdle", true);
            gladius.SetActive (true);
        }

        if ((leftHand == CardSet.Trident) || (rightHand == CardSet.Trident))
        {
            animator.SetBool("TridentIdle", true);
            trident.SetActive(true);
        }

        if ((leftHand == CardSet.Scutum) || (rightHand == CardSet.Scutum))
        {
            animator.SetBool("ScutumIdle", true);
            scutum.SetActive(true);
        }


        if ((leftHand == CardSet.Pugio) || (rightHand == CardSet.Pugio))
        {
            animator.SetBool("PugiuIdle", true);
            pugiu.SetActive(true);
        }


        if ((leftHand == CardSet.Doru) || (rightHand == CardSet.Doru))
        {
            animator.SetBool("DoruIdle", true);
            doru.SetActive(true);
        }


        if ((leftHand == CardSet.Parmula) || (rightHand == CardSet.Parmula))
        {
            animator.SetBool("ParmulaIdle", true);
            parmula.SetActive(true);
        }

        if ((leftHand == CardSet.Scindo) || (rightHand == CardSet.Scindo))
        {
            animator.SetBool("ScindoIdle", true);
            scindo.SetActive(true);
        }


        if ((leftHand == CardSet.Rete) || (rightHand == CardSet.Rete))
        {
            animator.SetBool("ReteIdle", true);
            rete.SetActive(true);
        }

        if ((head == CardSet.Cassis))
        {
            cassis.SetActive(true);
        }

        if ((head == CardSet.Galerus))
        {
            galerus.SetActive(true);
        }

        if ((leg == CardSet.Ocrea))
        {
            ocrea.SetActive(true);
        }
    }
    public void PlayGladiusAnimation()
    {
        animator.SetTrigger("GladiusAttack");
    }

    public void PlayTridentAnimation()
    {
        animator.SetTrigger("TridentAttack");
    }

    public void PlayScutumAnimation()
    {
        animator.SetTrigger("ScutumAttack");
    }

    public void PlayPugiuAnimation()
    {
        animator.SetTrigger("PugiuAttack");
    }

    public void PlaySparthaAnimation()
    {
        animator.SetTrigger("SparthaAttack");
    }

    public void PlayDoruAnimation()
    {
        animator.SetTrigger("DoruAttack");
    }

    public void PlayPilumAnimation()
    {
        animator.SetTrigger("PilumAttack");
    }

    public void PlayParmulaAnimation()
    {
        animator.SetTrigger("ParmulaAttack");
    }


    public void PlayScindoAnimation()
    {
        animator.SetTrigger("ScindoAttack");
    }


    public void PlayCestusAnimation()
    {
        animator.SetTrigger("CestusAttack");
    }


    public void PlayLaqueusAnimation()
    {
        animator.SetTrigger("LaqueusAttack");
    }


    public void PlayReteAnimation()
    {
        animator.SetTrigger("ReteAttack");
    }

    public void PlayParmulaBlockAnimation()
    {
        animator.SetTrigger("ParmulaBlock");
    }

    public void PlayScutumBlockAnimation()
    {
        animator.SetTrigger("ScutumBlock");
    }
}

