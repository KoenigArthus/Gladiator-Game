using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentCardInfo : CardInfo
{
    public PermanentCardInfo(string name, CardSet set, CardType type, int cost, bool destroyOnDiscard = false) :
        base(name, set, type, cost, destroyOnDiscard)
    {
    }

    public override object Clone()
    {
        throw new System.NotImplementedException();
    }

    public delegate CardInfo DrawAction(CardInfo effectCard, CardInfo drawnCard);
}