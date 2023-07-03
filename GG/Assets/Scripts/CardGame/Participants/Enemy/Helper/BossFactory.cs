using System;

public partial class Enemy
{
    /// <summary>
    /// Add boss specific resources to base enemy from BuildEnemy()
    /// </summary>
    public static Enemy BuildBoss(Enemy enemy, EnemyType type)
    {
        enemy = BuildEnemy(enemy, type);

        switch (type)
        {
            case EnemyType.Tutorialgladiator:
                enemy.health = 16;
                enemy.reward = new RewardInfo(3, 30);
                enemy.attackAilments = new AttackAilmentInfo(0.1f, Ailment.Hopeless, Ailment.Panic_Attack);
                break;

            case EnemyType.Murmillo:
                enemy.health = 60;
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => { e.AddStatus(StatusEffect.Strength, 2); e.AddStatus(StatusEffect.Defence, 2); e.Heal(15); })
                { Frequency = 3, CantUseSkillsThisTurn = false };
                enemy.enrage = new EnrageInfo((EnrageInfo l, Enemy e) => e.Actions = 1, 25);
                enemy.reward = new RewardInfo(8, 30);
                enemy.attackAilments = new AttackAilmentInfo(0.25f, Ailment.Bone_Fracture, Ailment.Injured);
                break;

            case EnemyType.Trax:
                enemy.health = 40;
                enemy.Actions = 4;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, 3, (4, 8)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, 3, (8, 12), (SkillInfo s, Enemy e) =>
                    {
                        //Jeder Angriff, welcher nicht geblockt wird fügt 1 Blutung zu.
                        if (e.Player.Block < 1) e.Player.AddStatus(StatusEffect.Bleeding, 1);
                        //Reduziert den Schutz des Spielers um 1 mit jedem dritten Angriff.
                        if (s.ExecuteCount % 3 == 0) e.Player.RemoveStatus(StatusEffect.Defence, 1, true);
                        //Angriffe: 4-16
                        e.Attack(e.Player, s.GetPower());
                    }),
                    //Skill
                    new SkillInfo(EnemyIntension.Attack, 1, (4, 6), (SkillInfo s, Enemy e) =>
                    //4-6 Verwundbarkeit auf den Spieler zu wirken
                    e.Player.AddStatus(StatusEffect.Vulnerable, s.GetPower()))
                    };
                enemy.forcedIntension = new EnemyIntension?[] { EnemyIntension.Block, EnemyIntension.Attack, EnemyIntension.Attack, EnemyIntension.Attack };
                //Solange er blockt werden dem Spieler 2 Schaden pro Angriff gegen Block mitgegeben.

                enemy.AddStatus(StatusEffect.Regeneration, 5);
                enemy.reward = new RewardInfo(6, 50);
                enemy.attackAilments = new AttackAilmentInfo(0.2f, Ailment.Bone_Fracture);
                break;

            case EnemyType.Hoplomachus:
                enemy.health = 100;
                enemy.attackAilments = new AttackAilmentInfo(0.2f, Ailment.Blood_Poisoning, Ailment.Injured);
                break;

            case EnemyType.Scissor:
                enemy.health = 50;
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => { e.RemoveAllBlock(); e.AddBlock(30); });
                enemy.reward = new RewardInfo(15, 100);
                enemy.attackAilments = new AttackAilmentInfo(0.1f, Ailment.Blood_Poisoning);
                break;

            case EnemyType.Dimachaerus:
                enemy.health = 120;
                break;

            case EnemyType.Schwerathlet:
                enemy.health = 320;
                enemy.spareThreshold = 100;
                enemy.reward = new RewardInfo(35, 25, 400, 200);
                enemy.attackAilments = new AttackAilmentInfo(0.1f, Ailment.Discouraged, Ailment.Injured, Ailment.Bone_Fracture);
                break;

            case EnemyType.Sklaventreiber:
                enemy.health = 450;
                enemy.spareThreshold = 150;
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => { for (int i = 0; i < 3; i++) e.Player.AddAilment((Ailment)UnityEngine.Random.Range(0, 17)); })
                { Frequency = 2 };
                enemy.reward = new RewardInfo(35, 25, 600, 300);
                enemy.AddStatus(StatusEffect.Regeneration, 10);
                break;

            case EnemyType.Sonnenbringer:
                enemy.health = 400;
                enemy.spareThreshold = 200;
                enemy.reward = new RewardInfo(50, 20, 800, 400);
                break;

            case EnemyType.Krieger:
                enemy.health = 600;
                enemy.spareThreshold = 50;
                enemy.reward = new RewardInfo(75, 50, 1000, 500);
                enemy.attackAilments = new AttackAilmentInfo(0.1f, Ailment.Injured) { CanUpgrade = true };
                break;

            case EnemyType.Nemesis:
                enemy.health = 1000;
                enemy.attackAilments = new AttackAilmentInfo(1, Ailment.Flesh_Wound);
                break;

            case EnemyType.Huhn:
                enemy.health = 10;
                enemy.attackAilments = new AttackAilmentInfo(1, Ailment.Degeneration, Ailment.Disease);
                break;

            case EnemyType.Bestie:
                enemy.health = 500;
                enemy.Actions = 1;
                enemy.reward = new RewardInfo(0, 2500);
                enemy.attackAilments = new AttackAilmentInfo(1, Ailment.Degeneration, Ailment.Disease, Ailment.Toxin);
                break;

            case EnemyType.Bestienkämpfer:
                enemy.health = 250;
                enemy.reward = new RewardInfo((1, 1));
                enemy.attackAilments = new AttackAilmentInfo(1, Ailment.Flesh_Wound);
                break;
        }

        return enemy;
    }
}