using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intension
{
    private EnemyAction type;

    public Intension(EnemyAction type)
    {
        this.type = type;
    }

    public Intension(int attackChance, int defendChance)
    {
        int rng = Random.Range(0, 100);
        if (rng < attackChance)
            type = EnemyAction.Attack;
        else if (rng < attackChance + defendChance)
            type = EnemyAction.Block;
        else
            type = EnemyAction.Special;
    }

    public EnemyAction Type { get => type; set => type = value; }

    public override string ToString()
    {
        return type.ToString()[0].ToString();
    }
}