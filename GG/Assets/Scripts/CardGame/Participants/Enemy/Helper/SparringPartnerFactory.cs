using UnityEngine.UIElements;

public partial class Enemy
{
    /// <summary>
    /// Add sparring partner specific resources to base enemy from BuildEnemy()
    /// </summary>
    public static Enemy BuildSparringPartner(Enemy enemy, EnemyType type)
    {
        enemy = BuildEnemy(enemy, type);

        switch (type)
        {
            case EnemyType.Murmillo:
                enemy.health = 30;
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => { e.AddStatus(StatusEffect.Strength, 1); e.AddStatus(StatusEffect.Defence, 1); });
                enemy.reward = new RewardInfo((5, 2));
                break;

            case EnemyType.Retiarius:
                enemy.health = 20;
                enemy.reward = new RewardInfo((3, 1));
                break;

            case EnemyType.Trax:
                enemy.health = 25;
                enemy.Actions = 3;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, (6, 10)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (8, 12), (SkillInfo s, Enemy e) => {e.Attack(e.Player, s.GetPower() ); e.Player.AddStatus(StatusEffect.Vulnerable, 3); })
                };
                enemy.AddStatus(StatusEffect.Regeneration, 3);
                enemy.reward = new RewardInfo((3, 1));
                break;

            case EnemyType.Hoplomachus:
                enemy.health = 75;
                enemy.skills[1].Range = (10, 24);
                enemy.enrage.Threshold = 20;
                break;

            case EnemyType.Scissor:
                enemy.health = 50;
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => { e.RemoveAllBlock(); e.AddBlock(20); });
                enemy.reward = new RewardInfo((10, 2));
                break;

            case EnemyType.Dimachaerus:
                enemy.health = 80;
                enemy.skills[2].Power = 10;
                enemy.enrage.Threshold = 50;
                break;

            case EnemyType.Schwerathlet:
                enemy.health = 220;
                enemy.Actions = 6;
                enemy.skills[1].Power = 20;
                enemy.forcedIntension = new EnemyIntension?[] { null, null, null, null, null, EnemyIntension.Special };
                enemy.reward = new RewardInfo((12, 3));
                break;

            case EnemyType.Sklaventreiber:
                enemy.health = 300;
                enemy.skills[1].Range = (12, 18);
                enemy.enrage.Threshold = 150;
                enemy.AddStatus(StatusEffect.Regeneration, 14);
                enemy.reward = new RewardInfo((18, 4));
                break;

            case EnemyType.Sonnenbringer:
                enemy.health = 200;
                enemy.reward = new RewardInfo((20, 4));
                break;

            case EnemyType.Krieger:
                enemy.health = 550;
                enemy.skills[0].Range = (25, 45);
                enemy.skills[1].Range = (35, 50);
                enemy.reward = new RewardInfo((30, 5));
                break;
        }

        return enemy;
    }
}