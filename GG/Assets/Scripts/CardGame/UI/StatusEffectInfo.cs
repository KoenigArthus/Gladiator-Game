using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInfo
{
    #region Fields

    private Dictionary<string, Sprite> effectSprites = new Dictionary<string, Sprite>();

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
    public string Name => CardLibrary.GetTranslatedName(effect.ToString());
    public string Description => CardLibrary.GetTranslatedDescription(effect.ToString());

    public Sprite Sprite
    { get { string effect = GetSpriteName(); if (!effectSprites.ContainsKey(effect)) effectSprites.Add(effect, GetSprite(effect)); return effectSprites[effect]; } }

    #endregion Properties

    public string GetSpriteName()
    {
        string name = effect.ToString();

        if (name.EndsWith("Strength"))
        {
            if (value < 0)
                return "Strength-";
            else
                return "Strength";
        }

        if (name.EndsWith("Defence"))
        {
            if (value < 0)
                return "Defence-";
            else
                return "Defence";
        }

        return name;
    }

    public static Sprite GetSprite(string name)
    {
        Sprite sprite = Resources.Load<Sprite>($"Textures/Icons/StatusEffect/{name}");
        if (sprite != null)
            return sprite;

        return Resources.Load<Sprite>($"Textures/CardGame/Icons/Debug");
    }
}