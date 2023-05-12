using System;

public class PermanentCardInfo : CardInfo
{
    #region Fields

    private (PermanentEffect, Delegate) effect;

    #endregion Fields

    #region ctor

    public PermanentCardInfo(string name, CardSet set, CardType type, int cost, bool destroyOnDiscard = false) :
        base(name, set, type, cost, destroyOnDiscard)
    {
    }

    public PermanentCardInfo(string name, CardSet set, CardType type, int cost, GameEvent gameEvent, bool destroyOnDiscard = false) :
        this(name, set, type, cost, destroyOnDiscard)
    {
        effect = (PermanentEffect.OnRoundStart, gameEvent);
    }

    public PermanentCardInfo(string name, CardSet set, CardType type, int cost, ChangeCardAction changeCard, bool destroyOnDiscard = false) :
        this(name, set, type, cost, destroyOnDiscard)
    {
        effect = (PermanentEffect.OnDeck, changeCard);
    }

    public PermanentCardInfo(string name, CardSet set, CardType type, int cost, AttackEvent attackEvent, bool destroyOnDiscard = false) :
        this(name, set, type, cost, destroyOnDiscard)
    {
        effect = (PermanentEffect.OnDefend, attackEvent);
    }

    public PermanentCardInfo(string name, CardSet set, CardType type, int cost, CardAction cardAction, bool playNotTributeAction, bool destroyOnDiscard = false) :
        this(name, set, type, cost, destroyOnDiscard)
    {
        effect = (playNotTributeAction ? PermanentEffect.OnPlay : PermanentEffect.OnTribute, cardAction);
    }

    #endregion ctor

    #region Properties

    public override bool DestroyOnDiscard => true;

    public (PermanentEffect, Delegate) Effect => effect;

    #endregion Properties

    public override object Clone()
    {
        return new PermanentCardInfo(Name, Set, Type, Cost, DestroyOnDiscard)
        {
            CostReduction = this.CostReduction,
            effect = this.effect
        };
    }
}