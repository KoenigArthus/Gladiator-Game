using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class CardInfo : ICloneable
{
    #region Fields

    private CardObject card;

    private string name = "<Error>";
    private CardSet set = CardSet.None;
    private CardType type;
    private int tier = 0;

    private int cost;
    private GetPower costReduction = null;
    private DieInfo[] dice = new DieInfo[0];

    private bool destroyOnDiscard;

    #endregion Fields

    #region ctor

    public CardInfo(string name, CardSet set, CardType type, int cost, bool destroyOnDiscard = false)
    {
        this.name = name;
        this.set = set;
        this.type = type;
        this.cost = cost;

        this.destroyOnDiscard = destroyOnDiscard || set == CardSet.Item;
    }

    #endregion ctor

    #region Setup

    public void Setup(CardObject card)
    {
        this.card = card;
    }

    #endregion Setup

    #region Properties

    public string Name => name;
    public string TranslatedName => CardLibrary.GetTranslatedName(Name);
    public string TranslatedDescription => CardLibrary.GetTranslatedDescription(Name);
    public CardSet Set => set;
    public CardType Type => type;

    public int Cost => cost - (costReduction != null && Player != null ? Math.Min(cost, costReduction(this)) : 0);
    public GetPower CostReduction { get => costReduction; set => costReduction = value; }
    public bool CostMeet => dice.Length == Cost;
    public int DicePower => dice.Sum(x => x.Value);

    public virtual bool DestroyOnDiscard => destroyOnDiscard;

    public CardObject Card => card;
    public Player Player => card?.Collection?.Player;
    public Enemy Enemy => Player?.Enemy;

    #endregion Properties

    public virtual void Clear()
    {
        dice = new DieInfo[0];
    }

    #region Dice

    public void ToggleDie(DieInfo die)
    {
        if (dice.Contains(die))
            dice = dice.Where(x => x != die).ToArray();
        else
            dice = dice.Concat(new DieInfo[] { die }).ToArray();
    }

    public bool HasDie(DieInfo die)
    {
        return dice.Contains(die);
    }

    public void DestroyDice()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            if (dice[i].Die != null && !dice[i].Die.gameObject.IsDestroyed())
                GameObject.Destroy(dice[i].Die.gameObject);
        }
    }

    #endregion Dice

    public abstract object Clone();
}