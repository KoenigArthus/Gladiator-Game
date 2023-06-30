using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using JSAM;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

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
    public DieCollection dice;
    public List<UIValues> uiChanges = new List<UIValues>();
    public UnityEvent uiHasChanged;


    private Player player;
    private Enemy enemy;

    private int drawAmount = 4;

    private bool battleEnded = false;


    #endregion Fields

    #region Properties

    public Player Player => player;
    public Enemy Enemy => enemy;
    

    #endregion Properties

    #region Main-Loop

    private void Awake()
    {
        CardLibrary.Setup();
        player = new Player(this);

        //Select Opponent
        int nextOpponent = UserFile.SaveGame.NextOpponent;
        if (nextOpponent > 0)
        {
            //Special Fight
            enemy = new Enemy(this, Mathf.Max(100, 150 * nextOpponent), EnemyBehavior.Aggressive);
        }
        else
        {
            //Normal/Random Fight
            enemy = new Enemy(this, Mathf.Max(50, 70 * -nextOpponent), EnemyBehavior.Defensive);
        }

        //CardSet[] sets = new CardSet[] { CardSet.Gladius, CardSet.Scutum, CardSet.Cassis, CardSet.Manica, CardSet.Ocrea };
        CardInfo[] deck = CardLibrary.GetCardsByNames(UserFile.SaveGame.DeckCardEntries); //cards.Where(x => x.Tier == 0 && sets.Contains(x.Set)).ToArray();

        for (int i = 0; i < deck.Length; i++)
            player.Deck.Add(CardObject.Instantiate((CardInfo)deck[i].Clone(), this.deck.transform.position));

        player.cardAnimations = FindObjectOfType<CardAnimations>();
        player.camFocus = FindObjectOfType<Focus>();

        player.Deck.Shuffle();
        EndRound();
        NotifyStatsChange();
    }

    private void Start()
    {
      
    }

    private void Update()
    {
        //Stop updateing after battle
        if (battleEnded) return;

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
            UnityEngine.Debug.Log(enemy.Health);
            UnityEngine.Debug.Log(player.Health);
            //Give Rewards
            if (player.Health > 0)
                UserFile.SaveGame.Gold += 100 * Mathf.Abs(UserFile.SaveGame.NextOpponent);

            //Go back to overwolrd
            battleEnded = true;
            SceneManager.LoadScene("Ludus");
        }
    }

    #endregion Main-Loop

    #region UI

    public void NotifyStatsChange()
    {
        uiChanges.Add(new UIValues(player, enemy));
        uiHasChanged?.Invoke();

    }

    /* public UIValues[] CollectUIChanges()
     {
         UIValues[] changes =  uiChanges.ToArray();
         uiChanges.Clear();

         return changes;
     }
     public UIValues[] CollectUIChanges(bool clearValues)
     {
         UIValues[] changes = uiChanges.ToArray();

         if (clearValues)
         uiChanges.Clear();

         return changes;
     }*/


    #endregion UI

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

        player.AddDie(new DieInfo(4));
        player.AddDie(new DieInfo(6));
        player.AddDie(new DieInfo(8));
        player.AddDie(new DieInfo(10));
        player.AddDie(new DieInfo(12));
        player.AddDie(new DieInfo(20));
    }

    public void Debug()
    {
        CardLibrary.CreateLanugageFile();

        //CardLibrary.RenameCardImages(cards);
    }
}