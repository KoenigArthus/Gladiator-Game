using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;

public class Player : Participant
{
    #region Fields

    private static readonly int[] diceValues = new int[] { 4, 6, 8, 12, 20 };

    private CardCollection deck;
    private CardCollection hand;
    private CardCollection block;
    private CardCollection discard;

    private CardObject prepareing = null;

    private DieObject[] dice = new DieObject[5];
    private int diceAmount = 3;
    private int dicePower = 0;

    #endregion Fields

    public Player(CardGameManager manager, int health, int blockSlots) : base(manager, health, blockSlots)
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

    public override int[] BlockStack => block.Cards.Select(x => (x.Info as BlockCardInfo).CurrentBlock).ToArray();
    public Enemy Enemy => Manager.Enemy;

    public CardCollection Deck => deck;
    public CardCollection Hand => hand;

    public CardObject Prepareing => prepareing;

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

            hand.Add(deck.DrawCard());
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
    }

    protected void PlayPreparedCard()
    {
        CardObject card = prepareing;
        prepareing = null;

        //Destroy used dice
        card.Info.DestroyDice();

        //Move card to correct collection
        if (card.Info is InstantCardInfo instantCard)
        {
            instantCard.Execute();

            //Hand -> Discard
            DiscardSingle(card);
        }
        else if (card.Info is PermanentCardInfo permanentCard)
        {
            if (permanentCard is BlockCardInfo blockCard)
            {
                //Hand -> Block
                block.Add(card);

                //Discard if block is overfilled
                if (block.Count > BlockSlots)
                    DiscardSingle(block.Cards[0]);
            }
            else
            {
                //Hand -> Permanent
            }
        }
    }

    public void PrepareCard(CardObject card)
    {
        if (card != null)
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

    #endregion Play

    #region Defend

    protected override void ReduceBlock(ref int amount)
    {
        BlockCardInfo[] blockCards = block.Cards.Select(x => x.Info as BlockCardInfo).ToArray();
        for (int i = 0; i < blockCards.Length; i++)
        {
            if (amount > 0)
                blockCards[i].ApplyDamage(ref amount);
            else
                return;
        }
    }

    #endregion Defend

    #region Discard

    public void DiscardSingle(CardObject card)
    {
        if (!card.Info.DestroyOnDiscard)
        {
            discard.Add(card);
            card.Info.Clear();
        }
        else
        {
            if (card.Collection != null)
                card.Collection.Remove(card);
            GameObject.Destroy(card.gameObject);
        }
    }

    public void DiscardHand()
    {
        hand.MoveAllTo(discard);
    }

    #endregion Discard

    #region Dice

    public void AddDie(DieInfo info)
    {
        int index = dice.IndexOf(null);

        if (index > -1 && index < dice.Length)
        {
            Vector2 targetPosition = new Vector2(350, 600) - new Vector2(0, 150 * index);
            dice[index] = DieObject.Instantiate(info, targetPosition);
            dice[index].Player = this;
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
}