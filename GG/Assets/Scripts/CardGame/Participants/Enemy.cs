using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public enum EnemyBehavior
{
    None, Aggressive, Defensive, Balanced, Tactical
}

public class Enemy : Participant
{
    #region Fields

    private static readonly (int, int)[] behaviorValues = new (int, int)[]
    {
        (0, 100),
        (50, 30),
        (20, 60),
        (40, 40),
        (35, 35)
    };

    private int[] blockStack = new int[0];
    private EnemyBehavior behavior;
    private Intension[] intensions;

    #endregion Fields

    public Enemy(CardGameManager manager, int health, int blockSlots, EnemyBehavior behavior) : base(manager, health, blockSlots)
    {
        ChangeIntension();
        this.behavior = behavior;
    }

    #region Properties

    public string Intension => string.Join('-', intensions.Select(x => x.ToString()));
    public override int[] BlockStack => blockStack;

    #endregion Properties

    #region Play

    public void ChangeIntension()
    {
        EnemyBehavior behavior = this.behavior;
        if (behavior == EnemyBehavior.Tactical)
        {
            int block = Block;
            if (block > 10)
                behavior = EnemyBehavior.Aggressive;
            else if (block < 5)
                behavior = EnemyBehavior.Defensive;
            else
                behavior = EnemyBehavior.Balanced;
        }

        (int, int) behaviorValues = Enemy.behaviorValues[(int)behavior];

        intensions = new Intension[]
        {
            new Intension(behaviorValues.Item1, behaviorValues.Item2),
            new Intension(behaviorValues.Item1, behaviorValues.Item2),
            new Intension(behaviorValues.Item1, behaviorValues.Item2)
        };
    }

    public void TakeTurn(Player target)
    {
        for (int i = 0; i < intensions.Length; i++)
        {
            Intension current = intensions[i];
            if (current.Type == Action.Attack)
                this.Attack(target, Random.Range(1, 5));
            else if (current.Type == Action.Block)
            {
                blockStack = blockStack.Concat(new int[] { Random.Range(2, 7) }).ToArray();
                if (blockStack.Length > BlockSlots)
                    blockStack = blockStack.Skip(1).ToArray();
            }
        }
    }

    #endregion Play

    #region Defend

    protected override void ReduceBlock(ref int amount)
    {
        for (int i = 0; i < blockStack.Length; i++)
        {
            if (amount < blockStack[i])
            {
                blockStack[i] -= amount;
                amount = 0;
                break;
            }
            else
            {
                amount -= blockStack[i];
                blockStack[i] = 0;
            }
        }

        blockStack = blockStack.Where(x => x > 0).ToArray();
    }

    #endregion Defend

    public override string ToString()
    {
        return base.ToString() +
            $"\nIntend: {Intension}";
    }
}