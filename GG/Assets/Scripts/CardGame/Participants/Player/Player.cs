using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;

public class Player : Participant
{
    #region Fields

    private static readonly int[] diceValues = new int[] { 4, 6, 8, 12, 20 };

    private CardCollection deck;
    private CardCollection hand;
    private CardCollection block;
    private CardCollection discard;

    private DieCollection dice;
    private int diceAmount = 3;
    private int dicePower = 0;

    private int drawAmount = 4;

    private List<CardInfo> playedCards = new List<CardInfo>();
    private bool[] lockedCardTypes = new bool[6];

    private CardObject prepareing = null;
    private CardObject[] selecting = null;

    private Delegate[][] permanentEffects = new Delegate[][]
    {
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {}
    };

    private Delegate[][] passiveEffects = new Delegate[][]
    {
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {},
        new Delegate[] {}
    };

    //bonus bleed stacks on applied bleed
    private int bleedSalt = 0;

    #endregion Fields

    public Player(CardGameManager manager) : base(manager)
    {
        this.deck = manager.deck;
        this.hand = manager.hand;
        this.block = manager.block;
        this.discard = manager.discard;
        this.dice = manager.dice;

        this.deck.Player = this;
        this.hand.Player = this;
        this.hand.cardsInteractable = true;
        this.block.Player = this;
        this.discard.Player = this;
    }

    #region Properties

    public override string Name => "Player";
    public override int Health { get => UserFile.SaveGame.Health; set => UserFile.SaveGame.Health = value; }
    public override int[] BlockStack => block.Cards.Select(x => (x.Info as BlockCardInfo).CurrentBlock).ToArray();
    public Enemy Enemy => Manager.Enemy;

    public CardCollection Deck => deck;
    public CardCollection Hand => hand;
    public CardCollection Discard => discard;
    public CardCollection ActiveBlock => block;
    public DieObject[] Dice => dice.Dice.Where(x => x != null && !x.IsDestroyed()).ToArray();

    public CardInfo[] PlayedCards => playedCards.Where(x => x != null).ToArray();

    public CardObject Prepareing => prepareing;

    public bool Busy => false;

    public int BleedSalt { get => bleedSalt; set => bleedSalt = value; }

    #endregion Properties

    #region Animators

    public CardAnimations cardAnimations;
    public Focus camFocus;
    public BloodController bloodController;

    #endregion Animators

    #region DrawCards

    public void DrawCards(int amount)
    {
        CardInfo[] drawnCards = new CardInfo[amount];
        for (int i = 0; i < amount; i++)
        {
            if (deck.Count < 1)
            {
                discard.MoveAllTo(deck);
                deck.Shuffle();
            }

            if (deck.Count < 1)
                break;

            CardObject drawnCard = deck.DrawCard();
            CardInfo drawnCardInfo = drawnCard.Info;
            drawnCards[i] = drawnCardInfo;
            hand.Add(drawnCard);

            if (drawnCardInfo.Type == CardType.Ailment && drawnCardInfo.Cost == 0)
            {
                camFocus.PlayerAilmentParticles(CardType.Ailment);
                drawnCardInfo.Execute();

                //prepareing = drawnCard;
                //PlayPreparedCard();
            }
        }

        CardsAction[] drawActions = GetActionEffects(PermanentEffect.OnRoundStart).Cast<CardsAction>().ToArray();
        for (int i = 0; i < drawActions.Length; i++)
        {
            drawActions[i](drawnCards);
        }
    }

    public void DrawCards()
    {
        DrawCards(drawAmount);
    }

    #endregion DrawCards

    #region Play

    private void Awake()
    {
    }

    public void Update()
    {
        if (prepareing != null)
        {
            if (prepareing.Info.CostMeet)
                PlayPreparedCard();
            else
                prepareing.TargetPosition = new Vector3(0, 700, 0);
        }

        if (hand.Changed)
        {
            CardObject[] cards = hand.Cards;
            ChangeCard[] change = GetActionEffects(PermanentEffect.OnDeck).Cast<ChangeCard>().ToArray();
            for (int i = 0; i < cards.Length; i++)
            {
                for (int j = 0; j < change.Length; j++)
                {
                    cards[i].ChangeInfo(change[j](cards[i].Info));
                }
            }

            hand.ChangeHandeled();
        }
    }

    protected void PlayPreparedCard()
    {
        if (prepareing == null)
        {
            Debug.Log("Nothing prepared");
            return;
        }

        CardObject card = prepareing;
        CardInfo info = card.Info;

        prepareing = null;

        //Destroy used dice
        card.Info.DestroyDice();

        if (Terror && UnityEngine.Random.Range(0, 2) > 0)
        {
            Debug.Log("Terror");
            playedCards.Add(null);
        }
        else
        {
            if (!IsStuned())
            {
                Debug.Log("PlayActions");
                CardAction[] playActions = GetActionEffects(PermanentEffect.OnPlay).Cast<CardAction>().ToArray();
                for (int i = 0; i < playActions.Length; i++)
                {
                    playActions[i](info);
                }
                info = card.Info;

                if (info.Type != CardType.Ailment)
                    playedCards.Add(info);

                Debug.Log("Execute Effect");

                if (info is InstantCardInfo instantCard)
                {
                    bool dontDiscard = false;

                    if (info is BlockCardInfo blockCard)
                    {
                        Debug.Log("Block");

                        //Apply bonus block
                        card.Info.DiceBonus += BonusBlock;

                        if (card.Info.DicePower > 0)
                        {
                            //Hand -> Block
                            block.Add(card);

                            //Discard if block is overfilled
                            if (block.Count > BlockSlots)
                                DiscardSingle(block.Cards[0]);

                            //Dont discard block
                            dontDiscard = true;
                        }
                    }

                    Debug.Log("Execute Instant Effect");
                    for (int i = 0; i < Mathf.Max(1, instantCard.Repeat); i++)
                        instantCard.Execute();

                    PlayCardAnimation(info);
                    Manager.NotifyStatsChange();

                    //Dont discard block
                    if (dontDiscard)
                        return;
                }
                else if (info is PermanentCardInfo permanentCard)
                {
                    Debug.Log("Register Permanent Effect");

                    PermanentEffect effect = permanentCard.Effect.Item1;
                    Delegate action = permanentCard.Effect.Item2;

                    AddActionEffect(effect, action, permanentCard, true);
                }
            }
            else
            {
                Debug.Log("Stunned");
                playedCards.Add(null);
                AbortPreparedCardPlay();
            }
        }

        //Hand -> Discard
        DiscardSingle(card);
    }

    public void PrepareCard(CardObject card)
    {
        if (!card.Info.Name.Equals(CardLibrary.BLOCK_PLAY_CARD_NAME) && hand.Cards.Any(x => x.Info.Name.Equals(CardLibrary.BLOCK_PLAY_CARD_NAME)))
        {
            Debug.Log($"{card.Info.Name} can not be played! Play {CardLibrary.BLOCK_PLAY_CARD_NAME} instead.");
            return;
        }

        if (card != null && !lockedCardTypes[(int)card.Info.Type] && (card.Info.Type != CardType.Ailment || card.Info.Cost > 0))
            prepareing = card;
    }

    public void AbortPreparedCardPlay()
    {
        if (prepareing != null)
        {
            prepareing.Info.Clear();
            prepareing = null;
        }
    }

    public void LockCardType(CardType type)
    {
        lockedCardTypes[(int)type] = true;
    }

    public bool CanPlayCards()
    {
        if (hand.Cards.Length < 1)
            return false;

        int cost = hand.Cards.Min(x => x.Info.Cost);

        return !(Dice.Count() < cost);
    }

    public void PlayCardAnimation(CardInfo info)
    {
        if (info.Type == CardType.Attack)
        { 


            switch (info.Set)
            {
                case CardSet.Trident:
                    cardAnimations.PlaySpearwithShieldAnimation();
                    cardAnimations.PlaySwordAnimation();
                    cardAnimations.PlaySpearAnimation();
                    cardAnimations.PlayDoubleSpearAnimation();
                    cardAnimations.PlayScindoAnimations();
                    cardAnimations.PlayReteAnimation();
                    Debug.Log("TridentGespielt");
                    break;

                case CardSet.Gladius:
                    cardAnimations.PlaySwordAnimation();
                    cardAnimations.PlayScindoAnimations();
                    cardAnimations.PlayReteAnimation();
                    Debug.Log("GladiusGespielt");
                    break;

                case CardSet.Rete:
                    cardAnimations.PlayReteAnimation();
                    Debug.Log("ReteGespielt");
                    break;

                case CardSet.Scutum:
                    cardAnimations.PlayShieldAttackAnimation();
                    cardAnimations.PlayScindoAnimations();
                    cardAnimations.PlayReteAnimation();
                    Debug.Log("ScutumGespielt");

                    break;

                case CardSet.Pugio:
                    cardAnimations.PlaySwordAnimation();
                    cardAnimations.PlayScindoAnimations();
                    cardAnimations.PlayReteAnimation();
                    Debug.Log("GladiusGespielt");
                    break;

                case CardSet.Doru:
                    cardAnimations.PlaySpearwithShieldAnimation();
                    cardAnimations.PlaySwordAnimation();
                    cardAnimations.PlaySpearAnimation();
                    cardAnimations.PlayDoubleSpearAnimation();
                    cardAnimations.PlayScindoAnimations();
                    cardAnimations.PlayReteAnimation();
                    Debug.Log("DoruGespielt");
                    break;

                case CardSet.Parmula:
                    cardAnimations.PlayShieldAttackAnimation();
                    cardAnimations.PlayScindoAnimations();
                    cardAnimations.PlayReteAnimation();
                    cardAnimations.PlaySwordAnimation();
                    Debug.Log("ParmulaGespielt");

                    break;

                case CardSet.Scindo:
                    cardAnimations.PlaySwordAnimation();
                    cardAnimations.PlayScindoAnimations();
                    cardAnimations.PlayReteAnimation();
                    Debug.Log("GladiusGespielt");
                    break;
            }

        }

        if (info.Type == CardType.Block)
        {     

            switch (info.Set)
            {
                case CardSet.Scutum:
                    cardAnimations.PlayScutumBlockAnimation();
                    Debug.Log("Scutum Block");
                    break;

                case CardSet.Parmula:
                    cardAnimations.PlayParmulaBlockAnimation();
                    Debug.Log("Parmula Block");
                    break;
            }

        }
            camFocus.PlayerDecideFocus(info.Type);
    }

    #endregion Play

    #region Effects

    public void AddActionEffect(GameEventAction action, CardInfo card, bool permanent = false)
    {
        AddActionEffect(PermanentEffect.OnRoundStart, action, card, permanent);
    }

    public void AddActionEffect(ChangeCardAction action, CardInfo card, bool permanent = false)
    {
        AddActionEffect(PermanentEffect.OnDeck, action, card, permanent);
    }

    public void AddActionEffect(CardsAction action, CardInfo card, bool permanent = false)
    {
        AddActionEffect(PermanentEffect.OnDraw, action, card, permanent);
    }

    public void AddActionEffect(AttackEvent action, CardInfo card, bool permanent = false)
    {
        AddActionEffect(PermanentEffect.OnDefend, action, card, permanent);
    }

    public void AddActionEffect(CardAction action, bool playNotTributeAction, CardInfo card, bool permanent = false)
    {
        if (playNotTributeAction)
            AddActionEffect(PermanentEffect.OnPlay, action, card, permanent);
        else
            AddActionEffect(PermanentEffect.OnTribute, action, card, permanent);
    }

    protected void AddActionEffect(PermanentEffect effectType, Delegate action, CardInfo card, bool permanent = false)
    {
        if (effectType == PermanentEffect.OnDeck)
        {
            if (action is ChangeCardAction changeCardAction)
            {
                int power = 0;
                if (card != null)
                    power = card.DicePower;

                ChangeCard changeAction = (CardInfo c) => changeCardAction(c, power);
                action = changeAction;
            }
            else
            {
                Debug.Log($"Black Magic failed! | Card: {card.Name} | If you ever see this message, RUN!");
                return;
            }
        }
        else if (effectType == PermanentEffect.OnRoundStart)
        {
            if (action is GameEventAction gameEvent)
            {
                CardInfo snapshot = null;
                if (card != null)
                    snapshot = (CardInfo)card.Clone();

                GameEvent eventAction = (CardGameManager m) => gameEvent(m, snapshot);
                action = eventAction;
            }
            else
            {
                Debug.Log($"Black Magic failed! | Card: {card.Name} | If you ever see this message, RUN!");
                return;
            }
        }
        if (permanent)
            permanentEffects[(int)effectType] = permanentEffects[(int)effectType].Concat(new Delegate[] { action }).ToArray();
        else
            passiveEffects[(int)effectType] = passiveEffects[(int)effectType].Concat(new Delegate[] { action }).ToArray();
    }

    public Delegate[] GetActionEffects(PermanentEffect effectType)
    {
        int index = (int)effectType;
        return permanentEffects[index].Concat(passiveEffects[index]).ToArray();
    }

    public void AddAilment(Ailment ailment, bool addToDeck = false)
    {
        string cardName = ailment.ToString();
        CardInfo info = CardLibrary.GetCardByName(cardName);
        CardObject card = CardObject.Instantiate(info, Vector2.zero);

        if (!addToDeck)
            discard.Add(card);
        else
        {
            deck.Add(card);
            deck.Shuffle();
        }

        if (!info.DestroyOnDiscard)
            UserFile.SaveGame.DeckCardEntries = UserFile.SaveGame.DeckCardEntries.Concat(new string[] { cardName }).ToArray();
    }

    #endregion Effects

    #region Attack

    public override void OnAttack(Participant attacker, Participant defender, int power)
    {
        if (defender == this)
        {
            AttackEvent[] attackEvents = GetActionEffects(PermanentEffect.OnDefend).Cast<AttackEvent>().ToArray();
            for (int i = 0; i < attackEvents.Length; i++)
            {
                attackEvents[i](attacker, defender, power);
            }
        }
    }

    #endregion Attack

    #region Defend

    protected override void ReduceBlock(ref int amount)
    {
        BlockCardInfo[] blockCards = block.Cards.Select(x => x.Info as BlockCardInfo).ToArray();

        for (int i = 0; i < blockCards.Length; i++)
        {
            if (blockCards[i] is PassiveBlockCardInfo passiveInfo)
                passiveInfo.OnBlock(ref amount);
        }

        if (GetStatus(StatusEffect.Invulnerable) > 0)
            return;

        for (int i = 0; i < blockCards.Length; i++)
        {
            if (amount > 0)
                blockCards[i].ApplyDamage(ref amount);
        }
    }

    public override int RemoveLastBlock()
    {
        CardObject card = block.Cards.LastOrDefault();
        if (card != null)
        {
            DiscardSingle(card);
            return (card.Info as BlockCardInfo).CurrentBlock;
        }

        return 0;
    }

    public override int RemoveAllBlock()
    {
        int block = Block;
        foreach (var card in this.block.Cards)
            DiscardSingle(card);

        return block;
    }

    #endregion Defend

    #region Discard

    public void DiscardSingle(CardObject card, bool isTribute = false)
    {
        if (isTribute && card.Info is not TokenCardInfo || !card.Info.DestroyOnDiscard)
        {
            discard.Add(card);
            card.Info.Clear();
        }
        else
            DestroyCard(card);
    }

    public void DiscardHand()
    {
        foreach (var item in hand.Cards.Where(x => x.Info.DestroyOnDiscard && x.Info is TokenCardInfo))
            DestroyCard(item);

        hand.MoveAllTo(discard);
    }

    public void DestroyCard(CardObject card)
    {
        if (card.Collection != null)
            card.Collection.Remove(card);
        GameObject.Destroy(card.gameObject);

        if (card.Info.Set == CardSet.Item)
        {
            //Remove item from inventory
            List<string> cards = new List<string>(UserFile.SaveGame.DeckCardEntries);
            cards.Remove(card.Info.Name);
            UserFile.SaveGame.DeckCardEntries = cards.ToArray();
        }
    }

    #endregion Discard

    #region Dice

    public void AddDie(DieInfo info, bool rollDie = true)
    {
        DieObject die = DieObject.Instantiate(info, new Vector2(-1000, -1000));
        dice.Add(die);
        die.Player = this;
        if (rollDie)
            die.Roll();
    }

    public void RollDice()
    {
        //Destroy old dice
        dice.Clear();

        if (diceAmount > 0)
        {
            //Calculate die values
            int upgradeIndex = dicePower % diceAmount;
            int upgradePower = dicePower / diceAmount;

            //Create new dice
            for (int i = 0; i < diceAmount; i++)
            {
                int dieIndex = upgradePower;
                if (i < upgradeIndex)
                    dieIndex += 1;
                dieIndex = Mathf.Min(dieIndex, diceValues.Length - 1);

                AddDie(new DieInfo(diceValues[dieIndex]));
            }
        }
    }

    #endregion Dice

    public override void AddStatus(StatusEffect effect, int count)
    {
        base.AddStatus(effect, count);

        if (bleedSalt > 0 && effect == StatusEffect.Bleeding)
        {
            int amount = bleedSalt;
            if (StrengthBleedSalt)
                amount += Strenght;
            bleedSalt = -1;
            AddStatus(StatusEffect.Bleeding, amount);
            bleedSalt = amount;
        }
    }

    public override int GetStatus(StatusEffect effect)
    {
        int bonus = 0;
        if (effect != StatusEffect.FragileStrength && effect != StatusEffect.FragileDefence)
            bonus = this.block.Cards.Where(x => x.Info is BlockCardInfo blocInfo && blocInfo.StatusMod.Item1 == effect).Sum(x => (x.Info as BlockCardInfo).StatusMod.Item2);

        return base.GetStatus(effect) + bonus;
    }


    protected override void OnAdvanceRound()
    {
        //Reset card play history
        playedCards = new List<CardInfo>();
        //Reset locked card types
        lockedCardTypes = new bool[6];

        //Execute queued round start events

        //Execute passive block effects
        PassiveBlockCardInfo[] blockCards = block.Cards.Select(x => x.Info as PassiveBlockCardInfo).ToArray();
        for (int i = 0; i < blockCards.Length; i++)
        {
            if (blockCards[i] != null)
                blockCards[i].OnRoundStart(Manager);
        }

        //Execute permanent round start effects
        GameEvent[] roundStartEvents = GetActionEffects(PermanentEffect.OnRoundStart).Cast<GameEvent>().ToArray();
        for (int i = 0; i < roundStartEvents.Length; i++)
        {
            roundStartEvents[i](Manager);
        }

        //Reset passive Effects
        passiveEffects = new Delegate[][]
        {
            new Delegate[] {},
            new Delegate[] {},
            new Delegate[] {},
            new Delegate[] {},
            new Delegate[] {}
        };
    }
}