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
        Height,
        FixedRows,
        FixedColumns
    }

    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;

            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }


        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);

        if (fitType == FitType.Height || fitType == FitType.FixedRows)
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);


        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / columns - (spacing.x / columns * 2) - (padding.left / columns) - (padding.right / columns);
        float cellHeight = parentHeight / rows - (spacing.y / rows * 2) - (padding.top / rows) - (padding.bottom / rows);


        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int rowCount = 0;
        int columnCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            RectTransform previousItem = null;

            /*if(rectChildren[i - 1] != null)
            {
               previousItem = rectChildren[i - 1];
            }*/

            var xPos = 0f;
            var yPos = 0f;

            /*if (item.gameObject.GetComponent<LayoutElement>() != null)
            {
                LayoutElement layoutElement = item.gameObject.GetComponent<LayoutElement>();

                xPos = (layoutElement.preferredWidth * columnCount) + (spacing.x * columnCount) + padding.left - (item.rect.width);
                yPos = (layoutElement.preferredHeight * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, layoutElement.preferredWidth );
                SetChildAlongAxis(item, 1, yPos, layoutElement.preferredHeight);
            }*/
            //else
            //{
                xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            //}



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
