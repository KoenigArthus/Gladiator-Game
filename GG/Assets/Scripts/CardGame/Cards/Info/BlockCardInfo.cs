using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCardInfo : PermanentCardInfo
{
    private GetPower blockPower;
    private int damage = 0;

    public BlockCardInfo(string name, CardSet set, CardType type, int cost, GetPower blockPower, bool destroyOnDiscard = false) :
        base(name, set, type, cost, destroyOnDiscard)
    {
        this.blockPower = blockPower;
    }

    public int CurrentBlock => blockPower(this) - this.damage;

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
            Player.DiscardSingle(Card);
        }
    }

    public override void Clear()
    {
        base.Clear();
        this.damage = 0;
    }

    public override object Clone()
    {
        return new BlockCardInfo(Name, Set, Type, Cost, blockPower, DestroyOnDiscard);
    }

    public delegate int GetPower(CardInfo card);
}