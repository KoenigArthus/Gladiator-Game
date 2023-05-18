using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Participant
{
    #region Fields

    private CardGameManager manager;

    private int health;
    private int blockSlots;
    private int[] statusEffectStacks = new int[8];

    #endregion Fields

    public Participant(CardGameManager manager, int health, int blockSlots)
    {
        this.manager = manager;
        this.health = health;
        this.blockSlots = blockSlots;
    }

    #region Properties

    protected CardGameManager Manager => manager;
    public int Health { get => health; set => health = value; }
    public int BonusDamage => GetStatus(StatusEffect.Strenght) - GetStatus(StatusEffect.Weak);
    public int Block => BlockStack.Sum();
    public abstract int[] BlockStack { get; }
    public int BlockSlots => blockSlots;

    #endregion Properties

    public void AdvanceRound()
    {
        OnAdvanceRound();

        //Apply status effects
        //Noting yet

        //Decay status effects
        int regeneration = GetStatus(StatusEffect.Regeneration);
        for (int i = (int)StatusEffect.Regeneration + 1; i < statusEffectStacks.Length; i++)
        {
            statusEffectStacks[i] = Mathf.Max(0, statusEffectStacks[i] - regeneration);
        }
    }

    protected virtual void OnAdvanceRound()
    {
    }

    #region Attack

    public void Attack(Participant target, int power)
    {
        //Calculate bonus
        power += BonusDamage;
        if (target.GetStatus(StatusEffect.Vulnerable) > 0)
            power += (int)(power * 0.05f);

        target.ReduceBlock(ref power);

        if (power > 0)
            target.Health -= power + target.GetStatus(StatusEffect.Bleeding);
    }

    public abstract void ReduceBlock(ref int amount);

    #endregion Attack

    #region Status

    public void AddStatus(StatusEffect effect, int count)
    {
        statusEffectStacks[(int)effect] += count;
    }

    public void RemoveStatus(StatusEffect effect, int count)
    {
        statusEffectStacks[(int)effect] -= count;
    }

    public int GetStatus(StatusEffect effect)
    {
        return statusEffectStacks[(int)effect];
    }

    #endregion Status

    public override string ToString()
    {
        return $"{GetType().Name}\n" +
            $"Health: {Health}\n" +
            $"Block : {Block}";
    }
}