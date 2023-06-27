using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using JSAM;

public class CardObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Fields

    public bool interactable = true;
    public float speed;
    public float arcAngle;
    public CharacterRotation characterRotation;

    private static GameObject cardPrefab;
    private static Sprite cardBack;
    private static CardObject hoveredCard = null;

    private CardInfo cardInfo = null;
    private CardCollection collection = null;

    public CharacterRotation CharacterRotation => characterRotation;

    private Canvas canvas;
    private Sprite cardFront;
    private Image cardImage;
    private Text nameUI;
    private Text setUI;
    private Text typeUI;
    private Text descriptionUI;

    private Vector3 startPosition = Vector3.zero;
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
        card.ChangeInfo(cardInfo);

        return card;
    }

    #region Properties

    public CardInfo Info => cardInfo;
    public CardCollection Collection { get => collection; set => collection = value; }
    public bool Interactable => interactable && (collection == null || collection.cardsInteractable) && (collection?.Player == null || !collection.Player.Busy);
    public bool Hovered => this == hoveredCard;

    public Vector3 TargetPosition
    { get => targetPosition; set { startPosition = transform.position; targetPosition = value; } }

    public float Width => cardImage.rectTransform.rect.width;
    public float Scale => canvas.transform.lossyScale.x;

    public bool Draging => draging;
    public bool Prepareing => collection != null && collection.Player != null && this == collection.Player.Prepareing;

    public static CardObject HoveredCard => hoveredCard;

    #endregion Properties

    #region Main-Loop

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        Text[] texts = this.GetComponentsInChildren<Text>();

        nameUI = texts[0];
        setUI = texts[1];
        typeUI = texts[2];
        descriptionUI = texts[3];

        cardImage = GetComponentInChildren<Image>();

        characterRotation = FindObjectOfType<CharacterRotation>();
    }

    private void Start()
    {
        if (cardBack == null)
            cardBack = Resources.Load<Sprite>("Textures/CardGame/back");

        cardFront = Info.Sprite;
    }

    // Update is called once per frame
    private void Update()
    {
        updateCardUI();
    }

    private void FixedUpdate()
    {
        if (!Draging)
        {
            Vector3 delta = targetPosition - transform.localPosition;
            float deltaLength = delta.magnitude;
            float speed = this.speed;

            //Make card "fly" to destination
            if (!(deltaLength > 0) || deltaLength < speed)
                transform.localPosition = targetPosition;
            else
            {
                float deltaAngle = Mathf.Atan2(delta.y, delta.x);
                {
                    Vector3 totalDelta = targetPosition - startPosition;
                    float totalDeltaAngle = Mathf.Atan2(delta.y, delta.x);
                    if (totalDeltaAngle > -Mathf.PI * 0.5f && totalDeltaAngle < Mathf.PI * 0.5f)
                        deltaAngle += arcAngle;
                    else
                        deltaAngle -= arcAngle;
                }
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
        descriptionUI.text =
            $"{(Info.Cost > -1 + (Info.Type == CardType.Ailment ? 1 : 0) ? $"{CardLibrary.GetTranslatedName("Cost")}: {Info.Cost}" : $"[{CardLibrary.GetTranslatedName("Cost")}]")}\n" +
            $"{Info.TranslatedDescription}";

        if (Info is BlockCardInfo blockCard && blockCard.CurrentBlock > 0 && blockCard.Player != null)
            descriptionUI.text += $"\nBlock: {blockCard.CurrentBlock}";

        if (!UserFile.Settings.DisableRomanNumbursOnDice)
            descriptionUI.text = RomanNumberHelper.ReplaceNumbersWithRoman(descriptionUI.text);
    }

    #endregion Main-Loop

    public void ChangeInfo(CardInfo cardInfo)
    {
        if (cardInfo == null)
            return;

        this.cardInfo = cardInfo;
        cardInfo.Setup(this);

        updateCardUI();
    }

    #region Pointer

    #region Hover

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredCard = this;
        JSAM.AudioManager.PlaySound(Sounds.Hover);
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
        JSAM.AudioManager.PlaySound(Sounds.CardPlayed);


        if (transform.localPosition.y > 500 && collection != null && collection.Player != null)
        {

            collection.Player.PrepareCard(this);
            characterRotation.StopMovement();
            Debug.Log("Stopped Movement");

        }


    }

    #endregion Drag

    #endregion Pointer
}