using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCardInfo : CardInfo
{
    protected CardAction action;

    public InstantCardInfo(string name, CardSet set, CardType type, int tier, int cost, CardAction action, bool destroyOnDiscard = false) :
        base(name, set, type, tier, cost, destroyOnDiscard)
    {
        this.action = action;
    }

    protected InstantCardInfo(string name, CardSet set, CardType type, int tier, int cost, bool destroyOnDiscard = false) :
        base(name, set, type, tier, cost, destroyOnDiscard)
    {
    }

    public override void Execute()
    {
        if (action != null)
            action(this);
    }

    public override object Clone()
    {
        return new InstantCardInfo(Name, Set, Type, Tier, Cost, action, DestroyOnDiscard) { CostReduction = this.CostReduction };
    }
}