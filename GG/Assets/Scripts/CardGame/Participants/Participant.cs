using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Action
{
    Attack, Block, Special
}

public enum StatusEffect
{
    Weak, Stun
}

public abstract class Participant
{
    #region Fields

    private CardGameManager manager;

    private int health;
    private int blockSlots;
    private int[] statusEffectStacks = new int[5];

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
    public int Block => BlockStack.Sum();
    public abstract int[] BlockStack { get; }
    public int BlockSlots => blockSlots;

    #endregion Properties

    public void AdvanceRound()
    {
        //Apply status effects
    }

    #region Attack

    public void Attack(Participant target, int power)
    {
        target.ReduceBlock(ref power);

        target.Health -= power;
    }

    protected abstract void ReduceBlock(ref int amount);

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