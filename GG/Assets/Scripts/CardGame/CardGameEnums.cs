#region System

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