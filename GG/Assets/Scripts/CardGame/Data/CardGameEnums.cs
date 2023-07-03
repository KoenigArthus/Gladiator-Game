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
    Strength, Defence, Invulnerable, FragileStrength, FragileDefence, Regeneration, Stun, Weak, Feeble, Bleeding, Vulnerable
}

[Flags]
public enum SpecialCardEffectFlags
{
    SkipRegeneration = 1, NegativeStatusShield = 2, StrengthBleedSalt = 4, Terror = 8, Spiky_Block = 16
}

public enum Ailment
{
    Exhausted, Injured, Hunger, Terror, Trauma, Flesh_Wound, Bone_Fracture, Toxin, Hopeless, Degeneration, Discouraged, Psychosis, Blood_Poisoning, Frenzy, Crippling_Pain, Disease, Panic_Attack, Cardiac_Insufficiency
}

#endregion

#region Enemy

public enum EnemyType
{
    Tutorialgladiator, Murmillo, Trax, Hoplomachus, Scissor, Dimachaerus, Schwerathlet, Sklaventreiber, Sonnenbringer, Krieger, Nemesis, Retiarius,
    Huhn = -1, Bestie = -2, Bestienkämpfer = -3
}

public enum EnemyIntension
{
    Attack, Block, Skill, Special
}

public enum EnemyIntensionType
{
    Attack, Block, Buff, Debuff, Special, Ailment, Heal, Skip = -1
}

public enum BattleResult
{
    Lose = -1, Spare, Win
}

#endregion