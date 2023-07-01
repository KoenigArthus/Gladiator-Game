using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Yarn.Unity;

public partial class Enemy
{
    #region Fields

    private int health = 10;
    private int spareThreshold = int.MinValue;
    private SkillInfo[] skills = new SkillInfo[0];
    private AbilityInfo ability = null;
    private EnrageInfo enrage = null;
    private AttackAilmentInfo attackAilments = null;
    private RewardInfo reward;

    #endregion Fields

    #region Properties

    public int Actions
    {
        get => intension.Length;
        set { SkillInfo[] intension = this.intension; this.intension = new SkillInfo[value]; intension.Take(value).ToArray().CopyTo(this.intension, 0); }
    }

    public RewardInfo Reward => reward;

    #endregion Properties

    /// <summary>
    /// Build shared resoureces
    /// </summary>
    public static Enemy BuildEnemy(Enemy enemy, EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Tutorialgladiator:
                enemy.health = 26;
                enemy.Actions = 2;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, 2, (3, 6)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, 1, (2, 8))
                };
                enemy.AddStatus(StatusEffect.Regeneration, 2);
                enemy.reward = new RewardInfo(3);
                enemy.attackAilments = new AttackAilmentInfo(0.1f, Ailment.Bone_Fracture);
                break;

            case EnemyType.Murmillo:
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, 2, (6, 10)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, 1, (8, 12))
                };

                enemy.AddStatus(StatusEffect.Regeneration, 4);
                break;

            case EnemyType.Retiarius:
                enemy.Actions = 4;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, (3, 6)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (5, 8), (SkillInfo s, Enemy e) => e.Attack(e.Player, s.GetPower(), false, true))

                    //Solange er keinen Block hat, 40% Chance Angriffen auszuweichen.
                };

                enemy.AddStatus(StatusEffect.Regeneration, 4);
                break;

            case EnemyType.Hoplomachus:
                enemy.Actions = 2;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, (10, 20)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (14, 28))
                };
                enemy.forcedIntension = new EnemyIntension?[] { EnemyIntension.Block, EnemyIntension.Attack };
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => { e.AddStatus(StatusEffect.Strength, 2); e.AddStatus(StatusEffect.Defence, 2); })
                { Frequency = 3 };
                enemy.enrage = new EnrageInfo((EnrageInfo l, Enemy e) => enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, (10, 20), (SkillInfo s, Enemy e) => {e.AddBlock(s.GetPower()); e.AddStatus(StatusEffect.Regeneration, 1); }),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (14, 28), (SkillInfo s, Enemy e) => {e.Attack(e.Player, s.GetPower() ); e.Player.AddStatus(StatusEffect.Stun, 5); })
                }, 30);
                enemy.AddStatus(StatusEffect.Regeneration, 7);
                break;

            case EnemyType.Scissor:
                enemy.Actions = 5;
                enemy.skills = new SkillInfo[]
                {
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (3, 5), (SkillInfo s, Enemy e) => {e.Attack(e.Player, s.GetPower()); e.Player.AddStatus(StatusEffect.Bleeding, 1); })
                };
                enemy.Player.AddActionEffect((CardInfo c, int v) => { if (c.Type == CardType.Block) { c.DestroyOnDiscard = true; return c; } return null; }, null, true);
                enemy.AddStatus(StatusEffect.Regeneration, 3);
                break;

            case EnemyType.Dimachaerus:
                enemy.Actions = 2;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, (15, 25)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (10, 12)),
                    //Special
                    new SkillInfo(EnemyIntension.Special, 15, (SkillInfo s, Enemy e) => e.Player.AddStatus(StatusEffect.Vulnerable, s.GetPower()))
                };
                enemy.forcedIntension = new EnemyIntension?[] { EnemyIntension.Special, null, EnemyIntension.Attack, EnemyIntension.Attack };
                enemy.enrage = new EnrageInfo((EnrageInfo l, Enemy e) => e.Actions = 4, 60);
                enemy.AddStatus(StatusEffect.Regeneration, 8);
                break;

            case EnemyType.Schwerathlet:
                enemy.skills = new SkillInfo[]
                {
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (2, 3), (SkillInfo s, Enemy e) => {e.Attack(e.Player, s.GetPower(), false, true); e.Player.AddStatus(StatusEffect.Bleeding, 1); }),
                    //Special
                    new SkillInfo(EnemyIntension.Special, 30, (SkillInfo s, Enemy e) => e.Player.AddStatus(StatusEffect.Feeble, 30))
                };
                enemy.forcedIntension = new EnemyIntension?[] { null, null, null, null, null, null, null, EnemyIntension.Special };
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => e.AddStatus(StatusEffect.Strength, 1));
                enemy.AddStatus(StatusEffect.Regeneration, 22);
                break;

            case EnemyType.Sklaventreiber:
                enemy.Actions = 3;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, (30, 50)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (14, 20), (SkillInfo s, Enemy e) => {e.Attack(e.Player, s.GetPower()); e.skills[2].Execute(e); }),
                    //Special
                    new SkillInfo(EnemyIntension.Special, (3, 6), (SkillInfo s, Enemy e) => { e.Player.AddStatus(StatusEffect.Vulnerable, s.GetPower()); e.Player.AddStatus(StatusEffect.Weak, s.GetPower()); e.Player.AddStatus(StatusEffect.Stun, s.GetPower()); })
                };
                enemy.forcedIntension = new EnemyIntension?[] { EnemyIntension.Block };
                enemy.enrage = new EnrageInfo((EnrageInfo l, Enemy e) => e.skills[1] =
                new SkillInfo(EnemyIntension.Attack, (5, 10), (SkillInfo s, Enemy e) => { e.Attack(e.Player, s.GetPower(), true); e.skills[2].Execute(e); }), 200);
                enemy.AddStatus(StatusEffect.Regeneration, 20);
                break;

            case EnemyType.Sonnenbringer:
                enemy.Actions = 1;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, 80, (SkillInfo s, Enemy e) => { e.AddBlock(s.GetPower()); e.Heal(20); })
                };
                enemy.ability = new AbilityInfo((AbilityInfo a, Enemy e) => { e.Player.InstantDamage(100, false); e.InstantDamage(100, false); })
                { Frequency = 3 };
                break;

            case EnemyType.Krieger:

                enemy.Actions = 3;
                enemy.skills = new SkillInfo[]
                {
                    //Defensive
                    new SkillInfo(EnemyIntension.Block, (30, 50)),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, (40, 60))
                };
                break;

            case EnemyType.Nemesis:
                enemy.Actions = 3;
                Ailment[] ailments = new Ailment[] { Ailment.Cardiac_Insufficiency, Ailment.Panic_Attack, Ailment.Discouraged, Ailment.Hopeless, Ailment.Trauma, Ailment.Terror };
                enemy.skills = new SkillInfo[]
                {
                    //Skills
                    new SkillInfo(EnemyIntension.Skill, 4, (SkillInfo s, Enemy e) => e.AddStatus(StatusEffect.Strength, s.GetPower())),
                    new SkillInfo(EnemyIntension.Skill, 0, (SkillInfo s, Enemy e) => e.Player.AddAilment(ailments[Random.Range(0, ailments.Length)], true)),
                    new SkillInfo(EnemyIntension.Skill, 25, (SkillInfo s, Enemy e) => { e.Player.AddStatus(StatusEffect.Stun, s.GetPower()); e.Player.AddStatus(StatusEffect.Weak, s.GetPower()); }),
                    //Agressive
                    new SkillInfo(EnemyIntension.Attack, 30)
                };
                enemy.forcedIntension = new EnemyIntension?[] { EnemyIntension.Skill, EnemyIntension.Skill, EnemyIntension.Skill };
                enemy.enrage = new EnrageInfo((EnrageInfo l, Enemy e) => e.forcedIntension = new EnemyIntension?[0], 500);
                enemy.AddStatus(StatusEffect.Regeneration, 25);
                break;
        }

        return enemy;
    }
}