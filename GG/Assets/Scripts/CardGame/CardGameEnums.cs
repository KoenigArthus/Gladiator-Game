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
    Trident, Gladius, Rete, Scutum, Pugio, Spartha, Doru, Pilum, Parmula, Scindo, Cestus, Laqueus,

    //Armor
    Cassis, Galerus, Manica, Ocrea,

    //Other
    Health, Item
}

public enum CardType
{
    Attack, Block, Skill, Aid, Ailment, Quest
}

[Flags]
public enum EquipSlots
{
    RightHand = 1, LeftHand = 2, Head = 4, Shoulders = 8, Legs = 16
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
    OnRoundStart, OnDeck, OnDefend, OnPlay, OnTribute,
}

public enum StatusEffect
{
    Strenght, Defence, Invulnerable, FragileStrenght, FragileDefence, Regeneration, Stun, Weak, Feeble, Bleeding, Vulnerable
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