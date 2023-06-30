public class UIValues
{
    #region Fields

    private float playerHealth;
    private float playerBlock;
    private float enemyHealth;
    private float enemyBlock;

    private int[] playerStatusEffectStacks = new int[CardGameConstants.STATUS_EFFECT_COUNT];
    private int[] enemyStatusEffectStacks = new int[CardGameConstants.STATUS_EFFECT_COUNT];

    #endregion Fields

    public UIValues(Player player, Enemy enemy)
    {
        playerHealth = player.Health;
        playerBlock = player.Block;
        enemyHealth = enemy.Health;
        enemyBlock = enemy.Block;

        for (int i = 0; i < playerStatusEffectStacks.Length; i++)
        {
            playerStatusEffectStacks[i] = player.GetStatus((StatusEffect)i);
        }

        for (int i = 0; i < enemyStatusEffectStacks.Length; i++)
        {
            enemyStatusEffectStacks[i] = enemy.GetStatus((StatusEffect)i);
        }
    }

    #region Properties

    public float PlayerHealth => playerHealth;
    public float PlayerBlock => playerBlock;
    public float EnemyHealth => enemyHealth;
    public float EnemyBlock => enemyBlock;
    public int[] PlayerStatusEffectStacks => playerStatusEffectStacks;
    public int[] EnemyStatusEffectStacks => enemyStatusEffectStacks; 

    #endregion Properties
}