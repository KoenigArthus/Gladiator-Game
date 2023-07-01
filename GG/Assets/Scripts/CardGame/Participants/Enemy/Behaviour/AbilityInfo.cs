using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInfo
{
    #region Fields

    private EnemyAbilityAction action;
    private int frequency = 1;
    private bool cantUseSkillsThisTurn = false;

    #endregion Fields

    public AbilityInfo(EnemyAbilityAction action)
    {
        this.action = action;
    }

    #region Properties

    public int Frequency { get => frequency; set => frequency = value; }
    public bool CantUseSkillsThisTurn { get => cantUseSkillsThisTurn; set => cantUseSkillsThisTurn = value; }

    #endregion Properties

    public void Execute(Enemy enemy)
    {
        action(this, enemy);
    }
}