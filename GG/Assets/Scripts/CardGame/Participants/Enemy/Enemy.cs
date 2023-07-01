using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class Enemy : Participant
{
    #region Fields

    private int[] blockStack = new int[0];
    private SkillInfo[] intension = new SkillInfo[3];
    private EnemyIntension?[] forcedIntension = new EnemyIntension?[0];

    #endregion Fields

    public Enemy(CardGameManager manager, int level) : base(manager)
    {
    }

    #region Properties

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
                intension[i] = skills[CustomUtility.WeightedRandom(skills.Select(x => x.Chance).ToArray())];
            else
                intension[i] = currentSkills.FirstOrDefault();
        }
    }

    public void ChangeIntension(EnemyIntension action)
    {
        ChangeIntension(skills.Where(x => x.Intension == action).ToArray());
    }

    public void BlockIntension(EnemyIntension action)
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
        blockStack = blockStack.Concat(new int[] { amount }).ToArray();
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
        return base.ToString() + $"\nIntend: {Intension}";
    }
}