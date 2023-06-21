#region System

using System;

public enum Language
{
    German, English
}

#endregion

#region CardInfo

public enum CardSet
{
    None = -1,

    //Weapons
    Trident, Gladius, Rete, Scutum, Pugio, Doru, Parmula, Scindo,

    //Armor
    Cassis, Galerus, Manica, Ocrea,

    //Other
    Health, Item
}

public enum CardType
{
    Attack, Block, Skill, Aid, Quest, Ailment
}

#endregion

#region Collection

public enum CardCollectionType
{
    Stack, List, Fan
}

#endregion

#region Effects

public enum PermanentEffect
{
    OnRoundStart, OnDeck, OnDraw, OnDefend, OnPlay, OnTribute,
}

public enum StatusEffect
{
    Strenght, Defence, Invulnerable, FragileStrenght, FragileDefence, Regeneration, Stun, Weak, Feeble, Bleeding, Vulnerable
}

[Flags]
public enum SpecialCardEffectFlags
{
    SkipRegeneration = 1, NegativeStatusShield = 2, StrengthBleedSalt = 4, Terror = 8
}

#endregion

#region Enemy

public enum EnemyBehavior
{
    None, Aggressive, Defensive, Balanced, Tactical
}

public enum EnemyAction
{
    Attack, Block, Special
}

#endregion