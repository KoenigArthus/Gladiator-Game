using System;

public struct RewardInfo
{
    #region Fields

    private int goldOnWin;
    private int goldOnSpare;
    private int expOnWin;
    private int expOnSpare;
    private int expOnLose;

    #endregion Fields

    #region ctor

    /// <summary>
    /// All Parameters | Never used in game (yet)
    /// </summary>
    public RewardInfo(int expOnWin, int expOnSpare, int expOnLose, int goldOnWin, int goldOnSpare)
    {
        this.expOnWin = expOnWin;
        this.expOnSpare = expOnSpare;
        this.expOnLose = expOnLose;
        this.goldOnWin = goldOnWin;
        this.goldOnSpare = goldOnSpare;
    }

    /// <summary>
    /// Win and spare rewards (nothing on lose) | Boss fights with spare
    /// </summary>
    public RewardInfo(int expOnWin, int expOnSpare, int goldOnWin, int goldOnSpare) : this(expOnWin, expOnSpare, 0, goldOnWin, goldOnSpare)
    {
    }

    /// <summary>
    /// Only win rewards | Boss fights without spare
    /// </summary>
    public RewardInfo(int expOnWin, int goldOnWin) : this(expOnWin, 0, goldOnWin, 0)
    {
    }

    /// <summary>
    /// Win - Lose EXP | Training Fights
    /// </summary>
    public RewardInfo((int, int) exp) : this(0, 0, exp.Item1, 0, exp.Item2)
    {
    }

    /// <summary>
    /// Only EXP | Tutorial Fight
    /// </summary>
    public RewardInfo(int expOnWin) : this(0, expOnWin)
    {
    }

    #endregion ctor

    public void ApplyRewards(BattleResult battleResult)
    {
        int gold = 0;
        int exp = 0;

        switch (battleResult)
        {
            case BattleResult.Lose:
                exp = expOnLose;
                break;

            case BattleResult.Spare:
                gold = goldOnSpare;
                exp = expOnSpare;
                break;

            case BattleResult.Win:
                gold = goldOnWin;
                exp = expOnWin;
                break;
        }

        //Gold
        UserFile.SaveGame.Gold += gold;

        //EXP
        string[] equipment = UserFile.SaveGame.EquipmentCardEntries;
        for (int i = 0; i < equipment.Length; i++)
        {
            CardSet current;
            if (!Enum.TryParse(equipment[i], out current))
                continue;

            new EquipmentInfo(current).EXP += exp;
        }
    }
}