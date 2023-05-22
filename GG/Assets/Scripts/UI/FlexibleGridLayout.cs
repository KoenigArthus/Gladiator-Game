using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Heigt
    }


    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        float sqrRt = Mathf.Sqrt(transform.childCount);
        rows = Mathf.CeilToInt(sqrRt);
        columns = Mathf.CeilToInt(sqrRt);

        if(fitType == FitType.Width)
        {
          rows = Mathf.CeilToInt(rows);
        }

        if (fitType == FitType.Heigt)
        {

        }

        if (fitType == FitType.Uniform)
        {

        }


        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / (float)columns - ((spacing.x / (float)columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = parentHeight / (float)rows - ((spacing.y / (float)rows) * 2)- (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        int rowCount = 0;
        int columnCount = 0;

        for(int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x + spacing.x) * columnCount + padding.left;
            var yPos = (cellSize.y + spacing.y) * rowCount + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);


        }


    }



    public override void CalculateLayoutInputVertical()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutHorizontal()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutVertical()
    {
        //throw new System.NotImplementedException();
    }
}
