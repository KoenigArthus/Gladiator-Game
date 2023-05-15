using System;

public class PassiveBlockCardInfo : BlockCardInfo
{
    private CardAction defendEvent;

    public PassiveBlockCardInfo(string name, CardSet set, CardType type, int cost, GetPower blockPower, CardAction defendEvent, bool destroyOnDiscard = false) :
        base(name, set, type, cost, blockPower, destroyOnDiscard)
    {
        this.defendEvent = defendEvent;
    }

    public void OnBlock()
    {
        defendEvent(this);
    }

    public override object Clone()
    {
        return new PassiveBlockCardInfo(Name, Set, Type, Cost, BlockPower, defendEvent, DestroyOnDiscard);
    }
}