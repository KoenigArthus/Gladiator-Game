using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DieObject : MonoBehaviour, IPointerClickHandler
{
    #region Fields

    private static GameObject diePrefab;
    private static Dictionary<int, (Sprite, Rect)> dieSprites = null;

    private DieInfo dieInfo;
    private Player player;

    private Image backgroundUI;
    private Text valueUI;

    #endregion Fields

    public static DieObject Instantiate(DieInfo dieInfo, Vector2 position)
    {
        if (dieInfo == null)
            return null;

        if (diePrefab == null)
            diePrefab = (GameObject)Resources.Load("Prefabs/CardGame/Die");

        GameObject obj = GameObject.Instantiate(diePrefab, GameObject.FindObjectOfType<Canvas>().transform);
        obj.transform.localPosition = position;

        DieObject die = obj.GetComponent<DieObject>();
        dieInfo.Setup(die);
        die.dieInfo = dieInfo;

        return die;
    }

    #region Properties

    public DieInfo Info => dieInfo;
    public Player Player { get => player; set => player = value; }
    public bool Selected => player != null && player.Prepareing != null && player.Prepareing.Info.HasDie(Info);

    #endregion Properties

    #region Main-Loop

    private void Awake()
    {
        if (dieSprites == null)
            dieSprites = new Dictionary<int, (Sprite, Rect)>()
            {
                {4, (Resources.Load<Sprite>("Textures/CardGame/Dice/W4"), new Rect(0, -15, 450, 250)) },
                {6, (Resources.Load<Sprite>("Textures/CardGame/Dice/W6"), new Rect(0, 0, 350, 350))},
                {8, (Resources.Load<Sprite>("Textures/CardGame/Dice/W8"), new Rect(0, 0, 250, 250)) },
                {10, (Resources.Load<Sprite>("Textures/CardGame/Dice/W10"), new Rect(0, 5, 150, 150)) },
                {12, (Resources.Load<Sprite>("Textures/CardGame/Dice/W12"), new Rect(0, 0, 200, 200)) },
                {20, (Resources.Load<Sprite>("Textures/CardGame/Dice/W20"), new Rect(0, 0, 140, 140)) },
            };

        backgroundUI = GetComponentInChildren<Image>();
        valueUI = GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (dieInfo != null)
        {
            int sides = dieInfo.Sides.Length;
            if (!dieSprites.ContainsKey(sides))
                return;

            var dieVisuals = dieSprites[sides];
            backgroundUI.sprite = dieVisuals.Item1;
            valueUI.rectTransform.localPosition = dieVisuals.Item2.position;
            valueUI.rectTransform.sizeDelta = dieVisuals.Item2.size;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Selected)
            valueUI.color = Color.red;
        else
            valueUI.color = Color.black;

        if (UserFile.Settings.DisableRomanNumbursOnDice)
            valueUI.text = dieInfo.Value.ToString();
        else
            valueUI.text = RomanNumberHelper.NumericToRoman(dieInfo.Value);
    }

    #endregion Main-Loop

    public void Roll()
    {
        dieInfo.Roll();
    }

    #region Pointer

    public void OnPointerClick(PointerEventData eventData)
    {
        if (player != null && player.Prepareing != null)
            player.Prepareing.Info.ToggleDie(Info);
    }

    #endregion Pointer
}