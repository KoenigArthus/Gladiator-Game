using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DieCollection : MonoBehaviour, IPointerEnterHandler
{
    #region Fields

    public bool expandDown = false;
    public bool expandLeft = false;

    private DieObject[] dice = new DieObject[0];

    private float expansion = 0;
    private bool hovered = false;

    #endregion Fields

    #region Properties

    public DieObject[] Dice { get => dice; set => dice = value; }

    #endregion Properties

    #region Main-Loop

    // Update is called once per frame
    private void Update()
    {
        dice = dice.Where(x => x == null || !x.IsDestroyed()).ToArray();
    }

    private void FixedUpdate()
    {
        float cursorDelta = (transform.position - Input.mousePosition).magnitude;
        float xOffset = 150 * (expandLeft ? -1 : 1);
        float yOffset = 150 * (expandDown ? -1 : 1);

        if (cursorDelta > 350)
            hovered = false;

        if (hovered)
            expansion += 0.1f;
        else
            expansion -= 0.25f;
        expansion = Mathf.Clamp01(expansion);

        int columns = dice.Length / 4 + 1;
        for (int i = 0; i < dice.Length; i++)
        {
            DieObject current = dice[i];
            if (current == null || current.IsDestroyed())
                continue;

            Vector2 targetPosition = new Vector2(
                xOffset * (i % columns),
                yOffset * (i / (float)columns)
                ) * expansion;

            current.transform.localPosition = targetPosition;
        }
    }

    #endregion Main-Loop

    public void Add(DieObject die)
    {
        dice = dice.Concat(new DieObject[] { die }).ToArray();
        die.transform.parent = this.transform;
    }

    public void Clear()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            if (dice[i] != null && !dice[i].IsDestroyed())
                GameObject.Destroy(dice[i].gameObject);

            dice[i] = null;
        }

        dice = new DieObject[0];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }
}