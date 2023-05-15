using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

public static class CardLibrary
{
    #region Fields

    private const string TRIDENT_MAIN_CARD_NAME = "Stoß";

    private static readonly CardInfo[] cards;
    private static readonly CardInfo[] specialCards;

    private static XmlDocument translationDoc;

    #endregion Fields

    static CardLibrary()
    {
        cards = new CardInfo[]
        {
            #region Trident

            #region Tier 0

            //Stoß - Schaden entsprechend Augenzahl.
            new InstantCardInfo("Stoß", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower)),

            //Parade - Block entsprechend Augenzahl.
            new BlockCardInfo("Parade", CardSet.Trident, CardType.Block, 1,
            (CardInfo c) => c.DicePower),

            //Schub - Entferne die letzte geg. Verteidigungskarte.
            new InstantCardInfo("Schub", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) =>  { int damage = c.Enemy.BlockStack.Length > 0 ? c.Enemy.BlockStack.Last() : 0; if (damage > 0) c.Enemy.ReduceBlock(ref damage); }),

            #endregion Tier 0

            #region Tier 1

            //Dreistoß - 3 Angriffe. Je Angriff: Schaden entsprechend Augenzahl.
            new InstantCardInfo("Dreistoß", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) => { for(int i = 0; i < 3; i++) c.Player.Attack(c.Enemy, c.DicePower); }),

            //Sprungstoß - Schaden entsprechend doppelter Augenzahl. Füge 2 Schwach zu.
            new InstantCardInfo("Sprungstoß", CardSet.Trident, CardType.Attack, 2,
            (CardInfo c) => { c.Player.Attack(c.Enemy, c.DicePower * 2); c.Enemy.AddStatus(StatusEffect.Weak, 2); }),

            //Wegstoßen - Betäubung entsprechend Augenzahl.
            new InstantCardInfo("Wegstoßen", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower)),

            //Nachsetzen - Schaden entsprechend geg. Betäubungsstapel. Ziehe 1 Karte.
            new InstantCardInfo("Nachsetzen", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.Enemy.GetStatus(StatusEffect.Stun)); c.Player.DrawCards(1); }),

            #endregion Tier 1

            #region Tier 2

            //Belagerungsangriff - Permanent: Entferne die Kosten für alle "Stoß" Karten. Der Grund-Schaden von "Stoß" Karten wird zu der für diese Karte aufgewendeten Augenzahl.
            new PermanentCardInfo("Belagerungsangriff", CardSet.Trident, CardType.Skill, 2,
            (CardInfo c, int v) => {if (c.Name.Equals(TRIDENT_MAIN_CARD_NAME)) return new InstantCardInfo(TRIDENT_MAIN_CARD_NAME, CardSet.Trident, CardType.Attack, 0, (CardInfo nc) => nc.Player.Attack(nc.Enemy, v)); return null; }),

            //Wuchtiger Schwung - Schaden entsprechend Augenzahl. Betäubung entsprechend Augenzahl.
            new InstantCardInfo("Wuchtiger Schwung", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower); }),

            //Ein-Mann-Phalanx - Permanent: Jedesmal wenn durch eine "Stoß" Karte Schaden verursacht wird, erhalte 1 Stärke.
            new PermanentCardInfo("Ein-Mann-Phalanx", CardSet.Trident, CardType.Skill, 2,
            (CardInfo c) => {if (c.Name.Equals(TRIDENT_MAIN_CARD_NAME) && c.DicePower > 0 && c.DicePower > c.Enemy.Block) c.Player.AddStatus(StatusEffect.Strenght, 1); }, true),

            //Schluss Stich - 10 Schaden. Die Kosten dieser Karte sinken um 1 für jede "Stoß" Karte, die diese Runde gespielt wurden.
            new InstantCardInfo("Schluss Stich", CardSet.Trident, CardType.Attack, 2,
            (CardInfo c) => c.Player.Attack(c.Enemy, 10)) {CostReduction = (CardInfo c) => c.Player.PlayedCards.Count(x => x.Name.Equals(TRIDENT_MAIN_CARD_NAME))},

            #endregion Tier 2

            #region Tier 3

            //Krönende Spitze - Erzeuge "Stoß" Karten auf die Hand entsprechend der Augenzahl.
            new InstantCardInfo("Krönende Spitze", CardSet.Trident, CardType.Skill, 1,
            (CardInfo c) => {for(int i = 0; i < c.DicePower; i++) c.Player.Hand.Add(CardObject.Instantiate((CardInfo)cards[0].Clone(), Vector2.zero)); }),

            #warning TODO
            //Fels in der Brandung - Wirf eine beliebige Anzahl an Handkarten ab. Block entsprechend Augenzahl multipliziert mit abgeworfenen Handkarten.
            new BlockCardInfo("Fels in der Brandung", CardSet.Trident, CardType.Block, 1,
            (CardInfo c) => c.DicePower * 1),

            //Blitzangriff -  Nimm alle "Stoß" Karten aus dem Ablagestapel auf die Hand.
            new InstantCardInfo("Blitzangriff", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) => { foreach (var card in c.Player.Discard.Cards.Where(x => x.Info.Name.Equals(TRIDENT_MAIN_CARD_NAME))) c.Player.Hand.Add(card); }),

            //Tosende Dominanz - 12 Schaden. Füge Schwach entsprechend der Augenzahl zu.
            new InstantCardInfo("Tosende Dominanz", CardSet.Trident, CardType.Attack, 3,
            (CardInfo c)=> { c.Player.Attack(c.Enemy, 12); c.Enemy.AddStatus(StatusEffect.Weak, c.DicePower);}),

            #endregion Tier 3

            #region Tier 4

            #warning TODO
            //Strahlstrom - 8 Schaden. Ziehe Karten entsprechend der Augenzahl, wähle einen Angriff aus den gezogenen Karten und aktiviere ihn, nutze die selbe Augenzahl wie für die Aktivierung dieser Karte. Wirf die anderen gezogenen Karten ab.
            new InstantCardInfo("Strahlstrom", CardSet.Trident, CardType.Attack, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, 8); }),

            //Leviathanischer Zorn - Permanent: Ziehe jedesmal eine Karte wenn du einen Angriff aktivierst.
            new PermanentCardInfo("Leviathanischer Zorn", CardSet.Trident, CardType.Skill, 1,
            (CardInfo c) => { if (c.Type == CardType.Attack) c.Player.DrawCards(1); }, true),

            //Donnernde Herausforderung - 60 Schaden. Dieser Angriff macht keinen Schaden gegen Gegner ohne Block. Wenn dieser Angriff den geg. Block komplett bricht, Betäubung entsprechend Augenzahl.
            new InstantCardInfo("Donnernde Herausforderung", CardSet.Trident, CardType.Attack, 3,
            (CardInfo c) => { if (c.Enemy.Block > 0) { c.Player.Attack(c.Enemy, 60); if (!(c.Enemy.Block > 0)) c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower); } }),

            //Ruhe vor dem Sturm - 1 Block. Solange diese Karte im Block-Stapel aktiv ist, füge dem Gegner Betäubung entsprechend der Augenzahl zu, wenn er angreift.
            new PassiveBlockCardInfo("Ruhe vor dem Sturm", CardSet.Trident, CardType.Block, 1,
            (CardInfo c) => 1, (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower)),

            #endregion Tier 4

            #region Tier 5

            //Donnerndes Gericht - Erzeuge 4 "Stoß" Karten. Nimm alle "Stoß" Karten aus deinem Ablagestapel und  deinem Stapel auf die Hand.
            new InstantCardInfo("Donnerndes Gericht", CardSet.Trident, CardType.Skill, 4,
            (CardInfo c) => {for(int i = 0; i < 4; i++) c.Player.Hand.Add(CardObject.Instantiate((CardInfo)cards[0].Clone(), Vector2.zero)); CardObject[] tmCards = c.Player.Deck.Cards.Concat(c.Player.Discard.Cards).Where(x => x.Info.Name.Equals(TRIDENT_MAIN_CARD_NAME)).ToArray(); for (int i = 0; i < tmCards.Length; i++) c.Player.Hand.Add(tmCards[i]); }),

            //Jupiters Dorn - 4 Schaden multipliziert mit geg. Betäubung.
            new InstantCardInfo("Jupiters Dorn", CardSet.Trident, CardType.Attack, 4,
            (CardInfo c) => c.Player.Attack(c.Enemy, 4 * c.Enemy.GetStatus(StatusEffect.Stun))),

            #warning TODO
            //Neptuns Gericht - Permanent: Zu Rundenbeginn wirf einen 12-Würfel, erhalte,bis zum Rundenende, einen zusätzlichen Würfel und Stärke in Höhe dieses Würfels.
            new PermanentCardInfo("Neptuns Gericht", CardSet.Trident, CardType.Skill, 4,
            (CardGameManager m) => { m.Player.AddDie(new DieInfo(12)); }),

            //Kataklysmus - X Angriffe, entsprechen Augenzahl. Je Angriff: 4 Schaden und 1 Schwäche.
            new InstantCardInfo("Kataklysmus", CardSet.Trident, CardType.Attack, 4,
            (CardInfo c) => { for(int i = 0; i < c.DicePower; i++)  { c.Player.Attack(c.Enemy, 4); c.Enemy.AddStatus(StatusEffect.Weak, 1); } }),

            #endregion Tier 5

            #endregion Trident
        };

        specialCards = new CardInfo[]
        {
            //Flammendes Verlangen - Diese Karte kann nicht gespielt werden.
            new TokenCardInfo("Flammendes Verlangen", CardSet.Gladius, CardType.Skill),

            //Messer´s Schneide - 1 Schaden.
            new InstantCardInfo("Messer´s Schneide", CardSet.Pugio, CardType.Attack, 0,
            (CardInfo c) => c.Player.Attack(c.Enemy, 1)),

            //Linker Haken - 2 Verwundbarkeit.
            new InstantCardInfo("Linker Haken", CardSet.Cestus, CardType.Attack, 0,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Vulnerable, 2)),

            //Rechter Haken - 3 Schaden.
            new InstantCardInfo("Rechter Haken", CardSet.Cestus, CardType.Attack, 0,
            (CardInfo c) => c.Player.Attack(c.Enemy, 3)),

            //Verletzt - Diese Karte ist unspielbar. Nach einem Kampf oder wenn diese Karte in den Ablagestapel gelegt wird, entferne sie permanent aus dem Stapel.
            new TokenCardInfo("Verletzt", CardSet.Health, CardType.Ailment, true),
        };
    }

    #region Properties

    public static CardInfo[] Cards => cards;

    public static CardInfo[] SpecialCards => specialCards;

    #endregion Properties

    public static void Setup()
    {
        if (translationDoc == null)
            LoadLanguage(Language.German);
    }

    public static void LoadLanguage(Language language)
    {
        translationDoc = new XmlDocument();
        translationDoc.LoadXml(Resources.Load<TextAsset>($"XML/Languages/{language.ToString()}").text);
    }

    public static int GetIndexOfName(string name)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].Name.Equals(name))
                return i;
        }

        return -1;
    }

    public static XmlNode GetTranslationNode(string name)
    {
        int index = GetIndexOfName(name);

        if (index > -1)
            return translationDoc.FirstChild.ChildNodes[index];

        return null;
    }

    public static string GetTranslatedName(string name)
    {
        XmlNode node = GetTranslationNode(name);
        if (node != null)
            return node.Attributes["Name"].Value;

        return name;
    }

    public static string GetTranslatedDescription(string name)
    {
        XmlNode node = GetTranslationNode(name);
        if (node != null)
            return node.Attributes["Description"].Value;

        return "";
    }
}