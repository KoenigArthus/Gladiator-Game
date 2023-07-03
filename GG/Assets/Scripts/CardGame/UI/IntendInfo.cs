using System.Collections.Generic;
using UnityEngine;

public class IntendInfo
{
    #region Fields

    private Dictionary<EnemyIntensionType, Sprite> intendSprites = new Dictionary<EnemyIntensionType, Sprite>();

    private EnemyIntensionType icon;

    #endregion Fields

    #region Properties

    public Sprite Sprite
    { get { if (!intendSprites.ContainsKey(icon)) intendSprites.Add(icon, GetSprite(icon.ToString())); return intendSprites[icon]; } }

    #endregion Properties

    public static Sprite GetSprite(string name)
    {
        Sprite sprite = Resources.Load<Sprite>($"Textures/Icons/Intend/{name}");
        if (sprite != null)
            return sprite;

        return Resources.Load<Sprite>($"Textures/CardGame/Icons/Debug");
    }
}