using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class CardAnimations : MonoBehaviour
{
    public Animator animator;
    public Player player;
    public Player Player => player;

    public GameObject gladius;
    public GameObject trident;
    public GameObject scutum;
    public GameObject pugiu;
    public GameObject doru;
    public GameObject parmula;
    public GameObject scindo;
    public GameObject rete;
    public GameObject cassis;
    public GameObject galerus;
    public GameObject ocrea;

    public GameObject gladiusleft;
    public GameObject tridentleft;
    public GameObject scutumleft;
    public GameObject pugiuleft;
    public GameObject doruleft;
    public GameObject parmulaleft;
    public GameObject scindoleft;
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

        #region No Weapon
        if ((leftHand == CardSet.None) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingEmpty", true);           
        }
        #endregion No Weapon

        #region SwordRight
        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRight", true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRight", true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Scindo))
                {
            animator.SetBool("WalkingScindoRight", true);
            scindo.SetActive(true);
        }

        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingSwordRight", true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingSwordRight", true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingScindoRight", true);
            scindo.SetActive(true);
        }
        #endregion SwordRight

        #region SpearRight

        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingSpearRight", true);
            trident.SetActive (true);
        }
        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSpearRight", true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingSpearRight", true);
            trident.SetActive(true);
        }
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingSpearRight", true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.None))
        {
            animator.SetBool("ReteIdle", true);
            rete.SetActive(true);
        }
        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Rete))
        {
            animator.SetBool("ReteIdle", true);
            rete.SetActive(true);
        }
        #endregion SpearRight

        #region ShieldLeft
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingShieldRight", true);
            scutumleft.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.None))
        {
            animator.SetBool("WalkingShieldRight", true);
            parmulaleft.SetActive(true);
        }
        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingShieldRight", true);
            scutumleft.SetActive(true);
        }
        else if ((leftHand == CardSet.None) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingShieldRight", true);
            parmulaleft.SetActive(true);
        }
        #endregion ShieldRight

        #region SwordRightandLeft
        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRightandLeft", true);
            gladius.SetActive(true);
            gladiusleft.SetActive(true);
        }
        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRightandLeft", true);
            pugiu.SetActive(true);
            gladiusleft.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRightandLeft", true);
            gladius.SetActive(true);
            pugiuleft.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRightandLeft", true);
            pugiu.SetActive(true);
            pugiuleft.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRightandLeft", true);
            scindo.SetActive(true);
            scindoleft.SetActive(true);
        }
        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRightandLeft", true);
            gladiusleft.SetActive(true);
            scindo.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingScindoRightandLeft", true);
            scindoleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingScindoRightandLeft", true);
            scindo.SetActive(true);
            pugiuleft.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRightandLeft", true);
            pugiu.SetActive(true);
            scindoleft.SetActive(true);
        }

        #endregion SwordRightandLeft

        #region SwordRightandShieldLeft
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            scutumleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            scutumleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRight", true);
            scutumleft.SetActive(true);
            scindo.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRight", true);
            parmulaleft.SetActive(true);
            scindo.SetActive(true);
        }

        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            scutumleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            scutumleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingSwordRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingScindoRight", true);
            scutumleft.SetActive(true);
            scindo.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingScindoRight", true);
            parmulaleft.SetActive(true);
            scindo.SetActive(true);
        }
        #endregion SwordRightandShieldLeft

        #region SwordRightandSpearLeft
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            tridentleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            doruleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            tridentleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            doruleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRight", true);
            tridentleft.SetActive(true);
            scindo.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRight", true);
            doruleft.SetActive(true);
            scindo.SetActive(true);
        }

        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            tridentleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            doruleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            tridentleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if (   (leftHand == CardSet.Pugio) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            doruleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingScindoRight", true);
            tridentleft.SetActive(true);
            scindo.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingScindoRight", true);
            doruleft.SetActive(true);
            scindo.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Gladius))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            reteleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Rete))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            reteleft.SetActive(true);
            gladius.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Pugio))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            reteleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Rete))
        {
            animator.SetBool("WalkingSwordRightandSpearLeft", true);
            reteleft.SetActive(true);
            pugiu.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Scindo))
        {
            animator.SetBool("WalkingScindoRight", true);
            reteleft.SetActive(true);
            scindo.SetActive(true);
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Rete))
        {
            animator.SetBool("WalkingScindoRight", true);
            reteleft.SetActive(true);
            scindo.SetActive(true);
        }
        #endregion SwordRightandSpearLeft

        #region SpearRightandSpearLeft
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            tridentleft.SetActive(true);
            trident.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            doruleft.SetActive(true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            tridentleft.SetActive(true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) && (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            doruleft.SetActive(true);
            trident.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) || (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            reteleft.SetActive(true);
            trident.SetActive(true);
        }
        else if ((leftHand == CardSet.Trident) || (rightHand == CardSet.Rete))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            tridentleft.SetActive(true);
            rete.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) || (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            reteleft.SetActive(true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) || (rightHand == CardSet.Rete))
        {
            animator.SetBool("WalkingSpearRightandSpearLeft", true);
            doruleft.SetActive(true);
            rete.SetActive(true);
        }
        #endregion SpearRightandSpearLeft

        #region SpearRightandShieldLeft
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Trident))
        {
            
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            scutumleft.SetActive(true);
            trident.SetActive(true);
            
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Trident))
        {
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            trident.SetActive(true);
        }
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            scutumleft.SetActive(true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Doru))
        {
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            doru.SetActive(true);
        }

        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            scutumleft.SetActive(true);
            trident.SetActive(true);
        }
        else if ((leftHand == CardSet.Trident) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            trident.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            scutumleft.SetActive(true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Doru) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingSpearRightandShieldLeft", true);
            parmulaleft.SetActive(true);
            doru.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("ReteIdle", true);
            parmulaleft.SetActive(true);
            rete.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Rete))
        {
            animator.SetBool("ReteIdle", true);
            parmulaleft.SetActive(true);
            rete.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("ReteIdle", true);
            scutumleft.SetActive(true);
            rete.SetActive(true);
        }
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Rete))
        {
            animator.SetBool("ReteIdle", true);
            parmulaleft.SetActive(true);
            rete.SetActive(true);
        }
        #endregion SpearRightandShieldLeft

        #region ShieldLeftandShieldRight
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingShieldLeftandRight", true);
            scutumleft.SetActive(true);
            scutum.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingShieldLeftandRight", true);
            parmulaleft.SetActive(true);
            parmula.SetActive(true);
        }
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Parmula))
        {
            animator.SetBool("WalkingShieldLeftandRight", true);
            scutumleft.SetActive(true);
            parmula.SetActive(true);
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Scutum))
        {
            animator.SetBool("WalkingShieldLeftandRight", true);
            parmulaleft.SetActive(true);
            scutum.SetActive(true);
        }
        #endregion ShieldLeftandShieldRight

        #region Rete

        else if ((leftHand == CardSet.Rete) || (rightHand == CardSet.None))
        {
            animator.SetBool("ReteIdle", true);
            rete.SetActive(true);
        }
        else if ((leftHand == CardSet.None) || (rightHand == CardSet.Rete))
        {
            animator.SetBool("ReteIdle", true);
            rete.SetActive(true);
        }
        else if ((leftHand == CardSet.Rete) || (rightHand == CardSet.Rete))
        {
            animator.SetBool("ReteIdle", true);
            rete.SetActive(true);
        }

        
        #endregion Rete

        #region BodyWear
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
        #endregion BodyWear
    }

    public void PlaySwordAnimation()
    {
        switch (leftHand)
        {
            case CardSet.None:
                switch (rightHand)
                {
                    case CardSet.Gladius:
                    case CardSet.Pugio:
                        animator.SetTrigger("SwordAttack");
                        break;
                }
                break;

            case CardSet.Gladius:
            case CardSet.Pugio:
                switch (rightHand)
                {
                    case CardSet.None:
                    case CardSet.Scutum:
                    case CardSet.Parmula:
                    case CardSet.Trident:
                    case CardSet.Doru:
                        animator.SetTrigger("SwordAttack");
                        break;
                }
                break;

            case CardSet.Scutum:
            case CardSet.Parmula:
                switch (rightHand)
                {
                    case CardSet.Gladius:
                    case CardSet.Pugio:
                    case CardSet.Trident:
                    case CardSet.Doru:
                        animator.SetTrigger("SwordAttack");
                        break;
                }
                break;

            case CardSet.Trident:
            case CardSet.Doru:
                switch (rightHand)
                {
                    case CardSet.Gladius:
                    case CardSet.Pugio:
                        animator.SetTrigger("SwordAttack");
                        break;
                }
                break;


            default:
                break;
        }

    }

    public void PlayScindoAnimations()
    {
        switch ((leftHand, rightHand))
        {
            case (CardSet.None, CardSet.Scindo):
            case (CardSet.Scindo, CardSet.None):
            case (CardSet.Gladius, CardSet.Scindo):
            case (CardSet.Scindo, CardSet.Gladius):
            case (CardSet.Scindo, CardSet.Pugio):
            case (CardSet.Pugio, CardSet.Scindo):
            case (CardSet.Scindo, CardSet.Trident):
            case (CardSet.Trident, CardSet.Scindo):
            case (CardSet.Scindo, CardSet.Doru):
            case (CardSet.Doru, CardSet.Scindo):
            case (CardSet.Scindo, CardSet.Parmula):
            case (CardSet.Parmula, CardSet.Scindo):
            case (CardSet.Scindo, CardSet.Scutum):
            case (CardSet.Scutum, CardSet.Scindo):
                animator.SetBool("ScindoAttack", true);
                break;

            case (CardSet.Scindo, CardSet.Scindo):
                animator.SetBool("DoubleScindoAttack", true);
                break;
        }

    }

    public void PlaySpearwithShieldAnimation()
    {
        switch (leftHand)
        {
            case CardSet.Scutum:
            case CardSet.Parmula:
                switch (rightHand)
                {
                    case CardSet.Trident:
                    case CardSet.Doru:
                        animator.SetTrigger("SpearRightandShieldLeft");
                        break;
                }
                break;

            case CardSet.Trident:
            case CardSet.Doru:
                switch (rightHand)
                {
                    case CardSet.Scutum:
                    case CardSet.Parmula:
                        animator.SetTrigger("SpearRightandShieldLeft");
                        break;
                }
                break;


            default:
                break;
        }


    }

    public void PlaySpearAnimation()
    {
        switch (leftHand)
        {
            case CardSet.None:
                switch (rightHand)
                {
                    case CardSet.Trident:
                    case CardSet.Doru:
                        animator.SetTrigger("SpearAttack");
                        break;
                }
                break;

            case CardSet.Trident:
                if (rightHand == CardSet.None)
                {
                    animator.SetTrigger("SpearAttack");
                }
                break;


            default:
                break;
        }
    }

    public void PlayDoubleSpearAnimation()
    {
        switch (leftHand)
        {
            case CardSet.Trident:
                switch (rightHand)
                {
                    case CardSet.Trident:
                    case CardSet.Doru:
                        animator.SetTrigger("DoubleSpearAttack");
                        break;
                }
                break;

            case CardSet.Doru:
                if (rightHand == CardSet.Trident)
                {
                    animator.SetTrigger("DoubleSpearAttack");
                }
                break;


            default:
                break;
        }

    }

    public void PlayShieldAttackAnimation()
    {
        switch (leftHand)
        {
            case CardSet.Scutum:
            case CardSet.Parmula:
                if (rightHand == CardSet.None)
                {
                    animator.SetTrigger("ShieldAttack");
                }
                break;

            case CardSet.None:
                switch (rightHand)
                {
                    case CardSet.Scutum:
                    case CardSet.Parmula:
                        animator.SetTrigger("ShieldAttack");
                        break;
                }
                break;


            default:
                break;
        }


    }

    public void PlayReteAnimation()
    {
        if ((leftHand == CardSet.Rete) || (rightHand == CardSet.Trident))
        {
            animator.SetTrigger("DoubleReteAttack");
        }
        else if ((leftHand == CardSet.Trident) || (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("DoubleReteAttack");
        }
        else if ((leftHand == CardSet.Rete) || (rightHand == CardSet.Doru))
        {
            animator.SetTrigger("DoubleReteAttack");
        }
        else if ((leftHand == CardSet.Doru) || (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("DoubleReteAttack");
        }
        else if ((leftHand == CardSet.Rete) || (rightHand == CardSet.None))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.None) || (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Rete) || (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("DoubleReteAttack");
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Gladius))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Gladius) && (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Pugio))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Pugio) && (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Scindo))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Scindo) && (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Parmula))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Parmula) && (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Rete) && (rightHand == CardSet.Scutum))
        {
            animator.SetTrigger("ReteAttack");
        }
        else if ((leftHand == CardSet.Scutum) && (rightHand == CardSet.Rete))
        {
            animator.SetTrigger("ReteAttack");
        }

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

