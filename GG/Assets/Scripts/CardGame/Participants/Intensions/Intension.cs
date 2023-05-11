using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intension
{
    private Action type;

    public Intension(Action type)
    {
        this.type = type;
    }

    public Intension(int attackChance, int defendChance)
    {
        int rng = Random.Range(0, 100);
        if (rng < attackChance)
            type = Action.Attack;
        else if (rng < attackChance + defendChance)
            type = Action.Block;
        else
            type = Action.Special;
    }

    public Action Type { get => type; set => type = value; }

    public override string ToString()
    {
        return type.ToString()[0].ToString();
    }
}