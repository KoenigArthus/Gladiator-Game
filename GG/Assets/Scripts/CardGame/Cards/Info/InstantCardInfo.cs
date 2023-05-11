using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCardInfo : CardInfo
{
    private CardAction action;

    public InstantCardInfo(string name, CardSet set, CardType type, int cost, CardAction action, bool destroyOnDiscard = false) :
        base(name, set, type, cost, destroyOnDiscard)
    {
        this.action = action;
    }

    public void Execute()
    {
        action(this);
    }

    public override object Clone()
    {
        return new InstantCardInfo(Name, Set, Type, Cost, action, DestroyOnDiscard);
    }

    public delegate void CardAction(CardInfo card);
}