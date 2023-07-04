public class EnrageInfo
{
    #region Fields

    private EnemyEnrageAction action;
    private int threshold;

    #endregion Fields

    public EnrageInfo(EnemyEnrageAction action, int threshold)
    {
        this.action = action;
        this.threshold = threshold;
    }

    #region Properties

    public int Threshold { get => threshold; set => threshold = value; }

    #endregion Properties

    public void Execute(Enemy enemy)
    {
        action(this, enemy);
    }
}