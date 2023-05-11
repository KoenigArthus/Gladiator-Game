using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Fields

    public bool interactable = true;
    public float speed;
    public float arcAngle;

    private static GameObject cardPrefab;
    private static Sprite cardBack;
    private static CardObject hoveredCard = null;

    private CardInfo cardInfo = null;
    private CardCollection collection = null;

    private Sprite cardFront;
    private Image cardImage;
    private Text nameUI;
    private Text setUI;
    private Text typeUI;
    private Text descriptionUI;

    private Vector3 targetPosition;
    private bool draging = false;
    private Vector2 dragOffset;

    #endregion Fields

    public static CardObject Instantiate(CardInfo cardInfo, Vector2 position)
    {
        if (cardInfo == null)
            return null;

        if (cardPrefab == null)
            cardPrefab = (GameObject)Resources.Load("Prefabs/CardGame/Card");

        GameObject obj = GameObject.Instantiate(cardPrefab, GameObject.FindObjectOfType<Canvas>().transform);
        obj.transform.localPosition = position;

        CardObject card = obj.GetComponent<CardObject>();
        cardInfo.Setup(card);
        card.cardInfo = cardInfo;

        return card;
    }

    #region Properties

    public CardInfo Info => cardInfo;
    public CardCollection Collection { get => collection; set => collection = value; }
    public bool Interactable => interactable && (collection == null || collection.cardsInteractable);
    public bool Hovered => this == hoveredCard;

    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }

    public bool Draging => draging;
    public bool Prepareing => collection != null && collection.Player != null && this == collection.Player.Prepareing;

    public static CardObject HoveredCard => hoveredCard;

    #endregion Properties

    #region Main-Loop

    private void Awake()
    {
        if (cardBack == null)
            cardBack = Resources.Load<Sprite>("Textures/CardGame/back");

        //cardFront = Resources.Load<Sprite>($"Textures/CardGame/Images/{name}");
        cardFront = Resources.Load<Sprite>($"Textures/CardGame/Images/Debug");

        Text[] texts = this.GetComponentsInChildren<Text>();

        nameUI = texts[0];
        setUI = texts[1];
        typeUI = texts[2];
        descriptionUI = texts[3];

        cardImage = GetComponentInChildren<Image>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        updateCardUI();
    }

    // Update is called once per frame
    private void Update()
    {
        updateCardUI();

        if (!Draging)
        {
            Vector3 delta = targetPosition - transform.localPosition;
            float deltaLength = delta.magnitude;

            if (!(deltaLength > 0) || deltaLength < speed)
                transform.localPosition = targetPosition;
            else
            {
                float deltaAngle = Mathf.Atan2(delta.y, delta.x);
                if (deltaAngle > -Mathf.PI * 0.5f && deltaAngle < Mathf.PI * 0.5f)
                    deltaAngle += arcAngle;
                else
                    deltaAngle -= arcAngle;
                delta = new Vector3(Mathf.Cos(deltaAngle), Mathf.Sin(deltaAngle), delta.z);
                transform.localPosition += delta * speed;
            }
        }
    }

    private void updateCardUI()
    {
        bool revealed = collection == null || collection.showCardsAsRevealed;

        nameUI.enabled = revealed;
        setUI.enabled = revealed;
        typeUI.enabled = revealed;
        descriptionUI.enabled = revealed;

        if (!revealed)
        {
            cardImage.sprite = cardBack;
            return;
        }

        cardImage.sprite = cardFront;

        nameUI.text = cardInfo.TranslatedName;
        setUI.text = $"{cardInfo.Set}";
        typeUI.text = cardInfo.Type.ToString();
        descriptionUI.text = $"Cost: {Info.Cost}";

        if (Info is BlockCardInfo blockCard && blockCard.CurrentBlock > 0)
            descriptionUI.text += $"\nBlock: {blockCard.CurrentBlock}";
    }

    #endregion Main-Loop

    #region Pointer

    #region Hover

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredCard = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Hovered)
            hoveredCard = null;
    }

    #endregion Hover

    #region Drag

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Interactable)
        {
            if (collection.Player.Prepareing != null)
                collection.Player.AbortPreparedCardPlay();

            draging = true;
            dragOffset = (Vector2)transform.position - eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Move GameObject
        if (draging)
            transform.position = eventData.position + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draging = false;
        if (transform.localPosition.y > 500 && collection != null && collection.Player != null)
            collection.Player.PrepareCard(this);
    }

    #endregion Drag

    #endregion Pointer
}