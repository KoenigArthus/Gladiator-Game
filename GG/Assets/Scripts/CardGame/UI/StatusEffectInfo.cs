using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInfo
{
    #region Fields

    private Dictionary<StatusEffect, Sprite> effectSprites = new Dictionary<StatusEffect, Sprite>();

    private StatusEffect effect;
    private int value;

    #endregion Fields

    public StatusEffectInfo(StatusEffect effect, Participant participant)
    {
        this.effect = effect;
        value = participant.GetStatus(effect);
    }

    #region Properties

    public StatusEffect Effect => effect;
    public int Value => value;

    public Sprite Sprite
    { get { if (!effectSprites.ContainsKey(effect)) effectSprites.Add(effect, GetSprite(effect.ToString())); return effectSprites[effect]; } }

    #endregion Properties

    public static Sprite GetSprite(string name)
    {
        Sprite sprite = Resources.Load<Sprite>($"Textures/CardGame/Equipment{name}");
        if (sprite != null)
            return sprite;

        return Resources.Load<Sprite>($"Textures/CardGame/Equipment/Debug");
    }
}