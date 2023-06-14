using System;

public class PermanentCardInfo : CardInfo
{
    #region Fields

    private (PermanentEffect, Delegate) effect;

    #endregion Fields

    #region ctor

    public PermanentCardInfo(string name, CardSet set, int tier, int cost) :
        base(name, set, CardType.Skill, tier, cost, true)
    {
    }

    public PermanentCardInfo(string name, CardSet set, int tier, int cost, GameEventAction gameEvent) :
        this(name, set, tier, cost)
    {
        effect = (PermanentEffect.OnRoundStart, gameEvent);
    }

    public PermanentCardInfo(string name, CardSet set, int tier, int cost, ChangeCardAction changeCard) :
        this(name, set, tier, cost)
    {
        effect = (PermanentEffect.OnDeck, changeCard);
    }

    public PermanentCardInfo(string name, CardSet set, int tier, int cost, CardsAction drawCrads) :
        this(name, set, tier, cost)
    {
        effect = (PermanentEffect.OnDraw, drawCrads);
    }

    public PermanentCardInfo(string name, CardSet set, int tier, int cost, AttackEvent attackEvent) :
        this(name, set, tier, cost)
    {
        effect = (PermanentEffect.OnDefend, attackEvent);
    }

    public PermanentCardInfo(string name, CardSet set, int tier, int cost, CardAction cardAction, bool playNotTributeAction) :
        this(name, set, tier, cost)
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
        return new PermanentCardInfo(Name, Set, Tier, Cost) { CostReduction = this.CostReduction, effect = this.effect };
    }
}