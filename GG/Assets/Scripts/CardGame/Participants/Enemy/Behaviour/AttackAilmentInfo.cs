using UnityEngine;

public class AttackAilmentInfo
{
    private Ailment[] ailments;
    private float chance;

    public AttackAilmentInfo(float chance, params Ailment[] ailments)
    {
        this.ailments = ailments;
        this.chance = chance;
    }

    public void OnAttack(Enemy enemy)
    {
        if (enemy.Player.Block < 1 && Random.Range(0f, 1f) < chance)
            Apply(enemy.Player);
    }

    public void Apply(Player player)
    {
        if (ailments.Length < 1)
            return;

        player.AddAilment(ailments[Random.Range(0, ailments.Length)]);
    }
}