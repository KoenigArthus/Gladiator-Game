using UnityEngine;

public class SkillInfo
{
    #region Fields

    private EnemySkillAction action;
    private int executeCount = 0;

    private EnemyIntension intension;
    private int chance = 1;
    private (int, int) range;
    private int repeat = 1;

    #endregion Fields

    #region ctor

    public SkillInfo(EnemyIntension intension, int chance, (int, int) range, EnemySkillAction action = null)
    {
        if (action == null)
            switch (intension)
            {
                case EnemyIntension.Attack:
                case EnemyIntension.Special:
                    action = (SkillInfo s, Enemy e) => e.Attack(e.Player, s.GetPower());
                    break;

                case EnemyIntension.Block:
                    action = (SkillInfo s, Enemy e) => e.AddBlock(s.GetPower());
                    break;
            }

        this.action = action;

        this.intension = intension;
        this.chance = chance;
        this.range = range;
    }

    public SkillInfo(EnemyIntension intension, int chance, int range, EnemySkillAction action = null) :
        this(intension, chance, (range, range), action)
    {
    }

    public SkillInfo(EnemyIntension intension, (int, int) range, EnemySkillAction action = null) :
        this(intension, 1, range, action)
    {
    }

    public SkillInfo(EnemyIntension intension, int range, EnemySkillAction action = null) :
        this(intension, 1, range, action)
    {
    }

    #endregion ctor

    #region Properties

    public EnemySkillAction Action { get => action; set => action = value; }
    public int ExecuteCount => executeCount;
    public EnemyIntension Intension => intension;
    public (int, int) Range { get => range; set => range = value; }
    public int Power { get => GetPower(); set => Range = (value, value); }
    public int Repeat { get => repeat; set => repeat = value; }

    #endregion Properties

    public void Execute(Enemy enemy)
    {
        executeCount += 1;
        action(this, enemy);
    }

    public int GetChance(Enemy enemy)
    {
        if (enemy.DontOverstackBlock && intension == EnemyIntension.Block && enemy.BlockStack.Length > 2)
            return 0;

        return chance;
    }

    public int GetPower()
    {
        if (range.Item1 == range.Item2)
            return range.Item1;

        return Random.Range(range.Item1, range.Item2);
    }

    public override string ToString()
    {
        return $"{intension} ({range.Item1}-{range.Item2})";
    }
}