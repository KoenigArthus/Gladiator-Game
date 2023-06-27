using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCardInfo : InstantCardInfo
{
    #region Fields

    private GetPower blockPower;
    private int damage = 0;
    private (StatusEffect, int) statusMod = (StatusEffect.FragileStrenght, -1);

    #endregion Fields

    public BlockCardInfo(string name, CardSet set, int tier, int cost, GetPower blockPower, bool destroyOnDiscard = false) :
        base(name, set, CardType.Block, tier, cost, destroyOnDiscard)
    {
        this.blockPower = blockPower;
    }

    #region Properties

    public int CurrentBlock => 42;// blockPower(this) - this.damage;
    protected GetPower BlockPower => blockPower;
    public (StatusEffect, int) StatusMod { get => statusMod; set => statusMod = value; }
    public CardAction InstantAction { get => action; set => action = value; }

    #endregion Properties

    public void ApplyDamage(ref int damage)
    {
        int block = CurrentBlock;

        if (damage < block)
        {
            this.damage += damage;
            damage = 0;
        }
        else
        {
            damage -= block;

            if (this is PassiveBlockCardInfo passive)
                passive.OnBreak(ref damage);

            Player.DiscardSingle(Card);
        }
    }

    public override void Clear()
    {
        base.Clear();
        ResetDamage();
    }

    public void ResetDamage()
    {
        this.damage = 0;
    }

    public override object Clone()
    {
        return new BlockCardInfo(Name, Set, Tier, Cost, blockPower, DestroyOnDiscard) { action = this.action, CostReduction = this.CostReduction };
    }
}