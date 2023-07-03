using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class Enemy : Participant
{
    #region Fields

    private EnemyType type;
    private int[] blockStack = new int[0];
    private SkillInfo[] intension = new SkillInfo[3];
    private EnemyIntension?[] forcedIntension = new EnemyIntension?[0];

    #endregion Fields

    public Enemy(CardGameManager manager, int level) : base(manager)
    {
        //Check out of range
        if (level < -12 || level > 13 || level == -10)
        {
            BuildEnemy(this, EnemyType.Tutorialgladiator);
            return;
        }

        bool sparring = level < 0;
        EnemyType type;
        if (level > -12 && level < 11)
            type = (EnemyType)Mathf.Abs(level);
        else
            type = (EnemyType)(-Mathf.Abs(level) + 10);

        if (sparring)
            BuildSparringPartner(this, type);
        else
            BuildBoss(this, type);

        this.type = type;
    }

    #region Properties

    public override string Name => type.ToString();
    public EnemyType Type => type;

    public CardSet[] Equipped
    {
        get
        {
            switch (type)
            {
                case EnemyType.Tutorialgladiator:
                case EnemyType.Murmillo:
                    return new CardSet[] { CardSet.Gladius, CardSet.Parmula }; //Scuntum
                case EnemyType.Trax:
                    return new CardSet[] { CardSet.Gladius, CardSet.Scutum };

                case EnemyType.Hoplomachus:
                    return new CardSet[] { CardSet.Doru, CardSet.Parmula };

                case EnemyType.Scissor:
                    return new CardSet[] { CardSet.Scindo, CardSet.Gladius };

                case EnemyType.Dimachaerus:
                    return new CardSet[] { CardSet.Gladius, CardSet.Gladius };

                case EnemyType.Schwerathlet:
                    return new CardSet[] { }; //Cestus
                case EnemyType.Sklaventreiber:
                    return new CardSet[] { CardSet.Scutum, CardSet.Rete };

                case EnemyType.Sonnenbringer:
                    return new CardSet[] { }; //Staff
                case EnemyType.Krieger:
                    return new CardSet[] { CardSet.Gladius };

                case EnemyType.Nemesis:
                    return new CardSet[] { CardSet.Gladius }; //Gladius
                case EnemyType.Retiarius:
                    return new CardSet[] { CardSet.Trident, CardSet.Rete };

                case EnemyType.Bestienkämpfer:
                    return new CardSet[] { CardSet.Scindo, CardSet.Scindo };
            }

            return new CardSet[0];
        }
    }

    public override int Health { get => health; set => health = value; }
    public override int[] BlockStack => blockStack;
    public string Intension => string.Join('|', intension.Select(x => x.ToString()));

    public Player Player => Manager.Player;

    #endregion Properties

    #region Play

    public void ChangeIntension()
    {
        ChangeIntension(skills);
    }

    public void ChangeIntension(SkillInfo[] skills)
    {
        if (ability != null && ability.CantUseSkillsThisTurn && Manager.Round % ability.Frequency == 0)
        {
            intension = new SkillInfo[intension.Length];
            return;
        }

        for (int i = 0; i < intension.Length; i++)
        {
            SkillInfo[] currentSkills = skills;

            if (i < forcedIntension.Length && forcedIntension[i] != null)
                currentSkills = currentSkills.Where(x => x.Intension == forcedIntension[i].Value).ToArray();
            else
                currentSkills = currentSkills.Where(x => x.Intension != EnemyIntension.Special).ToArray();

            if (currentSkills.Length > 1)
                intension[i] = skills[CustomUtility.WeightedRandom(skills.Select(x => x.GetChance(this)).ToArray())];
            else
                intension[i] = currentSkills.FirstOrDefault();
        }
    }

    public void ChangeIntension(EnemyIntension action)
    {
        ChangeIntension(skills.Where(x => x.Intension == action).ToArray());
    }

    public void LockIntension(EnemyIntension action)
    {
        for (int i = 0; i < intension.Length; i++)
        {
            if (intension[i] != null && intension[i].Intension == action)
                intension[i] = null;
        }
    }

    public void TakeTurn(Player target)
    {
        if (enrage != null && !(health > enrage.Threshold))
        {
            enrage.Execute(this);
            enrage = null;
        }

        if (ability != null && Manager.Round % ability.Frequency == 0)
            ability.Execute(this);

        int stun = GetStatus(StatusEffect.Stun);
        for (int i = 0; i < intension.Length; i++)
        {
            SkillInfo current = intension[i];

            if (current == null || IsStuned())
                continue;

            if (attackAilments != null && current.Intension == EnemyIntension.Attack)
                attackAilments.OnAttack(this);

            current.Execute(this);

#warning TODO: Play Enemy Animations
        }
        Manager.NotifyStatsChange();
    }

    #endregion Play

    #region Defend

    public void AddBlock(int amount)
    {
        this.blockStack = blockStack.Concat(new int[] { amount }).ToArray();

        if (this.blockStack.Length > 3)
        {
            int[] blockStack = new int[3];
            this.blockStack.CopyTo(blockStack, this.blockStack.Length - 3);
            this.blockStack = blockStack;
        }
    }

    protected override void ReduceBlock(ref int amount)
    {
        for (int i = 0; i < blockStack.Length; i++)
        {
            if (amount < blockStack[i])
            {
                blockStack[i] -= amount;
                amount = 0;
                break;
            }
            else
            {
                amount -= blockStack[i];
                blockStack[i] = 0;
            }
        }

        blockStack = blockStack.Where(x => x > 0).ToArray();
    }

    public override int RemoveLastBlock()
    {
        if (blockStack.Length < 1)
            return 0;

        int index = blockStack.Length - 1;
        int block = blockStack[index];
        blockStack[index] = 0;
        return block;
    }

    public override int RemoveAllBlock()
    {
        int block = Block;
        blockStack = new int[0];
        return block;
    }

    #endregion Defend

    public override string ToString()
    {
        return base.ToString() + $"\nIntend:\n{Intension}";
    }
}