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
    public GameObject laqueus;
    public GameObject rete;
    public GameObject cassis;
    public GameObject galerus;
    public GameObject ocrea;

    public CardSet leftHand;
    public CardSet rightHand;
    public CardSet head;
    public CardSet shoulder;
    public CardSet leg;

    public void Awake()
    {
        






       // player = GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("player found");
        }

        if ((leftHand == null) && (rightHand == null) && (head == null) && (shoulder == null) && (leg == null))
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
            animator.SetBool("ScutumIdleForBlock", true);
            scutum.SetActive(true);
        }


        if ((leftHand == CardSet.Pugio) || (rightHand == CardSet.Pugio))
        {
            animator.SetBool("PugiuIdle", true);
            pugiu.SetActive(true);
        }

        if ((leftHand == CardSet.Spartha) || (rightHand == CardSet.Spartha))
        {
            animator.SetBool("SparthaIdle", true);
            spartha.SetActive(true);
        }

        if ((leftHand == CardSet.Doru) || (rightHand == CardSet.Doru))
        {
            animator.SetBool("DoruIdle", true);
            doru.SetActive(true);
        }

        if ((leftHand == CardSet.Pilum) || (rightHand == CardSet.Pilum))
        {
            animator.SetBool("PilumIdle", true);
            pilum.SetActive(true);
        }

        if ((leftHand == CardSet.Pilum) || (rightHand == CardSet.Pilum))
        {
            animator.SetBool("PilumIdle", true);
            pilum.SetActive(true);
        }

        if ((leftHand == CardSet.Parmula) || (rightHand == CardSet.Parmula))
        {
            animator.SetBool("ParmulaIdle", true);
            animator.SetBool("ParmulaIdleForBlock", true);
            parmula.SetActive(true);
        }

        if ((leftHand == CardSet.Scindo) || (rightHand == CardSet.Scindo))
        {
            animator.SetBool("ScindoIdle", true);
            scindo.SetActive(true);
        }

        if ((leftHand == CardSet.Cestus) || (rightHand == CardSet.Cestus))
        {
            animator.SetBool("CestusIdle", true);
            cestus.SetActive(true);
        }

        if ((leftHand == CardSet.Laqueus) || (rightHand == CardSet.Laqueus))
        {
            animator.SetBool("LaqueusIdle", true);
            laqueus.SetActive(true);
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

        if ((shoulder == CardSet.Galerus))
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

