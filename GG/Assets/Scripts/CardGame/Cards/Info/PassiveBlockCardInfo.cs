using System;

public class PassiveBlockCardInfo : BlockCardInfo
{
    #region Fields

    private bool blockNotBreakAction;
    private DefendEvent defendEvent;
    private GameEventAction roundStartEvent;

    #endregion Fields

    #region ctor

    public PassiveBlockCardInfo(string name, CardSet set, int tier, int cost, GetPower blockPower, DefendEvent defendEvent, bool blockNotBreakAction, bool destroyOnDiscard = false) :
        base(name, set, tier, cost, blockPower, destroyOnDiscard)
    {
        this.blockNotBreakAction = blockNotBreakAction;
        this.defendEvent = defendEvent;
    }

    public PassiveBlockCardInfo(string name, CardSet set, int tier, int cost, GetPower blockPower, GameEventAction roundStartEvent, bool destroyOnDiscard = false) :
        base(name, set, tier, cost, blockPower, destroyOnDiscard)
    {
        this.roundStartEvent = roundStartEvent;
    }

    #endregion ctor

    #region OnEvent

    public void OnBlock(ref int damage)
    {
        if (blockNotBreakAction && defendEvent != null)
            defendEvent(this, ref damage);
    }

    public void OnBreak(ref int damage)
    {
        if (!blockNotBreakAction && defendEvent != null)
            defendEvent(this, ref damage);
    }

    public void OnRoundStart(CardGameManager manager)
    {
        if (roundStartEvent != null)
            roundStartEvent(manager, this);
    }

    #endregion OnEvent

    public override object Clone()
    {
        return new PassiveBlockCardInfo(Name, Set, Tier, Cost, BlockPower, defendEvent, DestroyOnDiscard) { action = this.action, CostReduction = this.CostReduction };
    }
}