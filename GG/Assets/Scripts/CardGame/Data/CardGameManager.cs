using System.Collections.Generic;
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
    public DieCollection dice;

    private Player player;
    private Enemy enemy;

    private int round = 0;
    private bool battleEnded = false;

    private List<UIValues> uiChanges = new List<UIValues>();

    #endregion Fields

    #region Properties

    public Player Player => player;
    public Enemy Enemy => enemy;

    public int Round => round;

    #endregion Properties

    #region Main-Loop

    private void Awake()
    {
        CardLibrary.Setup();
    }

    private void Start()
    {
        player = new Player(this);

        //Select Opponent
        int nextOpponent = UserFile.SaveGame.NextOpponent;
        enemy = new Enemy(this, nextOpponent);

        //CardSet[] sets = new CardSet[] { CardSet.Gladius, CardSet.Scutum, CardSet.Cassis, CardSet.Manica, CardSet.Ocrea };
        CardInfo[] deck = CardLibrary.GetCardsByNames(UserFile.SaveGame.DeckCardEntries); //cards.Where(x => x.Tier == 0 && sets.Contains(x.Set)).ToArray();

        for (int i = 0; i < deck.Length; i++)
            player.Deck.Add(CardObject.Instantiate((CardInfo)deck[i].Clone(), this.deck.transform.position));

        player.cardAnimations = FindObjectOfType<CardAnimations>();

        player.Deck.Shuffle();
        EndRound();
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
            battleEnded = true;

            UnityEngine.Debug.Log(enemy.Health);
            UnityEngine.Debug.Log(player.Health);

            //Give Rewards
            enemy.Reward.ApplyRewards(
                //If
                player.Health > 0 ?
                    //If
                    enemy.Health > 0 ?
                        BattleResult.Spare :
                        //Else
                        BattleResult.Win :
                    //Else
                    BattleResult.Lose);

            //Go back to overwolrd
            LevelLoader.i.LoadScene("Ludus");
        }
    }

    #endregion Main-Loop

    #region UI

    public void NotifyStatsChange()
    {
        uiChanges.Add(new UIValues(player, enemy));
    }

    public UIValues[] CollectUIChanges()
    {
        UIValues[] changes = this.uiChanges.ToArray();
        this.uiChanges.Clear();

        return changes;
    }

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
        //Round End

        player.AbortPreparedCardPlay();
        player.DiscardHand();

        enemy.TakeTurn(player);

        round += 1;

        //Round Start

        enemy.ChangeIntension();

        player.RollDice();
        player.DrawCards();

        player.AdvanceRound();
        enemy.AdvanceRound();
    }

    public void Debug()
    {
        CardLibrary.CreateLanugageFile();

        //CardLibrary.RenameCardImages(cards);
    }
}