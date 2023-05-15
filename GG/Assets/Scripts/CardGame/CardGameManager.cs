using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardGameManager : MonoBehaviour
{
    #region Fields

    public static CardInfo[] cards = CardLibrary.Cards;

    public Text playerStats;
    public Text enemyStats;

    public CardCollection deck;
    public CardCollection hand;
    public CardCollection block;
    public CardCollection discard;

    private Player player;
    private Enemy enemy;

    private int drawAmount = 4;

    #endregion Fields

    #region Properties

    public Player Player => player;
    public Enemy Enemy => enemy;

    #endregion Properties

    #region Main-Loop

    private void Awake()
    {
        CardLibrary.Setup();
    }

    private void Start()
    {
        player = new Player(this, 20, 3);
        enemy = new Enemy(this, 40, 5, EnemyBehavior.Tactical);

        CardInfo[] deck;

        switch (1)
        {
            case 1:
                deck = cards;
                break;

            case 2:
                deck = new CardInfo[30].Select(x => cards[Random.Range(0, cards.Length)]).ToArray();
                break;

            case 3:
                deck = new CardInfo[]
                {
                    cards[1],
                    cards[1],
                    cards[1],
                    cards[1],
                    cards[1],
                    cards[1],
                    cards[1],
                    cards[1],
                    cards[1],
                    cards[1],
                };
                break;
        }

        for (int i = 0; i < deck.Length; i++)
            player.Deck.Add(CardObject.Instantiate((CardInfo)deck[i].Clone(), this.deck.transform.position));

        player.Deck.Shuffle();
        EndRound();
    }

    private void Update()
    {
        player.Update();

        if (playerStats != null)
            playerStats.text = player.ToString();

        if (enemyStats != null)
            enemyStats.text = enemy.ToString();
    }

    #endregion Main-Loop

    public void DrawCards(int amount)
    {
        player.DrawCards(amount);
    }

    public void Abort()
    {
        player.AbortPreparedCardPlay();
    }

    public void EndRound()
    {
        player.AbortPreparedCardPlay();
        player.DiscardHand();

        enemy.TakeTurn(player);
        enemy.ChangeIntension();

        player.RollDice();
        player.DrawCards(drawAmount);

        player.AdvanceRound();
        enemy.AdvanceRound();
    }
}