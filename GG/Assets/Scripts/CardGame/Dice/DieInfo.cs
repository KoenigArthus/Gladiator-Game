using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DieInfo
{
    #region Fields

    private readonly int[] validSideCounts = new int[] { 4, 6, 8, 12, 20 };
    private int bonus = 0;
    private string tag = "";

    private DieObject die;

    private int[] sides;
    private int index = 0;

    #endregion Fields

    #region ctor

    public DieInfo(int sideCount)
    {
        if (sideCount < 2)
            throw new System.ArgumentException("Die must have more than one side");

        int totalSideCount = sideCount;
        //while (!validSideCounts.Contains(totalSideCount))
        //{
        //    if (totalSideCount < 21)
        //        totalSideCount *= 2;
        //    else
        //        throw new System.ArgumentException("SideCount not valid");
        //}

        this.sides = new int[totalSideCount];
        for (int i = 0; i < sides.Length; i++)
        {
            this.sides[i] = i % sideCount + 1;
        }
    }

    public DieInfo(params int[] sides)
    {
        if (sides == null)
            throw new System.ArgumentNullException();
        else if (sides.Length < 2)
            throw new System.ArgumentException("Die must have more than one side");

        this.sides = sides;

        //while (!validSideCounts.Contains(sides.Length))
        //{
        //    if (sides.Length < 21)
        //        this.sides = this.sides.Concat(sides).ToArray();
        //    else
        //        throw new System.ArgumentException("SideCount not valid");
        //}
    }

    #endregion ctor

    #region Setup

    public void Setup(DieObject die)
    {
        this.die = die;
    }

    #endregion Setup

    #region Properties

    public int[] Sides => sides;
    public int TrueValue => sides[index];
    public int Value => Mathf.Max(1, sides[index] + bonus);
    public DieObject Die => die;
    public bool Rolling => die != null && die.Rolling;

    public int Bonus { get => bonus; set => bonus = value; }
    public string Tag { get => tag; set => tag = value; }

    #endregion Properties

    public void Roll()
    {
        index = Random.Range(0, sides.Length);
    }

    public void SpinUp(int amount)
    {
        index = Mathf.Min(sides.Length - 1, index + amount);
    }

    public void SpinDown(int amount)
    {
        index = Mathf.Min(0, index - amount);
    }
}