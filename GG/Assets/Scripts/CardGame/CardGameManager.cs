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

    public static CardInfo[] cards = new CardInfo[]
    {
        new InstantCardInfo("Stoß", CardSet.Trident, CardType.Attack, 1, (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower)),
        new BlockCardInfo("Parade", CardSet.Trident, CardType.Block, 1, (CardInfo c) => c.DicePower),
        new InstantCardInfo("Schub", CardSet.Trident, CardType.Attack, 1, (CardInfo c) => c.Player.Attack(c.Enemy, c.Enemy.BlockStack.Length > 0 ? c.Enemy.BlockStack.Last() : 0)),

        new InstantCardInfo("Dreistoß", CardSet.Trident, CardType.Attack, 1, (CardInfo c) => { for(int i = 0; i < 3; i++) c.Player.Attack(c.Enemy, c.DicePower); }),
        new InstantCardInfo("Sprungstoß", CardSet.Trident, CardType.Attack, 2, (CardInfo c) => { c.Player.Attack(c.Enemy, c.DicePower * 2); c.Enemy.AddStatus(StatusEffect.Weak, 2); }),
        new InstantCardInfo("Wegstoßen", CardSet.Trident, CardType.Attack, 1, (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower)),
        new InstantCardInfo("Nachsetzen", CardSet.Trident, CardType.Attack, 1, (CardInfo c) => {c.Player.Attack(c.Enemy, c.Enemy.GetStatus(StatusEffect.Stun)); c.Player.DrawCards(1); }),

        //Belagerungsangriff
        new InstantCardInfo("Wuchtiger Schwung", CardSet.Trident, CardType.Attack, 1, (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower); }),
        //Ein-Mann-Phalanx
        new InstantCardInfo("Schluss Stich", CardSet.Trident, CardType.Attack, -2, (CardInfo c) => c.Player.Attack(c.Enemy, 10)), //TODO Reduce Cost

        new InstantCardInfo("Krönende Spitze", CardSet.Trident, CardType.Skill, 1, (CardInfo c) => {for(int i = 0; i < c.DicePower; i++) c.Player.Hand.Add(CardObject.Instantiate((CardInfo)cards[0].Clone(), Vector2.zero)); }),
    };

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

    public Player Player => player;
    public Enemy Enemy => enemy;

    #region Main-Loop

    private void Start()
    {
        player = new Player(this, 20, 3);
        enemy = new Enemy(this, 40, 5, EnemyBehavior.Tactical);

        for (int j = 0; j < 30; j++)
            player.Deck.Add(CardObject.Instantiate((CardInfo)cards[Random.Range(0, cards.Length)].Clone(), deck.transform.position));

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
    }
}