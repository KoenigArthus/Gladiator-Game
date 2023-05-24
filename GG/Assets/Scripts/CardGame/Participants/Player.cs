using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using System;
using Yarn.Unity;
using UnityEngine.Android;
using System.Threading.Tasks;

public class Player : Participant
{
    #region Fields

    private static readonly int[] diceValues = new int[] { 4, 6, 8, 12, 20 };

    private CardCollection deck;
    private CardCollection hand;
    private CardCollection block;
    private CardCollection discard;

    private DieObject[] dice = new DieObject[10];
    private int diceAmount = 4;
    private int dicePower = 0;

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
        new Delegate[] {}
    };

    private Queue<GameEvent> eventQueue = new Queue<GameEvent>();

    #endregion Fields

    public Player(CardGameManager manager) : base(manager)
    {
        this.deck = manager.deck;
        this.hand = manager.hand;
        this.block = manager.block;
        this.discard = manager.discard;

        this.deck.Player = this;
        this.hand.Player = this;
        this.hand.cardsInteractable = true;
        this.block.Player = this;
        this.discard.Player = this;
    }

    #region Properties

    public override int Health { get => UserFile.SaveGame.Health; set => UserFile.SaveGame.Health = value; }
    public override int[] BlockStack => block.Cards.Select(x => (x.Info as BlockCardInfo).CurrentBlock).ToArray();
    public Enemy Enemy => Manager.Enemy;

    public CardCollection Deck => deck;
    public CardCollection Hand => hand;
    public CardCollection Discard => discard;
    public CardCollection ActiveBlock => block;

    public CardInfo[] PlayedCards => playedCards.Where(x => x != null).ToArray();

    public CardObject Prepareing => prepareing;

    public bool Busy => false;

    #endregion Properties

    #region DrawCards

    public void DrawCards(int amount)
    {
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
            hand.Add(drawnCard);

            if (drawnCardInfo.Type == CardType.Ailment && drawnCardInfo.Cost == 0)
            {
                prepareing = drawnCard;
                PlayPreparedCard();
            }
        }
    }

    #endregion DrawCards

    #region Play

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
            ChangeCard[] change = permanentEffects[(int)PermanentEffect.OnDeck].Cast<ChangeCard>().ToArray();
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
            return;

        CardObject card = prepareing;
        CardInfo info = card.Info;
        prepareing = null;

        //Destroy used dice
        card.Info.DestroyDice();

        int stun = GetStatus(StatusEffect.Stun) - playedCards.Count(x => x == null);
        if (stun < 1 || UnityEngine.Random.Range(0, 100) > 9)
        {
            CardAction[] playActions = permanentEffects[(int)PermanentEffect.OnPlay].Cast<CardAction>().ToArray();
            for (int i = 0; i < playActions.Length; i++)
            {
                playActions[i](info);
            }
            info = card.Info;

            playedCards.Add(info);

            //Move card to correct collection
            if (info is InstantCardInfo instantCard)
            {
                instantCard.Execute();
                if (info is BlockCardInfo blockCard)
                {
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
                        return;
                    }
                }
            }
            else if (info is PermanentCardInfo permanentCard)
            {
                PermanentEffect effect = permanentCard.Effect.Item1;
                Delegate action = permanentCard.Effect.Item2;

                if (effect == PermanentEffect.OnDeck)
                {
                    if (action is ChangeCardAction changeCardAction)
                    {
                        int power = permanentCard.DicePower;
                        ChangeCard changeAction = (CardInfo c) => changeCardAction(c, power);
                        action = changeAction;
                    }
                    else
                    {
                        Debug.Log($"Black Magic failed! | Card: {permanentCard.Name} | If you ever see this message, RUN!");
                        return;
                    }
                }
                else if (effect == PermanentEffect.OnRoundStart)
                {
                    if (action is GameEventAction gameEvent)
                    {
                        CardInfo snapshot = permanentCard;
                        GameEvent eventAction = (CardGameManager m) => gameEvent(m, snapshot);
                        action = eventAction;
                    }
                    else
                    {
                        Debug.Log($"Black Magic failed! | Card: {permanentCard.Name} | If you ever see this message, RUN!");
                        return;
                    }
                }

                permanentEffects[(int)effect] = permanentEffects[(int)effect].Concat(new Delegate[] { action }).ToArray();
            }
        }
        else
        {
            playedCards.Add(null);
        }

        //Hand -> Discard
        DiscardSingle(card);
    }

    public void PrepareCard(CardObject card)
    {
        if (card != null && !lockedCardTypes[(int)card.Info.Type])
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
        Debug.Log($"{type} locked");
        lockedCardTypes[(int)type] = true;
    }

    public bool CanPlayCards()
    {
        if (hand.Cards.Length < 1)
            return false;

        int cost = hand.Cards.Min(x => x.Info.Cost);

        return !(dice.Count(x => x != null && !x.IsDestroyed()) < cost);
    }

    public void AddAction(GameEvent gameAction, bool permanent = false)
    {
        if (permanent)
            permanentEffects[(int)PermanentEffect.OnRoundStart] = permanentEffects[(int)PermanentEffect.OnRoundStart].Concat(new Delegate[] { gameAction }).ToArray();
        else
            eventQueue.Enqueue(gameAction);
    }

    #endregion Play

    #region Attack

    public override void OnAttack(Participant attacker, Participant defender, int power)
    {
        if (defender == this)
        {
            AttackEvent[] attackEvents = permanentEffects[(int)PermanentEffect.OnDefend].Cast<AttackEvent>().ToArray();
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
        }
    }

    #endregion Discard

    #region Dice

    public void AddDie(DieInfo info, bool rollDie = true)
    {
        int index = dice.IndexOf(null);

        if (index > -1 && index < dice.Length)
        {
            Vector2 targetPosition = new Vector2(350, 600) - new Vector2(0, 150 * index);
            dice[index] = DieObject.Instantiate(info, targetPosition);
            dice[index].Player = this;
            if (rollDie)
                dice[index].Roll();
        }
    }

    public void RollDice()
    {
        //Destroy old dice
        for (int i = 0; i < dice.Length; i++)
        {
            if (dice[i] != null && !dice[i].IsDestroyed())
                GameObject.Destroy(dice[i].gameObject);

            dice[i] = null;
        }

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

    public override int GetStatus(StatusEffect effect)
    {
        int bonus = 0;
        if (effect != StatusEffect.FragileStrenght && effect != StatusEffect.FragileDefence)
            bonus = this.block.Cards.Where(x => x.Info is BlockCardInfo blocInfo && blocInfo.StatusMod.Item1 == effect).Sum(x => (x.Info as BlockCardInfo).StatusMod.Item2);

        return base.GetStatus(effect) + bonus;
    }

    protected override void OnAdvanceRound()
    {
        playedCards = new List<CardInfo>();
        lockedCardTypes = new bool[6];

        //Execute queued round start events
        while (eventQueue.Count > 0)
            eventQueue.Dequeue()(Manager);

        //Execute passive block effects
        PassiveBlockCardInfo[] blockCards = block.Cards.Select(x => x.Info as PassiveBlockCardInfo).ToArray();
        for (int i = 0; i < blockCards.Length; i++)
        {
            if (blockCards[i] != null)
                blockCards[i].OnRoundStart(Manager);
        }

        //Execute permanent round start effects
        GameEvent[] roundStartEvents = permanentEffects[(int)PermanentEffect.OnRoundStart].Cast<GameEvent>().ToArray();
        for (int i = 0; i < roundStartEvents.Length; i++)
        {
            roundStartEvents[i](Manager);
        }
    }
}