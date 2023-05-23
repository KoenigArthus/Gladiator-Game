using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public static class CardLibrary
{
    #region Fields

    private const string TRIDENT_MAIN_CARD_NAME = "Thrust";

    private static readonly CardInfo[] cards;

    private static XmlDocument translationDoc;
    private static Language loadedLanguage = Language.English;

    #endregion Fields

    static CardLibrary()
    {
        cards = new CardInfo[]
        {
            #region Weapons

            #region Trident

            #region Tier 0

            //Stoß - Schaden entsprechend Augenzahl.
            new InstantCardInfo("Thrust", CardSet.Trident, CardType.Attack, 0, 1,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower)),

            //Schub - Entferne die letzte geg. Verteidigungskarte.
            new InstantCardInfo("Shove", CardSet.Trident, CardType.Attack, 0, 1,
            (CardInfo c) => c.Enemy.RemoveLastBlock()),

            //Sprungstoß - Schaden entsprechend doppelter Augenzahl. Füge 2 Schwach zu.
            new InstantCardInfo("Jump_Thrust", CardSet.Trident, CardType.Attack, 0, 2,
            (CardInfo c) => { c.Player.Attack(c.Enemy, c.DicePower * 2); c.Enemy.AddStatus(StatusEffect.Weak, 2); }),

            #endregion Tier 0

            #region Tier 1

            //Wuchtiger Schwung - Schaden entsprechend Augenzahl. Betäubung entsprechend Augenzahl.
            new InstantCardInfo("Powerful_Sweep", CardSet.Trident, CardType.Attack, 1, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower); }),

            //Dreistoß - 3 Angriffe. Je Angriff: Schaden entsprechend Augenzahl.
            new InstantCardInfo("Triple_Thrust", CardSet.Trident, CardType.Attack, 1, 1,
            (CardInfo c) => { for(int i = 0; i < 3; i++) c.Player.Attack(c.Enemy, c.DicePower); }),

            #warning TODO
            //Fels in der Brandung - Wirf eine beliebige Anzahl an Handkarten ab. Block entsprechend Augenzahl multipliziert mit abgeworfenen Handkarten.
            new BlockCardInfo("Rock_Solid", CardSet.Trident, 1, 1,
            (CardInfo c) => c.DicePower * 1),

            //Donnerndes Gericht - Erzeuge 4 "Stoß" Karten. Nimm alle "Stoß" Karten aus deinem Ablagestapel und  deinem Stapel auf die Hand.
            new InstantCardInfo("Thundering_Judgment", CardSet.Trident, CardType.Skill, 1, 2,
            (CardInfo c) => {for(int i = 0; i < 4; i++) c.Player.Hand.Add(CardObject.Instantiate((CardInfo)cards[0].Clone(), Vector2.zero)); CardObject[] tmCards = c.Player.Deck.Cards.Concat(c.Player.Discard.Cards).Where(x => x.Info.Name.Equals(TRIDENT_MAIN_CARD_NAME)).ToArray(); for (int i = 0; i < tmCards.Length; i++) c.Player.Hand.Add(tmCards[i]); }),

            #endregion Tier 1

            #region Tier 2

            //Kataklysmus - X Angriffe, entsprechen Augenzahl. Je Angriff: 4 Schaden und 1 Schwäche.
            new InstantCardInfo("Cataclysm", CardSet.Trident, CardType.Attack, 2, 2,
            (CardInfo c) => { for(int i = 0; i < c.DicePower; i++)  { c.Player.Attack(c.Enemy, 4); c.Enemy.AddStatus(StatusEffect.Weak, 1); } }),

            //Schluss Stich - 10 Schaden. Die Kosten dieser Karte sinken um 1 für jede "Stoß" Karte, die diese Runde gespielt wurden.
            new InstantCardInfo("Finishing_Stab", CardSet.Trident, CardType.Attack, 2, 2,
            (CardInfo c) => c.Player.Attack(c.Enemy, 10))
            {CostReduction = (CardInfo c) => c.Player.PlayedCards.Count(x => x.Name.Equals(TRIDENT_MAIN_CARD_NAME))},

            //Krönende Spitze - Erzeuge "Stoß" Karten auf die Hand entsprechend der Augenzahl.
            new InstantCardInfo("Royal_Tip", CardSet.Trident, CardType.Skill,2,  1,
            (CardInfo c) => {for(int i = 0; i < c.DicePower; i++) c.Player.Hand.Add(CardObject.Instantiate((CardInfo)cards[0].Clone(), Vector2.zero)); }),

            //Neptuns Gericht - Permanent: Zu Rundenbeginn wirf einen 12-Würfel, erhalte,bis zum Rundenende, einen zusätzlichen Würfel und Stärke in Höhe dieses Würfels.
            new PermanentCardInfo("Neptuns_Judgment", CardSet.Trident, 2, 4,
            (CardGameManager m, CardInfo c) => { var bonusDie = new DieInfo(12); m.Player.AddDie(bonusDie);
            Task.Run(async () => {while(bonusDie.Rolling) await Task.Delay(42); int amount = bonusDie.Value; m.Player.AddStatus(StatusEffect.FragileStrenght, amount); }); }),

            #endregion Tier 2

            #region Tier 3

            //Leviathanischer Zorn - Permanent: Entferne die Kosten von allen "Stoß" Karten. Der Grund-Schaden von "Stoß" Karten wird zu der für diese Karte aufgewendeten Augenzahl.
            new PermanentCardInfo("Leviathan_Fury", CardSet.Trident, 3, 3,
            (CardInfo c, int v) => {if (c.Name.Equals(TRIDENT_MAIN_CARD_NAME))
                    return new InstantCardInfo(TRIDENT_MAIN_CARD_NAME, CardSet.Trident, CardType.Attack, 0, 0, (CardInfo nc) => nc.Player.Attack(nc.Enemy, v)); return null; }),

            //Belagerungsangriff - Permanent: Ziehe jedes mal 1 Karte, wenn du eine "Stoß" Karte aktivierst.
            new PermanentCardInfo("Siege_Attack", CardSet.Trident, 3, 4,
            (CardInfo c) => { if (c.Name.Equals(TRIDENT_MAIN_CARD_NAME)) c.Player.DrawCards(1); }, true),

            #endregion Tier 3

            #endregion Trident

            #region Gladius

            #region Tier 0

#warning TODO
            //Stich - Schaden entsprechend Augenzahl. Diese Runde: Andere Angriffe fügen zusätzlich 3 Verwundbarkeit zu.
            new InstantCardInfo("Sting", CardSet.Gladius, CardType.Attack, 0, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Enemy.AddStatus(StatusEffect.Vulnerable, 3); }),

            //Mächtiger Schwung - Schaden entsprechend Augenzahl, multipliziert mit 2.
            new InstantCardInfo("Mighty_Sweep", CardSet.Gladius, CardType.Attack, 0, 2,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * 2)),

            //Knaufschlag - Schaden entsprechend Augenzahl. Ziehe 1 Karte, Ziehe 2 Karten wenn diese Karte der erste gespielte Angriff in dieser Runde ist.
            new InstantCardInfo("Pommel_Strike", CardSet.Gladius, CardType.Attack, 0, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Player.DrawCards(c.Player.PlayedCards.Count(x => x.Type == CardType.Attack) < 2 ? 2 : 1); }),

            #endregion Tier 0

            #region Tier 1

            //Sprung Hieb - 16 Schaden. Erzeuge 2 "Verletzt" Karten in deinen Stapel.
            new InstantCardInfo("", CardSet.Gladius, CardType.Attack, 1, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, 16); CardInfo info = GetCardByName("Injured"); for (int i = 0; i < 2; i++) c.Player.Deck.Add(CardObject.Instantiate(info, Vector2.zero)); }),

            //Doppel Stich - 2 Angriffe. Je Angriff: Schaden entsprechend Augenzahl, ziehe 1 Karte.
            new InstantCardInfo("", CardSet.Gladius, CardType.Attack, 1, 1,
            (CardInfo c) => {c.Player.DrawCards(1); for (int i = 0; i < 2; i++) c.Player.Attack(c.Enemy, c.DicePower); }),

            //Schwerttanz - Permanent: Diese Runde: 1 Stärke, jedes Mal, wenn du einen Angriff aktivierst.
            new PermanentCardInfo("", CardSet.Gladius, 1, 2,
            (CardInfo c) => {if (c.Type == CardType.Attack) c.Player.AddStatus(StatusEffect.FragileStrenght, 1); }, true),

#warning TODO
            //Schwertwirbel - Wirf eine beliebige Anzahl an Handkarten ab. X Angriffe, entsprechend abgeworfener Karten. Je Angriff: Schaden entsprechend Augenzahl.
            //new InstantCardInfo("", CardSet.Gladius, CardType.Attack, 1, 2, (CardInfo c) => ),

            #endregion Tier 1

            #region Tier 2

            //Lodernder Aufschlag - Schaden entsprechend Augenzahl. 12 Verwundbarkeit.
            new InstantCardInfo("", CardSet.Gladius, CardType.Attack, 2, 3,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Enemy.AddStatus(StatusEffect.Vulnerable, 12); }),

            //Gleißende Klinge - 16 Schaden. Entkräftet entsprechend Augenzahl.
            new InstantCardInfo("", CardSet.Gladius, CardType.Attack, 2, 2,
            (CardInfo c) => {c.Player.Attack(c.Enemy, 16); c.Enemy.AddStatus(StatusEffect.Feeble, c.DicePower);}),

#warning TODO
            //Flammende Leidenschat - Permanent: Erzeuge einen 12-Würfel. Dieser Würfel wird nicht verbraucht bei der verwendung von "Stich" Karten.
            new PermanentCardInfo("", CardSet.Gladius, 2, 3,
            (CardGameManager m, CardInfo c) => m.Player.AddDie(new DieInfo(12))),

            //Perfekte Parade - Block entsprechend Augenzahl. Verteidigungsstapel: Schaden entsprechend Augenzahl, jedes Mal, wenn der Gegner angreift.
            new PassiveBlockCardInfo("", CardSet.Gladius, 2, 2,
            (CardInfo c) => c.DicePower, (CardInfo c, ref int d) => c.Enemy.InstantDamage(c.RawDicePower), true),

            #endregion Tier 2

            #region Tier 3

            //Damokles Schwert - Schaden entsprechend Augenzahl. Diese Runde: Reduziere geg. Stärke und Schutz entsprechend Augenzahl.
            new InstantCardInfo("", CardSet.Gladius, CardType.Attack, 3, 4,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower);
                c.Enemy.RemoveStatus(StatusEffect.FragileStrenght, c.DicePower, true);
                c.Enemy.RemoveStatus(StatusEffect.FragileDefence, c.DicePower, true); }),

            //Sengendes Inferno - X Angriffe, entsprechend Augenzahl. Je Angriff: 2 Schaden, 2 Verwundbarkeit.
            new InstantCardInfo("", CardSet.Gladius, CardType.Attack, 3, 4,
            (CardInfo c) => {for (int i = 0; i < c.DicePower; i++)  { c.Player.Attack(c.Enemy, 2); c.Enemy.AddStatus(StatusEffect.Vulnerable, 2); } }),

            #endregion Tier 3

            #endregion Gladius

            #region Rete

            #region Tier 0

            //Stolperstrick - Schwäche entsprechend Augenzahl.
            new InstantCardInfo("Trip_Rope", CardSet.Rete, CardType.Attack, 0, 1,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Weak, c.DicePower)),

            //Netzwurf - Entferne: Reduziere Stärke entsprechend Augenzahl.
            new InstantCardInfo("Cast_Net", CardSet.Rete, CardType.Attack, 0, 1,
            (CardInfo c) => c.Enemy.RemoveStatus(StatusEffect.Strenght, c.DicePower, true), true),

            //Einschnüren - Entferne: Reduziere Schutz entsprechend Augenzahl.
            new InstantCardInfo("Constrict", CardSet.Rete, CardType.Attack, 0, 1,
            (CardInfo c) => c.Enemy.RemoveStatus(StatusEffect.Defence, c.DicePower, true), true),

            #endregion Tier 0

            #region Tier 1

            //Dompteur - Schaden entspricht geg. Schwäche.
            new InstantCardInfo("", CardSet.Rete, CardType.Attack, 1, 0,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.Enemy.GetStatus(StatusEffect.Weak))),

#warning TODO
            //Netzweber - Erzeuge 3 "Netzwurf" Karten, auf die Hand, diese verbrauchen den für die Aktivierung gewählten Würfel nicht.
            new InstantCardInfo("", CardSet.Rete, CardType.Skill, 1, 2,
            (CardInfo c) => {CardInfo info = GetCardByName("Cast_Net"); for (int i = 0; i < 3; i++) c.Player.Hand.Add(CardObject.Instantiate(info, Vector2.zero)); } ),

            //Fallensteller - Block entsprechend Augenzahl. Verteidigungsstapel: wenn diese Karte durch einen Angriff entfernt wird, Schwäche entsprechend Augenzahl.
            new PassiveBlockCardInfo("", CardSet.Rete, 1, 2,
            (CardInfo c) => c.DicePower, (CardInfo c, ref int d) => c.Enemy.AddStatus(StatusEffect.Weak, c.DicePower), false),

            //Entwaffnen - Schwäche und Betäubung entsprechend Augenzahl.
            new InstantCardInfo("", CardSet.Rete, CardType.Skill, 1, 1,
            (CardInfo c) => {c.Enemy.AddStatus(StatusEffect.Weak, c.DiceBonus); c.Enemy.AddStatus(StatusEffect.Stun, c.DiceBonus); }),

            #endregion Tier 1

            #region Tier 2

            //Pittakos - Permanent: Jedes Mal, wenn dein Gegner angreift, füge ihm, 4 Schwäche und 4 Entkräftet zu.
            new PermanentCardInfo("", CardSet.Rete, 2, 2,
            (Participant attacker, Participant defender, int damage) => {if (attacker is Enemy enemy) {enemy.AddStatus(StatusEffect.Weak, 4); enemy.AddStatus(StatusEffect.Feeble, 4);}}),

#warning TODO
            //Höhnische Gewandheit - 15 Block. Verteidigungsstapel: Chance von 25 % (max. 75%), das gegnerische Angriffe verfehlen.
            //new InstantCardInfo("", CardSet.Rete, CardType.Block, 2, 3, (CardInfo c) => ),

            //Arachne - Permanenet: Zu Rundenbeginn: erzeuge 1 "Netzwurf" Karte. Diese  Runde: die erzeugte Karte hat keine Kosten und die Grund-Augenzahl für ihre Aktivierung entspricht  der Augenzahl dieser Karte.
            new PermanentCardInfo("", CardSet.Rete, 2, 3,
            (CardGameManager m, CardInfo c) => {
                CardInfo info = new InstantCardInfo("Cast_Net", CardSet.Rete, CardType.Attack, 0, 0,
                    (CardInfo c) => c.Enemy.RemoveStatus(StatusEffect.Strenght, c.DicePower, true), true);
                m.Player.Hand.Add(CardObject.Instantiate(info, Vector2.zero));
            }),

#warning TODO
            //Schicksalsfaden - Permanent: Jedes Mal, wenn der Gegner negative Status durch Regeneration abbaut, Schaden in Höhe der abgebauten Stapel, durchstoßend.
            //new PermanentCardInfo("", CardSet.Rete, 2, 2, (CardInfo c) => ),

            #endregion Tier 2

            #region Tier 3

            //Kontrolle - Diese Runde: setze geg. Regeneration auf 0.
            new InstantCardInfo("", CardSet.Rete, CardType.Skill, 3, 0,
            (CardInfo c) => {int value = c.Enemy.GetStatus(StatusEffect.Regeneration); c.Enemy.RemoveStatus(StatusEffect.Regeneration, value);
            c.Player.AddAction((CardGameManager m) => m.Enemy.AddStatus(StatusEffect.Regeneration, value)); }),

            //Graumsames Schicksal - Entferne alle geg. negativen Status. Schaden entsprechend Anzahl der kombinierten entfernten Stapel, multipliziert mit 4, durchstoßend.
            new InstantCardInfo("", CardSet.Rete, CardType.Attack, 3, 2,
            (CardInfo c) =>
            {
                int sum = 0;
                for (int i = (int)StatusEffect.Regeneration; i < (int)StatusEffect.Vulnerable + 1; i++)
                {
                    StatusEffect current = (StatusEffect)i;
                    int value = c.Enemy.GetStatus(current);
                    sum += value;
                    c.Enemy.RemoveStatus(current, value);
                }
                c.Player.Attack(c.Enemy, sum * 4, true);
            }),

            #endregion Tier 3

            #endregion Rete

            #region Scutum

            #region Tier 0

            //Schildblock - Block entsprechend Augenzahl, multipliziert mit 2.
            new BlockCardInfo("Shield_Block", CardSet.Scutum, 0, 2,
            (CardInfo c) => c.DicePower * 2),

            //Testudo - 15 Block. Du kannst diese Karte nicht aktivieren wenn du in dieser Runde einen Angriff gespielt hast. Du kannst keine Angriffe nach Aktivierung dieser Karte spielen.
            new BlockCardInfo("Testudo", CardSet.Scutum, 0, 1,
            (CardInfo c) => 15 + c.DiceBonus)
            {CostReduction = (CardInfo c) => -c.Player.PlayedCards.Count(x => x.Type == CardType.Attack),
            InstantAction = (CardInfo c) => c.Player.LockCardType(CardType.Attack)},

            //Schildseele - 1 Schutz, multipliziert mit aktiven Verteidigungskarten.
            new InstantCardInfo("Shield_Soul", CardSet.Scutum, CardType.Skill, 0, 0,
            (CardInfo c) => c.Player.AddStatus(StatusEffect.Defence, c.Player.BlockStack.Length)),

            #endregion Tier 0

            #region Tier 1

#warning TODO
            //Provokation - Block entsprechend Augenzahl. Verteidigunsstapel: dein Gegner kann nur Angriffe ausführen, ändere alle Nicht-Angriffsaktionen zu Standartangriffen.
            //new InstantCardInfo("", CardSet.Scutum, CardType.Block, 1, 1, (CardInfo c) => ),

            //Schildlogik - Permanent: Zu Rundenbeginn: Ziehe 1 Karte für jede aktive Verteidigungskarte.
            new PermanentCardInfo("", CardSet.Scutum, 1, 2,
            (CardGameManager m, CardInfo c) => m.Player.DrawCards(m.Player.BlockStack.Length)),

#warning TODO
            //Unbewegliches Objekt - Block dieser Karte wird zu deinem doppelten aktuellen Block.
            //new BlockCardInfo("", CardSet.Scutum, 1, 3, (CardInfo c) => ),

            //Schildflamme - 1 Stärke, multipliziert mit aktiven Verteidigungskarten.
            new InstantCardInfo("", CardSet.Scutum, CardType.Skill, 1, 0,
            (CardInfo c) => c.Player.AddStatus(StatusEffect.Strenght, c.Player.BlockStack.Length)),

            #endregion Tier 1

            #region Tier 2

#warning TODO
            //Bastion - Block entsprichend Augenzahl, multipliziert mit 2. Verteidigungsstapel:  Schutz entsprechend Augenzahl.
            //new InstantCardInfo("", CardSet.Scutum, CardType.Block, 2, 2, (CardInfo c) => ),

            //Petra Solida - Block entsprichend Augenzahl multipliziert mit 3. Verteidigungsstapel: Zu Rundenbeginn: regeneriere den ursprünglichen Blockwert dieser Karte.
            new PassiveBlockCardInfo("", CardSet.Scutum, 2, 3,
            (CardInfo c) => c.DicePower * 3,
            (CardGameManager m, CardInfo c) => (c as BlockCardInfo).ResetDamage()),

            //Scutum Vitalis - Permanent: Erhalte einen zusätzlichen Platz in deinem Verteidigungsstapel.
            new InstantCardInfo("", CardSet.Scutum, CardType.Skill, 2, 0,
            (CardInfo c) => c.Player.BlockSlots += 1, true),

            //Rammbock - Schaden entsprechend Block.
            new InstantCardInfo("", CardSet.Scutum, CardType.Attack, 2, 3,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.Player.Block)),

            #endregion Tier 2

            #region Tier 3

            //Mars Schirmherrschaft - Permanent: Zu Rundenbeginn:  Diese Runde: erhalte einen zusätzlichen 4-Würfel für jede aktive Verteidigungskarte.
            new PermanentCardInfo("", CardSet.Scutum, 3, 3,
            (CardGameManager m, CardInfo c) =>  {for (int i = 0; i < m.Player.BlockStack.Length; i++) m.Player.AddDie(new DieInfo(4)); } ),

            //Moloch - Permanent: Lege zum Ende deines Zuges eine Karte aus deinem Verteidigungsstapel ab und füge deinem Gegner Schaden entsprechend abgelegtem Block, multipliziert mit Augenzahl zu.
            new PermanentCardInfo("", CardSet.Scutum, 3, 1,
            (CardGameManager m, CardInfo c) => { CardObject[] cards = m.Player.ActiveBlock.Cards; CardObject card = cards[UnityEngine.Random.Range(0, cards.Length)];
                m.Player.Attack(m.Enemy, c.DicePower * (card.Info as BlockCardInfo).CurrentBlock); m.Player.DiscardSingle(card); }),

            #endregion Tier 3

            #endregion Scutum

            #region Pugio

            #region Tier 0

            //Dolchstoß - 2 Schaden. Blutung entsprechend Augenzahl, falls der Gegner keinen Block hat.
            new InstantCardInfo("Dagger_Thrust", CardSet.Pugio, CardType.Attack, 0, 1,
            (CardInfo c) => { c.Player.Attack(c.Enemy, 2); if (c.Enemy.Block < 1) c.Enemy.AddStatus(StatusEffect.Bleeding, c.DicePower); }),

            //Arterienschnitt -  Keine Wirkung auf Gegner mit Block, Blutung entsprechend Augenzahl.
            new InstantCardInfo("Artery_Cut", CardSet.Pugio, CardType.Attack, 0, 1,
            (CardInfo c) => {if (c.Enemy.Block < 1) c.Enemy.AddStatus(StatusEffect.Bleeding, c.DicePower);}),

            //Messerwurf - Schaden entsprechend Augenzahl. Multipliziert mit 2, falls der Gegner keinen Block hat.
            new InstantCardInfo("Knife_Throw", CardSet.Pugio, CardType.Attack, 0, 1,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * (c.Enemy.Block < 1 ? 2 : 1))),

            #endregion Tier 0

            #region Tier 1

#warning ???
            //Herzsucher - 4 Schaden. Keine Wirkung auf Gegner mit Block, Blutung entsprechend Augenzahl.
            new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 1, 2,
            (CardInfo c) => { c.Player.Attack(c.Enemy, 4); if (c.Enemy.Block < 1) c.Enemy.AddStatus(StatusEffect.Bleeding, c.DicePower); }),

#warning TODO
            //Kehlschnitt - 2 Schaden. Diese Runde:  Der Gegner kann keine Fähigkeiten verwenden, falls er keinen Block hat.
            //new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 1, 2, (CardInfo c) => ),

            //Plattenbrecher - Schaden entsprechend Augenzahl. Multipliziert mit 6, falls der Gegner Block hat.
            new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 1, 1,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * (c.Enemy.Block < 1 ? 6 : 1))),

            //Chirugischer Eingriff - 8 Blutung.
            new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 1, 2,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Bleeding, 8)),

            #endregion Tier 1

            #region Tier 2

            //Purpurnen Flüsse - Blutung entsprechend Augenzahl. Multipliziert mit 2, falls der Gegner keinen Block hat.
            new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 2, 2,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Bleeding, 8 * (c.Enemy.Block < 1 ? 2 : 1))),

            //Blutschmerz - Verwundbarkeit entsprechend geg. Blutungsstapel.
            new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 2, 2,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Vulnerable, c.Enemy.GetStatus(StatusEffect.Bleeding))),

#warning TODO
            //Salz in die Wunde - Permanent: Jedes Mal, wenn du dem Gegner Blutung zufügst, füge zusätzliche Blutung entsprechend Augenzahl zu.
            //new InstantCardInfo("", CardSet.Pugio, CardType.Skill, 2, 1, (CardInfo c) => ),

#warning TODO
            //Ausweiden - 1 Schaden. Bonusschaden durch Blutung wird multipliziert mit 3.
            //new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 2, 2, (CardInfo c) => ),

            #endregion Tier 2

            #region Tier 3

            //Herzensbrecher - X Angriffe, entsprechend Augenzahl. Je Angriff: 1 Schaden und 1 Blutung.
            new InstantCardInfo("", CardSet.Pugio, CardType.Attack, 3, 2,
            (CardInfo c) => { for(int i = 0; i < c.DicePower; i++) {c.Player.Attack(c.Enemy, 1); c.Enemy.AddStatus(StatusEffect.Bleeding, 1); } }),

            //Hemokinesis - Permanent: Jeder Angriff fügt dem Gegner zusätzlich 1 Blutung zu.
            new PermanentCardInfo("", CardSet.Pugio, 3, 2,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Bleeding, 1), true),

            #endregion Tier 3

            #endregion Pugio

            #region Doru

            #region Tier 0

            //Speer Stoß - Schaden entsprechend Augenzahl.
            new InstantCardInfo("Spear_Thrust", CardSet.Doru, CardType.Attack, 0, 1,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower)),

            //Abwehren - Block entsprechend Augenzahl.
            new BlockCardInfo("Fend_Off", CardSet.Doru, 0, 1,
            (CardInfo c) => c.DicePower),

            //Zweite Spitze - 6 Schaden. Keine Kosten wenn in dieser Runde ein Angriff gespielt wurde.
            new InstantCardInfo("Second_Tip", CardSet.Doru, CardType.Attack, 0, 1,
            (CardInfo c) => c.Player.Attack(c.Enemy, 6))
            { CostReduction = (CardInfo c) => c.Player.PlayedCards.Count(x => x.Type == CardType.Attack)},

            #endregion Tier 0

            #region Tier 1

            //Perforieren - X Angriffe, entsprechend Augenzahl. Je Angriff: 1 Schaden und 1  Blutung.
            new InstantCardInfo("", CardSet.Doru, CardType.Attack, 1, 2,
            (CardInfo c) => { for(int i = 0; i < c.DicePower; i++) {c.Player.Attack(c.Enemy, 1); c.Enemy.AddStatus(StatusEffect.Bleeding, 1); } }),

            //Hoplit - 8 Block. Diese Karte skaliert mit Stärke und Schutz.
            new BlockCardInfo("", CardSet.Doru, 1, 1,
            (CardInfo c) => 8 + c.Player.GetStatus(StatusEffect.Strenght) + c.Player.GetStatus(StatusEffect.FragileStrenght)),

            //Horn des Katoblepas - 4 Schaden. Entferne alle deine Stapel Verwundbarkeit und füge dem Gegner Verwundbarkeit entsprechend der entfernten Stapel zu.
            new InstantCardInfo("", CardSet.Doru, CardType.Attack, 1, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, 4);
                int value = c.Player.GetStatus(StatusEffect.Vulnerable); c.Player.RemoveStatus(StatusEffect.Vulnerable, value); c.Enemy.AddStatus(StatusEffect.Vulnerable, value); }),

#warning TODO
            //Hybris - Schaden entsprechend dreifacher Augenzahl. Die Würfel werden nicht verbraucht. Diese Karte kann nur aktiviert werden wenn du keinen  Block hast.
            new InstantCardInfo("", CardSet.Doru, CardType.Attack, 1, 2,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * 3)),

            #endregion Tier 1

            #region Tier 2

            //Allbewaffnung - Erhöhe Stärke und Schutz um 4.
            new InstantCardInfo("", CardSet.Doru, CardType.Skill, 2, 2,
            (CardInfo c) => {c.Player.AddStatus(StatusEffect.Strenght, 4); c.Player.AddStatus(StatusEffect.Defence, 4); }),

            //Titan-Töter - Schaden entsprechend Augenzahl, multipliziert mit 4 wenn dein Gegner prozentuall mehr Vitalität besitzt als du.
            new InstantCardInfo("", CardSet.Doru, CardType.Attack, 2, 3,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * (c.Enemy.Health > c.Player.Health ? 4 : 1))),

            //Spartanische Seele - Permanent: Zu Rundenbeginn erhalte 2 Stärke und 2 Schutz.
            new PermanentCardInfo("", CardSet.Doru, 2, 4,
            (CardGameManager m, CardInfo c) => {m.Player.AddStatus(StatusEffect.Strenght, 2); m.Player.AddStatus(StatusEffect.Defence, 2); }),

            //Keres-Geist - Blutung entsprechend deiner zusätzlichen Stärke.
            new InstantCardInfo("", CardSet.Doru, CardType.Attack, 2, 2,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Bleeding, c.Player.GetStatus(StatusEffect.Strenght))),

            #endregion Tier 2

            #region Tier 3

#warning TODO
            //Hepheistos Arsenal - Permanent: Zu Rundenbeginn: Diese Runde: Erhöhe die Augenzahl auf allen Würfeln um 4.
            //new InstantCardInfo("", CardSet.Doru, CardType.Skill, 3, 3, (CardInfo c) => ),

            //Mächtiger Pallas - 80 Schaden. Die Augenzahl zur Aktivierung dieser Karte muss mindestens 12 betragen.
            new InstantCardInfo("", CardSet.Doru, CardType.Attack, 3, 3,
            (CardInfo c) => c.Player.Attack(c.Enemy, 80))
            {CostReduction = (CardInfo c) => c.DicePower < 12 ? -1 : 0 },

            #endregion Tier 3

            #endregion Doru

            #region Parmula

            #region Tier 0

            //Parma - Block entsprechend Augenzahl.
            new BlockCardInfo("Parma", CardSet.Parmula, 0, 1,
            (CardInfo c) => c.DicePower),

            //Schildschlag - Schaden entsprechend Augenzahl, skaliert mit Stärke und Schutz.
            new InstantCardInfo("Shield_Slam", CardSet.Parmula, CardType.Attack, 0, 1,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower + c.Player.GetStatus(StatusEffect.Defence) + c.Player.GetStatus(StatusEffect.FragileDefence))),

            //Umfegen - Schwäche entsprechend Block
            new InstantCardInfo("Sweep", CardSet.Parmula, CardType.Attack, 0, 2,
            (CardInfo c) => c.Enemy.AddStatus(StatusEffect.Weak, c.Player.Block)),

            #endregion Tier 0

            #region Tier 1

            //Gepanzeter Koloss - Permanent: Zu Rundenbeginn: Diese Runde: Erhalte 3 Stärke für jede aktive Verteidigungskarte.
            new PermanentCardInfo("", CardSet.Parmula, 1, 2,
            (CardGameManager m, CardInfo c) => m.Player.AddStatus(StatusEffect.FragileStrenght, 3 * m.Player.BlockStack.Length)),

#warning TODO
            //Titan - Wähle eine Karte aus deinem Verteidigungsstapel. Diese Runde: Stärke entsprechend dem Block, den die gewählte Karte gewährt.
            //new InstantCardInfo("", CardSet.Parmula, CardType.Skill, 1, 2, (CardInfo c) => ),

            //Wahre Phalanx - Permanent: Schutz entsprechend Augenzahl.
            new InstantCardInfo("", CardSet.Parmula, CardType.Skill, 1, 2,
            (CardInfo c) => c.Player.AddStatus(StatusEffect.Defence, c.DicePower), true),

#warning TODO
            //Gleichgewicht - Block entsprechend Augenzahl. 2 Schutz.
            //new InstantCardInfo("", CardSet.Parmula, CardType.Block, 1, 2, (CardInfo c) => ),

            #endregion Tier 1

            #region Tier 2

            //Minotaurus Stampfer - Schaden entsprechend Augenzahl. Diese Runde: Erhalte Schutz entspreched dem durch diese Karte zugefügten Schaden.
            new InstantCardInfo("", CardSet.Parmula, CardType.Attack, 2, 2, (CardInfo c) => c.Player.AddStatus(StatusEffect.Defence, c.Player.Attack(c.Enemy, c.DicePower))),

            //Eiserner Wille - Permanent: Jedes Mal, wenn du einen geg. Angriff blockst, ziehe 1 zusätzliche Karte, zu Beginn der nächsten Runde.
            new PermanentCardInfo("", CardSet.Parmula, 2, 4,
            (Participant a, Participant d, int v) => {if (d is Player p && !(p.Block < v)) p.AddAction((CardGameManager m) => m.Player.DrawCards(1)); }),

#warning TODO
            //Unbeugsam - Permanent: Du kannst keine negativen Status aufbauen, solange du aktive Verteidigungskarten hast.
            //new InstantCardInfo("", CardSet.Parmula, CardType.Skill, 2, 3, (CardInfo c) => ),

#warning TODO
            //Reflektieren - 1 Block. Verteidigungsstapel: Annulliere den nächsten Schaden eines Angriffs, der diese Karte betrifft. Füge deinem Gegner Schaden entsprechend dem annullierten Schaden zu. Lege diese Karte dannach auf den Ablagestapel.
            //new InstantCardInfo("", CardSet.Parmula, CardType.Block, 2, 4, (CardInfo c) => ),

            #endregion Tier 2

            #region Tier 3

            //Heroischer Ansturm - Schaden entsprechend Block. Die Kosten dieser Karte sinken um 1 für jede aktive Verteidigungskarte.
            new InstantCardInfo("", CardSet.Parmula, CardType.Attack, 3, 4,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.Player.Block))
            {CostReduction = (CardInfo c) => c.Player.BlockStack.Length},

#warning ???
            //Soterias Segen - Diese Karte kann nicht gespielt werden wenn in diese Runde ein Angriff gespielt wurde. Diese Runde:es können keine Angriffe gespielt werden.
            //new InstantCardInfo("", CardSet.Parmula, CardType.Skill, 3, 0, (CardInfo c) => ),

            #endregion Tier 3

            #endregion Parmula

            #region Scindo

            #region Tier 0

            //Stoß-Schnitt - Blutung entsprechend Augenzahl, falls der Gegner keinen Block hat.
            new InstantCardInfo("Jab_Cut", CardSet.Scindo, CardType.Attack, 0, 1,
            (CardInfo c) => {if (c.Enemy.Block < 1) c.Enemy.AddStatus(StatusEffect.Bleeding, c.DicePower);}),

            //Knochenschere - Schaden entsprechend Augenzahl. Diese Runde: Reduziere Schutz entsprechend Augenzahl.
            new InstantCardInfo("Bone_Shears", CardSet.Scindo, CardType.Attack, 0, 2,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Enemy.RemoveStatus(StatusEffect.FragileDefence, c.DicePower, true); }),

            //Messer Wetzen - Permanent: Stärke entsprechend Augenzahl.
            new InstantCardInfo("Dagger_Whet", CardSet.Scindo, CardType.Skill, 0, 2,
            (CardInfo c) => c.Player.AddStatus(StatusEffect.Strenght, c.DicePower)),

            #endregion Tier 0

            #region Tier 1

#warning TODO
            //Metzger - Permanent: Zusätzliche Stärke erhöht die Anzahl an zugefügten Blutungsstapeln.
            //new InstantCardInfo("", CardSet.Scindo, CardType.Skill, 1, 2, (CardInfo c) => ),

            //Fleischhaken - Verwundbarkeit, Blutung und Entkräften entsprechend Augenzahl, falls der Gegner keinen Block hat.
            new InstantCardInfo("", CardSet.Scindo, CardType.Attack, 1, 2,
            (CardInfo c) => {if (c.Enemy.Block < 1)
                {
                    c.Enemy.AddStatus(StatusEffect.Vulnerable, c.DicePower);
                    c.Enemy.AddStatus(StatusEffect.Bleeding, c.DicePower);
                    c.Enemy.AddStatus(StatusEffect.Feeble, c.DicePower);
                } }),

            //Reißzahn - Entferne die letzte geg. Verteidigungskarte. Falls der Gegner dannach keinen Block mehr hat, Blutung entsprechend Augenzahl.
            new InstantCardInfo("", CardSet.Scindo, CardType.Attack, 1, 2,
            (CardInfo c) =>  { if (c.Enemy.BlockStack.Length > 0) { c.Enemy.RemoveLastBlock();
            if (c.Enemy.Block < 1) c.Enemy.AddStatus(StatusEffect.Bleeding, c.DicePower); } }),

            //Kritischer Treffer - 4 Schaden. 50% Chance den zugefügten Schaden mit Augenzahl zu multiplizieren.
            new InstantCardInfo("", CardSet.Scindo, CardType.Attack, 1, 2,
            (CardInfo c) => c.Player.Attack(c.Enemy, 4 * UnityEngine.Random.Range(0, 2) > 0 ? 4 : 1)),

            #endregion Tier 1

            #region Tier 2

            //Schnellschritt - Permanent: Ziehe 1 Karte, jedes Mal wenn du eine Angriffkarte mit 1 oder weniger Kosten spielst.
            new PermanentCardInfo("", CardSet.Scindo, 2, 2,
            (CardInfo c) => {if (c.Cost < 2 && c.Type == CardType.Attack) c.Player.DrawCards(1); }, true),

            //Zerstückeln - Blutung entsprechend Augenzahl multipliziert mit 3, falls der Gegner keinen Block hat.
            new InstantCardInfo("", CardSet.Scindo, CardType.Attack, 2, 2,
            (CardInfo c) => {if (c.Enemy.Block < 1) c.Enemy.AddStatus(StatusEffect.Bleeding, c.DicePower * 3);}),

            //Merkur Trickklinge - Schaden entsprechend Augenzahl. Ziehe 1 Karte. Falls dein Gegner Verwundbarkeitsstapel hat, werden die Würfel für die Aktivierung dieser Karte nicht verbraucht.
            new InstantCardInfo("", CardSet.Scindo, CardType.Attack, 2, 3,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Player.DrawCards(1); if (c.Enemy.GetStatus(StatusEffect.Vulnerable) > 0) c.RefundDice(); }),

#warning TODO
            //Schwertarm - Permanent: Stärke entsprechend Augenzahl. Jedes Mal, wenn du eine Verteidigungskarte ziehst lege diese auf den Ablagestapel, ziehe 1 Karte und erhalte 3 Stärke.
            //new PermanentCardInfo("", CardSet.Scindo, 2, 3, (CardInfo c) => ),

            #endregion Tier 2

            #region Tier 3

            //Janus Kehrtwende - Schaden entsprechend Augenzahl. Erhöhe deine Stärke entsprechend zugefügtem Schaden, falls der Gegner keinen Block hat. Ziehe 1 Karte.
            new InstantCardInfo("", CardSet.Scindo, CardType.Attack, 3, 1,
            (CardInfo c) => {bool gainStrength = c.Enemy.Block < 1; int damage = c.Player.Attack(c.Enemy, c.DicePower);if (gainStrength) c.Player.AddStatus(StatusEffect.Strenght, damage); }),

            //Faunus Fallaxt - Entferne alle geg. Verteidigungskaren. Füge dann Schaden entsprechend Augenzahl, plus geg. Entfernten Block.
            new InstantCardInfo("", CardSet.Scindo, CardType.Attack, 3, 4,
            (CardInfo c) => {int block = c.Enemy.RemoveAllBlock(); c.Player.Attack(c.Enemy, c.DicePower + block); } ),

            #endregion Tier 3

            #endregion Scindo

            #endregion Weapons

            #region Armor

            #region Cassis

            #region Tier 0

            //Kopfsache - Führe sofort eine Regeneration durch.
            new InstantCardInfo("Head_Matter", CardSet.Cassis, CardType.Skill, 0, 0,
            (CardInfo c) => c.Player.DoRegeneration()),

            //Kopfstoß - 2 Schaden. Betäubung entsprechend Augenzahl.
            new InstantCardInfo("Head_Thrust", CardSet.Cassis, CardType.Attack, 0, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, 2); c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower); }),

            //Kühnheit - Diese Runde: 4 Stärke und 4 Schutz.
            new InstantCardInfo("Boldness", CardSet.Cassis, CardType.Skill, 0, 1,
            (CardInfo c) => {c.Player.AddStatus(StatusEffect.Strenght, 4); c.Player.AddStatus(StatusEffect.Defence, 4);
            c.Player.AddAction((CardGameManager m) => {m.Player.RemoveStatus(StatusEffect.FragileStrenght, 4); m.Player.RemoveStatus(StatusEffect.FragileDefence, 4);}); }),

            #endregion Tier 0

            #region Tier 1

#warning TODO
            //Schützender Käfig - Wirf eine beliebige Anzahl an Handkarten ab. Block entsprechend Augenzahl, multipliziert mit angeworfenen Handkarten.
            //new InstantCardInfo("", CardSet.Cassis, CardType.Block, 1, 2, (CardInfo c) => ),

            //Abfälschende Form - Block entsprechend Augenzahl. Verteidigungsstapel: füge dem Gegner 2 Betäubung zu,jedes Mal wenn er Angreift.
            new PassiveBlockCardInfo("", CardSet.Cassis, 1, 2,
            (CardInfo c) => c.DicePower, (CardInfo c, ref int d) => c.Enemy.AddStatus(StatusEffect.Stun, 2), true),

            #endregion Tier 1

            #region Tier 2

            //Symbol der Pflicht - Permanent: Zu Rundenbeginn: 2 Schutz.
            new PermanentCardInfo("", CardSet.Cassis, 2, 3, (CardGameManager m, CardInfo c) => m.Player.AddStatus(StatusEffect.Defence, 2)),

            #endregion Tier 2

            #region Tier 3

#warning TODO
            //Trotzkopf - Entferne: Wähle für jeden freien Platz in deinem Verteidigungsstapel eine Verteidigungskarte aus deinem Stapel und aktiviere sie. Verwende für die Aktivierung jeder Karte die für diese Karte aufgewendete Augenzahl.
            //new InstantCardInfo("", CardSet.Cassis, CardType.Skill, 3, 2, (CardInfo c) => , true),

            #endregion Tier 3

            #endregion Cassis

            #region Galerus

            #region Tier 0

            //Schulterangriff -  2 Schaden. Ziehe Karten entsprechend Augenzahl.
            new InstantCardInfo("Shoulder_Attack", CardSet.Gladius, CardType.Attack,0, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, 2); c.Player.DrawCards(c.DicePower); }),

            //Voll-Platte - Block entsprechend Augenzahl, multipliziert mit 2.
            new BlockCardInfo("Full_Plate", CardSet.Gladius, 0, 1,
            (CardInfo c) => c.DicePower * 2),

            //Agilität - Block entsprechend Augenzahl. Verteidigungsstapel: Zu Rundenbeginn: Ziehe 2 zusätzliche Karten.
            new PassiveBlockCardInfo("Agility", CardSet.Gladius, 0, 1,
            (CardInfo c) => c.DicePower, (CardGameManager m, CardInfo c) => m.Player.DrawCards(2)),

            #endregion Tier 0

            #region Tier 1

            //Entgehen - Block entsprechend Augenzahl. Verteidigungsstapel: 30 % Chance (max. 75%)  das geg. Angriffe verfehlen.
            new PassiveBlockCardInfo("", CardSet.Galerus, 1, 2,
            (CardInfo c) => c.DicePower,
            (CardInfo c, ref int d) => {float dogeChance = 0.3f; if (UnityEngine.Random.Range(0,1) < dogeChance) d = 0; }, true),

            #warning TODO
            //Klein-Schild - 1 Block. Schutz entsprechend Augenzahl.
            //new BlockCardInfo("", CardSet.Galerus, 1, 1, (CardInfo c) => 1),

            #endregion Tier 1

            #region Tier 2

            //Präsidarius - 30 Block. Verteidigunsstapel: Zu Rundenbeginn: Diese Runde: Erhalte einen zusätzlichen Würfel entsprechend Augenzahl.
            new PassiveBlockCardInfo("", CardSet.Galerus, 2, 3,
            (CardInfo c) => 30,
            (CardGameManager m, CardInfo c) => m.Player.AddDie(new DieInfo(c.RawDicePower, c.RawDicePower, c.RawDicePower, c.RawDicePower))),

            #endregion Tier 2

            #region Tier 3

            //Ausweichrolle - Diese Runde: Annuliere jeden eingehenden geg. Angriff.
            new InstantCardInfo("", CardSet.Galerus, CardType.Skill, 3, 4,
            (CardInfo c) => c.Player.AddStatus(StatusEffect.Invulnerable, 1000)),

            #endregion Tier 3

            #endregion Galerus

            #region Manica

            #region Tier 0

            //Kopfnuss - 2 Schaden. Verwundbarkeit entsprechend Augenzahl.
            new InstantCardInfo("Headbutt", CardSet.Manica, CardType.Attack, 0,1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, 2); c.Enemy.AddStatus(StatusEffect.Vulnerable, c.DicePower); }),

            //Ableit - 1 Block. Verteidigungsstapel: Schutz entsprechend Augenzahl.
            new BlockCardInfo("Deflect", CardSet.Manica, 0, 1,
            (CardInfo c) => 1)
            {InstantAction = (CardInfo c) => (c as BlockCardInfo).StatusMod = (StatusEffect.Defence, c.DicePower)},

            //Keil-Hieb - 20 Schaden. Keine Wirkung auf Gegner mit Block.
            new InstantCardInfo("Wedge_Slash", CardSet.Manica, CardType.Attack, 0,2,
            (CardInfo c) => { if (c.Enemy.Block > 0) c.Player.Attack(c.Enemy, 20); }),

            #endregion Tier 0

            #region Tier 1

            //Fangen - 3 Block. Verteidigungsstapel: Reduziere jeden eingehenden Schaden durch Angriffe entsprechend Augenzahl (Nie unter 1).
            new PassiveBlockCardInfo("", CardSet.Manica, 1, 2, (CardInfo c) => 3, (CardInfo c, ref int d) => d = Mathf.Max(d - c.DicePower, 1), true),

            //Winkel-Block - Block entsprechend Augenzahl. Verteidigungsstapel: Zu Rundenbeginn: Erhöhe den Block aller Karten im Verteidigungstapel um 5.
            new InstantCardInfo("", CardSet.Manica, CardType.Block, 1, 3,
            (CardInfo c) =>  {CardInfo[] blockCards = c.Player.ActiveBlock.Cards.Select(x => x.Info).ToArray(); foreach (var card in blockCards) card.DiceBonus += 5; }),

            #endregion Tier 1

            #region Tier 2

            //Umrempeln - Schaden entsprechend Augenzahl. Schwäche entsprechend Block.
            new InstantCardInfo("", CardSet.Manica, CardType.Attack, 2, 1,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Player.AddStatus(StatusEffect.Weak, c.Player.Block); }),

            #endregion Tier 2

            #region Tier 3

            //Segmentierter Panzer - Permanent: Zu Rundenbeginn: erhalte einen zusätzlichen Platz in deinem Verteidigungsstapel.
            new PermanentCardInfo("", CardSet.Manica, 3, 2, (CardGameManager m, CardInfo c) => m.Player.BlockSlots += 1),

            #endregion Tier 3

            #endregion Manica

            #region Ocrea

            #region Tier 0

            //Tritt - Schaden entsprechend Augenzahl. Multipliziert mit 2, falls der Gegner Block hat.
            new InstantCardInfo("Kick", CardSet.Ocrea, CardType.Attack, 0,1,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * (c.Enemy.Block > 0 ? 2 : 1))),

            //Ausweichschritt - 1 Block. Verteidigungsstapel: 5% Chance (max. 75%), multipliziert mit Augenzahl, das geg. Angriffe verfehlen.
            new PassiveBlockCardInfo("Deflect", CardSet.Manica, 0, 1,
            (CardInfo c) => 1,
            (CardInfo c, ref int d) => {float dogeChance = Math.Min(0.05f * c.DicePower, 0.75f); if (UnityEngine.Random.Range(0,1) < dogeChance) d = 0; }, true),

            //Beinarbeit - Block entsprechend Augenzahl. Verteidigungsstapel: Zu Rundenbeginn: Lege diese Karte auf den Ablagestapel, Stärke entsprechend abgelegtem Block.
            new PassiveBlockCardInfo("Footwork", CardSet.Manica, 0, 1,
            (CardInfo c) => c.DicePower,
            (CardGameManager m, CardInfo c) => {m.Player.AddStatus(StatusEffect.Strenght, (c as BlockCardInfo).CurrentBlock); m.Player.DiscardSingle(c.Card); }),

            #endregion Tier 0

            #region Tier 1

            //Kniestoß - Schaden und Betäubung entsprechend Augenzahl.
            new InstantCardInfo("", CardSet.Ocrea, CardType.Attack, 1, 2,
            (CardInfo c) => {c.Player.Attack(c.Enemy, c.DicePower); c.Enemy.AddStatus(StatusEffect.Stun, c.DicePower); }),

            //Kick - Schaden entsprechend Augenzahl, multipliziert mit aktiven Verteidigungskarten.
            new InstantCardInfo("", CardSet.Ocrea, CardType.Attack, 1, 2,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * c.Player.BlockStack.Length)),

            #endregion Tier 1

            #region Tier 2

            //Sprungtritt - Schaden entsprechend Augenzahl. Multipliziert mit 3, falls der Gegner Block hat.
            new InstantCardInfo("", CardSet.Ocrea, CardType.Attack, 2, 3,
            (CardInfo c) => c.Player.Attack(c.Enemy, c.DicePower * (c.Enemy.Block < 1 ? 3 : 1))),

            #endregion Tier 2

            #region Tier 3

            //Fliegender Dreh-Kick - Falls der Gegner keinen Block hat, werden die gewählten Würfel nicht verbraucht. Schaden entsprechend Augenzahl, multipliziert mit aktiven Verteidigungskarten.
            new InstantCardInfo("", CardSet.Ocrea, CardType.Attack, 3, 3,
            (CardInfo c) => { bool refundDice = c.Enemy.Block < 1; c.Player.Attack(c.Enemy, c.DicePower * c.Player.BlockStack.Length); if (refundDice) c.RefundDice(); }),

            #endregion Tier 3

            #endregion Ocrea

            #endregion Armor

            #region Health

            #region Positiv

            //Erfrischt - Entferne: Ziehe 1 Karte. Permanent: 3 Regeneration.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0,
            (CardInfo c) => {c.Player.DrawCards(1); c.Player.AddStatus(StatusEffect.Regeneration, 3); }, true),

            //Gut Genährt - Entferne: Ziehe 1 Karte. Stelle 2 Vitalität wieder her.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0,
            (CardInfo c) => {c.Player.DrawCards(1); c.Player.Heal(2); }, true),

            //Ausgeruht - Entferne: Ziehe 1 Karte. Diese Runde: Erzeuge einen 20-Würfel.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0,
            (CardInfo c) => {c.Player.DrawCards(1); c.Player.AddDie(new DieInfo(20)); }, true),

            //Gladiatorstärke - Entferne: Ziehe 1 Karte. Permanent: 3 Stärke.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0,
            (CardInfo c) => {c.Player.DrawCards(1); c.Player.AddStatus(StatusEffect.Strenght, 3); }, true),

            //Gladiatorausdauer - Entferne: Ziehe 1 Karte. Permanent: Zu Rundenbeginn: Diese Runde: Erzeuge einen 4-Würfel.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0,
            (CardInfo c) => {c.Player.DrawCards(1); c.Player.AddAction(
                (CardGameManager m) => m.Player.AddDie(new DieInfo(4)), true); }, true),

            //Gladiatorgewandtheit - Entferne: Ziehe 1 Karte. Permanent: 3 Schutz.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0,
            (CardInfo c) => {c.Player.DrawCards(1); c.Player.AddStatus(StatusEffect.Defence, 3); }, true),

            //Motivation - Entferne: Ziehe 1 Karte. Permanent: Zu Rundenbeginn: Ziehe 1 Karte.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0,
            (CardInfo c) => {c.Player.DrawCards(1); c.Player.AddAction(
                (CardGameManager m) => m.Player.DrawCards(1), true); }, true),

            //Hedonistisch - Entferne: Ziehe 1 Karte. Permanent: Zu Rundenbeginn: Wähle eine beliebige Anzahl an Handkarten. Wirf die gewählten Handkarten auf den Ablagestapel und ziehe die selbe Anzahl an Karten.
            new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0, (CardInfo c) => {c.Player.DrawCards(1); }, true),

#warning TODO
            //Epikureimus - Entferne: Ziehe 1 Karte. Permanent: Zu Rundenbeginn: Wähle einen Würfel und erhöhe dessen Augenzahl um 2.
            //new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0, (CardInfo c) => {c.Player.DrawCards(1);c.Player.AddAction((CardGameManager m) => , true); }, true),

#warning TODO
            //Stoisch - Entferne: Ziehe 1 Karte. Permanent: Nicht verbrauchte Würfel werden nicht abgelegt.
            //new InstantCardInfo("", CardSet.Health, CardType.Skill, 0, 0, (CardInfo c) => {c.Player.DrawCards(1); }, true),

            #endregion Positiv

            #region Negative

#warning TODO
            //Erschöpft - Diese Runde: Reduziere die Augenzahl aller Würfel um 1. (Nie unter 1)
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

            //Verletzt - Entferne.
            new InstantCardInfo("Injured", CardSet.Health, CardType.Ailment, 0, 1, null, true),

            //Hunger - 8 Schwäche. 8 Entkräftet.
            new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => {c.Player.AddStatus(StatusEffect.Weak, 8); c.Player.AddStatus(StatusEffect.Feeble, 8); }),

#warning TODO
            //Terror - Diese Runde: 50% Chance das Karten nach ihrer Aktivierung, ohne Wirkung abgeworfen werden.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

#warning TODO
            //Trauma - Diese Runde: Alle gespielten Karten werden entfernt.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

#warning TODO
            //Fleischwunde - Diese Runde: Erhalte Blutung entsprechend Augenzahl, jedes Mal, wenn du Würfel verwendest.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

#warning TODO
            //Knochenbruch - 8 Betäubung. Diese Runde: 2 Vitalitäts-Schaden, jedes Mal, wenn du eine Karte spielst
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

#warning TODO
            //Toxin - Diese Runde: Löse den Würfel mit der höchsten Augenzahl auf.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

            //Hoffnungslos - Entferne alle Karten aus deinem Verteidigungsstapel.
            new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0,
            (CardInfo c) => c.Player.RemoveAllBlock() ),

            //Degeneration - Entferne: Reduziere Stärke und Schutz um 1.
            new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0,
            (CardInfo c) => {c.Player.RemoveStatus(StatusEffect.Strenght, 1, true); c.Player.RemoveStatus(StatusEffect.Defence, 1, true); }),

#warning TODO
            //Mutlos - Diese Runde: Du kannst keine Angriffe spielen.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

            //Psychose - Erhöhe deine Stärke um 2. 5 Verwundbarkeit.
            new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0,
            (CardInfo c) => {c.Player.AddStatus(StatusEffect.Strenght, 2); c.Player.AddStatus(StatusEffect.Vulnerable, 5); }),

            //Blutvergiftung - 8 Verwundbarkeit. 8 Betäubung
            new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0,
            (CardInfo c) => {c.Player.AddStatus(StatusEffect.Vulnerable, 8); c.Player.AddStatus(StatusEffect.Stun, 8); }),

#warning TODO
            //Tobsucht - Diese Runde: 4 Stärke, du kannst nur Angriffe spielen.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => c.Player.AddStatus(StatusEffect.FragileStrenght, 4)),

#warning TODO
            //Lähmender Schmerz - Diese Runde: Du kannst nicht mehr als 3 Karten spielen.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

#warning TODO
            //Krankheit - Diese Runde: Regeneration wird nicht angewandt.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

#warning TODO
            //Panikattacke - Du kannst keine anderen Karten spielen solange sich diese Karte auf deiner Hand befindet. Entferne.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

#warning TODO
            //Herzschwäche - Diese Runde: 4 Betäubung, jedes Mal, wenn du eine Karte spielst.
            //new InstantCardInfo("", CardSet.Health, CardType.Ailment, 0, 0, (CardInfo c) => ),

            #endregion Negative

            #endregion Health

            #region Items

            //Ei - Ziehe 1 Karte. Stelle 4 Vitalität her.
            new InstantCardInfo("", CardSet.Item, CardType.Aid, 0, 0,
            (CardInfo c) => { c.Player.DrawCards(1); c.Player.Heal(4); }),

            //Panacea - Ziehe 1 Karte. Entferne 20 Stapel von allen negativen Status.
            new InstantCardInfo("", CardSet.Item, CardType.Aid, 0, 0,
            (CardInfo c) => { c.Player.DrawCards(1); for (int i = (int)StatusEffect.Regeneration + 1; i < (int)StatusEffect.Vulnerable + 1; i++) c.Player.RemoveStatus((StatusEffect)i, 20); }),

            //Bandage - Ziehe 1 Karte. Entferne alle Blutungsstapel.
            new InstantCardInfo("", CardSet.Item, CardType.Aid, 0, 0,
            (CardInfo c) => { c.Player.DrawCards(1); c.Player.RemoveStatus(StatusEffect.Bleeding, c.Player.GetStatus(StatusEffect.Bleeding)); }),

            //Quellwasser - Ziehe 1 Karte. Stelle 8 Vitalität her.
            new InstantCardInfo("", CardSet.Item, CardType.Aid, 0, 0,
            (CardInfo c) => { c.Player.DrawCards(1); c.Player.Heal(8); }),

            //Schleifstein - Erhöhe deine Stärke um 5.
            new InstantCardInfo("", CardSet.Item, CardType.Aid, 0, 1,
            (CardInfo c) => c.Player.AddStatus(StatusEffect.Strenght, 5)),

            //Werkzeuge
            new TokenCardInfo("Tools", CardSet.Item, CardType.Quest),

            //Baumstamm
            new TokenCardInfo("Log", CardSet.Item, CardType.Quest),

            //Schlafkräuter
            new TokenCardInfo("Sleeping_Herbs", CardSet.Item, CardType.Quest),

            //Wein
            new TokenCardInfo("Wine", CardSet.Item, CardType.Quest),

            #endregion Items
        };
    }

    #region Properties

    public static CardInfo[] Cards => cards.Where(x => x.Name.Length > 0).ToArray();

    #endregion Properties

    #region Setup

    public static void Setup()
    {
        if (translationDoc == null || UserFile.Settings.Language != loadedLanguage)
            LoadLanguage(UserFile.Settings.Language);
    }

    public static void LoadLanguage(Language language)
    {
        translationDoc = new XmlDocument();
        translationDoc.LoadXml(Resources.Load<TextAsset>($"XML/Languages/{language.ToString()}").text);

        loadedLanguage = language;

        Debug.Log($"{language} was loaded.");
    }

    #endregion Setup

    #region Get

    public static int GetIndexOfName(string name)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].Name.Equals(name))
                return i;
        }

        return -1;
    }

    public static CardInfo GetCardByName(string name)
    {
        return cards.FirstOrDefault(x => x.Name.Equals(name));
    }

    public static CardInfo[] GetCardsByNames(params string[] names)
    {
        List<CardInfo> result = new List<CardInfo>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (names.Contains(cards[i].Name))
                result.Add(cards[i]);
        }

        return result.ToArray();
    }

    public static CardInfo GetItemCard(CardSet set)
    {
        return new InstantCardInfo(set.ToString(), set, CardType.Quest, -1, -1, (CardInfo c) => { });
    }

    public static XmlNode GetTranslationNode(string name)
    {
        return translationDoc.SelectSingleNode("Language/" + name);
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

    public static Sprite GetSprite(string name)
    {
        Sprite sprite = Resources.Load<Sprite>($"Textures/CardGame/Images/{name}");
        if (sprite != null)
            return sprite;

        Debug.Log($"Not found: {name}");
        return Resources.Load<Sprite>($"Textures/CardGame/Images/Debug");
    }

    #endregion Get

    #region Create

    public static void CreateLanugageFile()
    {
        string path = $"Assets\\Resources\\XML\\Languages\\Language_Template.xml";
        XmlDocument doc = new XmlDocument();
        XmlElement rootNode = doc.CreateElement("Language");
        doc.AppendChild(rootNode);

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].Name.Length < 1)
                continue;

            XmlElement cardNode = doc.CreateElement(cards[i].Name);
            cardNode.SetAttribute("Name", cards[i].Name.Replace("_", " "));
            cardNode.SetAttribute("Description", "");

            rootNode.AppendChild(cardNode);
        }

        doc.Save(path);
    }

    public static void RenameCardImages(CardInfo[] cards)
    {
        string path = "Assets\\Resources\\Textures\\CardGame\\Images";
        for (int i = 0; i < cards.Length; i++)
        {
            CardInfo current = cards[i];
            string translatedName = current.TranslatedName;

            string filepath = $"{path}\\{translatedName}.png";
            if (File.Exists(filepath))
            {
                //Move image file
                string newFilepath = $"{path}\\{current.Name}.png";
                File.Move(filepath, newFilepath);

                //move meta file
                if (File.Exists(filepath + ".meta"))
                    File.Move(filepath + ".meta", newFilepath + ".meta");
            }
        }
    }

    #endregion Create
}