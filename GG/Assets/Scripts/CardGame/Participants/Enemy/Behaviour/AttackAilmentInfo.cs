using UnityEngine;

public class AttackAilmentInfo
{
    private Ailment[] ailments;
    private float chance;
    private bool canUpgrade = false;

    public AttackAilmentInfo(float chance, params Ailment[] ailments)
    {
        this.ailments = ailments;
        this.chance = chance;
    }

    public bool CanUpgrade { get => canUpgrade; set => canUpgrade = value; }

    public void OnAttack(Enemy enemy)
    {
        if (canUpgrade || enemy.Player.Block < 1 && Random.Range(0f, 1f) < chance)
            Apply(enemy.Player);
    }

    public void Apply(Player player)
    {
        if (ailments.Length < 1)
            return;

        if (canUpgrade && ailments.Length > 1)
        {
            if (Random.Range(0f, 1f) < chance)
                player.AddAilment(ailments[1]);
            else
                player.AddAilment(ailments[0]);

            return;
        }

        player.AddAilment(ailments[Random.Range(0, ailments.Length)]);
    }
}