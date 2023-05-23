using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
        player = new Player(this);
        enemy = new Enemy(this, 40, EnemyBehavior.Tactical);

        CardInfo[] deck = cards.Where(x => x.Tier == 0 && x.Set != CardSet.Health && x.Set != CardSet.Item).ToArray();

        for (int i = 0; i < deck.Length; i++)
            player.Deck.Add(CardObject.Instantiate((CardInfo)deck[i].Clone(), this.deck.transform.position));

        player.Deck.Shuffle();
        EndRound();
    }

    private void Update()
    {
        if (player.Health > 0 && enemy.Health > 0)
        {
            player.Update();

            if (playerStats != null)
                playerStats.text = player.ToString();

            if (enemyStats != null)
                enemyStats.text = enemy.ToString();

            if (!player.CanPlayCards())
                EndRound();
        }
        else
        {
            playerStats.text = "";
            if (player.Health > 0)
                enemyStats.text = "You Win";
            else
                enemyStats.text = "You Lost";
        }
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

    public void CreateLanugageFileTemplate()
    {
        CardLibrary.CreateLanugageFile();
    }
}