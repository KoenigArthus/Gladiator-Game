using System.Collections.Generic;
using System.Linq;

public class UIValues
{
    #region Fields

    private float playerHealth;
    private float playerBlock;
    private float enemyHealth;
    private float enemyBlock;

    private StatusEffectInfo[] playerStatusEffects;
    private StatusEffectInfo[] enemyStatusEffects;

    #endregion Fields

    public UIValues(Player player, Enemy enemy)
    {
        playerHealth = player.Health;
        playerBlock = player.Block;
        enemyHealth = enemy.Health;
        enemyBlock = enemy.Block;

        List<StatusEffectInfo> playerStatusEffects = new List<StatusEffectInfo>();
        List<StatusEffectInfo> enemyStatusEffects = new List<StatusEffectInfo>();
        for (int i = 0; i < 11; i++)
        {
            StatusEffect effect = (StatusEffect)i;
            playerStatusEffects.Add(new StatusEffectInfo(effect, player));
            enemyStatusEffects.Add(new StatusEffectInfo(effect, enemy));
        }
        this.playerStatusEffects = playerStatusEffects.Where(x => x.Value != 0).ToArray();
        this.enemyStatusEffects = enemyStatusEffects.Where(x => x.Value != 0).ToArray();
    }

    #region Properties

    public float PlayerHealth => playerHealth;
    public float PlayerBlock => playerBlock;
    public float EnemyHealth => enemyHealth;
    public float EnemyBlock => enemyBlock;

    public StatusEffectInfo[] PlayerStatusEffects => playerStatusEffects;
    public StatusEffectInfo[] EnemyStatusEffects => enemyStatusEffects;

    #endregion Properties
}