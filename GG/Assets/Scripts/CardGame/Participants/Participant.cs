using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Participant
{
    #region Fields

    private CardGameManager manager;

    private int blockSlots = 3;
    private int[] statusEffectStacks = new int[CardGameConstants.STATUS_EFFECT_COUNT];
    private int lastStatusDecayAmount = 0;
    private SpecialCardEffectFlags specialFlags = 0;

    #endregion Fields

    public Participant(CardGameManager manager)
    {
        this.manager = manager;
    }

    #region Properties

    protected CardGameManager Manager => manager;
    public abstract string Name { get; }
    public abstract int Health { get; set; }
    public int Strenght => GetStatus(StatusEffect.Strength) + GetStatus(StatusEffect.FragileStrength);
    public int BonusDamage => Strenght - GetStatus(StatusEffect.Weak);
    public int Defence => GetStatus(StatusEffect.Feeble);
    public int BonusBlock => Defence - GetStatus(StatusEffect.Feeble);
    public int Block => BlockStack.Sum();
    public abstract int[] BlockStack { get; }
    public int BlockSlots { get => blockSlots; set => blockSlots = value; }
    public int LastStatusDecayAmount => lastStatusDecayAmount;

    public bool SkipRegeneration
    { get => specialFlags.HasFlag(SpecialCardEffectFlags.SkipRegeneration); set { if (value) specialFlags |= SpecialCardEffectFlags.SkipRegeneration; else specialFlags &= (SpecialCardEffectFlags)(int.MaxValue ^ (int)SpecialCardEffectFlags.SkipRegeneration); } }

    public bool NegativeStatusShield
    { get => specialFlags.HasFlag(SpecialCardEffectFlags.NegativeStatusShield); set { if (value) specialFlags |= SpecialCardEffectFlags.NegativeStatusShield; else specialFlags &= (SpecialCardEffectFlags)(int.MaxValue ^ (int)SpecialCardEffectFlags.NegativeStatusShield); } }

    public bool StrengthBleedSalt
    { get => specialFlags.HasFlag(SpecialCardEffectFlags.StrengthBleedSalt); set { if (value) specialFlags |= SpecialCardEffectFlags.StrengthBleedSalt; else specialFlags &= (SpecialCardEffectFlags)(int.MaxValue ^ (int)SpecialCardEffectFlags.StrengthBleedSalt); } }

    public bool Terror
    { get => specialFlags.HasFlag(SpecialCardEffectFlags.Terror); set { if (value) specialFlags |= SpecialCardEffectFlags.Terror; else specialFlags &= (SpecialCardEffectFlags)(int.MaxValue ^ (int)SpecialCardEffectFlags.Terror); } }

    #endregion Properties

    public void AdvanceRound()
    {
        OnAdvanceRound();

        //Apply status effects
        //Noting yet

        //Decay status effects
        DoRegeneration();

        Terror = false;
    }

    protected virtual void OnAdvanceRound()
    {
    }

    public void DoRegeneration()
    {
        lastStatusDecayAmount = 0;

        //Reset status effects
        for (int i = (int)StatusEffect.Invulnerable; i < (int)StatusEffect.Regeneration; i++)
        {
            statusEffectStacks[i] = 0;
        }

        if (SkipRegeneration)
        {
            SkipRegeneration = false;
            return;
        }

        //Decay status effects
        int regeneration = Mathf.Max(1, GetStatus(StatusEffect.Regeneration));
        for (int i = (int)StatusEffect.Regeneration + 1; i < statusEffectStacks.Length; i++)
        {
            int decay = Mathf.Min(statusEffectStacks[i], regeneration);
            if (decay > 0)
            {
                lastStatusDecayAmount += decay;
                statusEffectStacks[i] -= decay;
            }
        }
    }

    public void Heal(int amount)
    {
        if (amount > 0)
            Health += amount;
    }

    public void InstantDamage(int amount, bool piercing = true)
    {
        if (amount < 1)
            return;

        if (!piercing)
            ReduceBlock(ref amount);

        Health -= amount;
    }

    #region Attack

    public int Attack(Participant target, int power, bool piercing = false, bool doubleBlockDamage = false)
    {
        Debug.Log($"{this.Name} attacked {target.Name} for {power} Damage. Piercing:{piercing} | DoubleBlockDamage:{doubleBlockDamage}");

        OnAttack(this, target, power);
        target.OnAttack(this, target, power);

        //Doge attack if invulnerable
        if (GetStatus(StatusEffect.Invulnerable) > 0)
        {
            RemoveStatus(StatusEffect.Invulnerable, 1);
            return 0;
        }

        //Calculate bonus
        power += BonusDamage;
        if (target.GetStatus(StatusEffect.Vulnerable) > 0)
            power += (int)(power * 0.05f);

        if (!piercing)
        {
            if (doubleBlockDamage)
            {
                int basePower = power;

                power += basePower;

                //Reduce block and trigger block effects
                target.ReduceBlock(ref power);

                power = Mathf.Max(0, power - basePower);
            }
            else
                //Reduce block and trigger block effects
                target.ReduceBlock(ref power);

            //Dage attack if invulnerable from block effect
            if (GetStatus(StatusEffect.Invulnerable) > 0)
            {
                RemoveStatus(StatusEffect.Invulnerable, 1);
                return 0;
            }
        }

        //Add bleed bonus damage
        power += target.GetStatus(StatusEffect.Bleeding);

        //Deal health damage
        if (power > 0)
            target.Health -= power;

        return power;
    }

    protected abstract void ReduceBlock(ref int amount);

    public abstract int RemoveLastBlock();

    public abstract int RemoveAllBlock();

    public virtual void OnAttack(Participant attacker, Participant defender, int power)
    {
    }

    public bool IsStuned()
    {
        int stun = GetStatus(StatusEffect.Stun);
        if (stun < 1)
            return false;

        float chance = 0.1f / (0.1f * stun + 1);
        bool stuned = Random.Range(0, 1 - float.Epsilon) > chance;

        return stuned;
    }

    #endregion Attack

    #region Status

    public virtual void AddStatus(StatusEffect effect, int count)
    {
        //NegativeStatusShield
        if (effect > StatusEffect.Regeneration && count > 0 && BlockStack.Length > 0 && NegativeStatusShield)
            return;

        if (count > 0)
            statusEffectStacks[(int)effect] += count;
    }

    public virtual void RemoveStatus(StatusEffect effect, int count, bool allowNegative = false)
    {
        if (count > 0)
            statusEffectStacks[(int)effect] -= count;

        if (!allowNegative && statusEffectStacks[(int)effect] < 0)
            statusEffectStacks[(int)effect] = 0;
    }

    public virtual int GetStatus(StatusEffect effect)
    {
        return statusEffectStacks[(int)effect];
    }

    #endregion Status

    public override string ToString()
    {
        string str = $"{Name}\n" +
            $"Health: {Health}\n" +
            $"Block : {Block}";

        for (int i = 0; i < statusEffectStacks.Length; i++)
        {
            if (statusEffectStacks[i] != 0)
                str += $"\n{(StatusEffect)i}: {statusEffectStacks[i]}";
        }

        return str;
    }
}