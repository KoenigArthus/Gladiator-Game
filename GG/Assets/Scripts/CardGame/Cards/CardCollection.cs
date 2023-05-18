using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    #region Fields

    public CardCollectionType collectionType;
    public bool cardsInteractable = false;
    public bool showCardsAsRevealed = true;
    public bool offsetHovered = false;

    private List<CardObject> cards = new List<CardObject>();
    private bool changed = false;
    private Player player;

    #endregion Fields

    #region Properties

    public int Count => cards.Count;
    public CardObject[] Cards => cards.ToArray();
    public bool Changed => changed;
    public Player Player { get => player; set => player = value; }

    #endregion Properties

    #region Main-Loop

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = cards.Count - 1; i > -1; i--)
        {
            CardObject current = cards[i];

            if (current == null || current.Collection != this)
            {
                cards.RemoveAt(i);
                continue;
            }

            if (!current.Draging && !current.Prepareing)
            {
                if (this.transform is RectTransform transform)
                {
                    float width = Mathf.Min(transform.rect.width, current.Width * current.Scale * cards.Count);

                    current.transform.SetSiblingIndex(i);

                    switch (collectionType)
                    {
                        case CardCollectionType.Stack:
                            current.TargetPosition = new Vector3(0, 0, 0);
                            current.transform.rotation = new Quaternion();
                            break;

                        case CardCollectionType.List:
                            {
                                float offset = 0;
                                if (cards.Count > 1)
                                    offset = width / (cards.Count - 1) * i - width * 0.5f;
                                if (offsetHovered && current.Hovered)
                                    current.TargetPosition = new Vector3(offset, 300, 0);
                                else
                                    current.TargetPosition = new Vector3(offset, 0, 0);
                                current.transform.rotation = new Quaternion();
                                break;
                            }

                        case CardCollectionType.Fan:
                            float angle = 0;
                            if (cards.Count > 1)
                            {
                                angle = 90 / (cards.Count - 1) * i - 45;
                            }
                            current.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                            angle += 90;
                            current.TargetPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 100;
                            break;
                    }
                }
            }
        }

        if (CardObject.HoveredCard != null)
            CardObject.HoveredCard.transform.SetAsLastSibling();
    }

    #endregion Main-Loop

    public void Shuffle()
    {
        var cards = this.cards.ToArray();
        cards.Shuffle();
        this.cards = cards.ToList();
    }

    public void Add(CardObject card)
    {
        card.Collection = this;
        cards.Add(card);
        card.transform.parent = this.transform;

        changed = true;
    }

    public void Remove(CardObject card)
    {
        if (card.Collection == this)
            card.Collection = null;

        cards.Remove(card);

        changed = true;
    }

    public CardObject DrawCard()
    {
        if (cards.Count > 0)
        {
            int index = cards.Count - 1;
            CardObject card = cards[index];
            cards.RemoveAt(index);

            changed = true;

            return card;
        }

        return null;
    }

    public void MoveAllTo(CardCollection collection)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            collection.Add(cards[i]);
        }
        cards = new List<CardObject>();

        changed = true;
    }

    public void ChangeHandeled()
    {
        changed = false;
    }
}