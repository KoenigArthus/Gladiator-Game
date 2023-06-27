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

    private Dictionary<string, Sprite> cardSprites = new Dictionary<string, Sprite>();

    private CardObject card;

    private string name = "<Error>";
    private CardSet set = CardSet.None;
    private CardType type = CardType.Quest;
    private int tier = 0;

    private int cost;
    private GetPower costReduction = null;
    private DieInfo[] dice = new DieInfo[0];
    private int diceBonus = 0;

    private int repeat = 1;

    private bool destroyOnDiscard;

    #endregion Fields

    #region ctor

    public CardInfo(string name, CardSet set, CardType type, int tier, int cost, bool destroyOnDiscard = false)
    {
        this.name = name;
        this.set = set;
        this.type = type;
        this.tier = tier;
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

    public Sprite Sprite
    { get { if (!cardSprites.ContainsKey(name)) cardSprites.Add(name, CardLibrary.GetSprite(name, set.ToString())); return cardSprites[name]; } }

    public CardSet Set => set;
    public CardType Type => type;
    public int Tier => tier;

    public int Cost
    {
        get
        {
            if (costReduction != null && Player != null)
            {
                int costReduction = this.costReduction(this);

                if (costReduction < 0)
                    return -1;

                return cost - costReduction;
            }

            return cost;
        }
    }

    public GetPower CostReduction { get => costReduction; set => costReduction = value; }
    public bool CostMeet => dice.Length == Cost;
    public int RawDicePower => dice.Sum(x => x.Value);
    public int DiceBonus { get => diceBonus; set => diceBonus = value; }
    public int DicePower => RawDicePower + diceBonus;

    public int Repeat { get => repeat; set => repeat = value; }

    public virtual bool DestroyOnDiscard { get => destroyOnDiscard; set => destroyOnDiscard = value; }

    public CardObject Card => card;
    public DieInfo[] Dice => dice;
    public Player Player => card?.Collection?.Player;
    public Enemy Enemy => Player?.Enemy;

    #endregion Properties

    public virtual void Execute()
    {
    }

    public virtual void Clear()
    {
        dice = new DieInfo[0];
        diceBonus = 0;
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

    public void RefundDice()
    {
        for (int i = 0; i < dice.Length; i++)
            Player.AddDie(dice[i], false);
    }

    #endregion Dice

    public abstract object Clone();
}