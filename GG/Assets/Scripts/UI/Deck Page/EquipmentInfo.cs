using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Deck_Page
{
    public class EquipmentInfo
    {
        #region Fields

        private const int MAX_EQUIPMENT_LEVEL = 3;

        private Dictionary<string, Sprite> equipmentSprites = new Dictionary<string, Sprite>();

        private CardSet equipmentType = CardSet.None;

        #endregion Fields

        public EquipmentInfo(CardSet equipmentType)
        {
            this.equipmentType = equipmentType;
        }

        #region Properties

        public string Name => equipmentType.ToString();

        public Sprite Sprite
        { get { if (!equipmentSprites.ContainsKey(Name)) equipmentSprites.Add(Name, GetSprite(Name)); return equipmentSprites[Name]; } }

        public CardInfo[] Cards => CardLibrary.Cards.Where(x => x.Set == equipmentType).ToArray();
        public float EXP { get => UserFile.SaveGame.EquipmentEXP[(int)equipmentType]; set => UserFile.SaveGame.EquipmentEXP[(int)equipmentType] = value; }
        public float Level => MathF.Min(EXP / 100, MAX_EQUIPMENT_LEVEL);

        #endregion Properties

        public static Sprite GetSprite(string name)
        {
            Sprite sprite = Resources.Load<Sprite>($"Textures/CardGame/Equipment{name}");
            if (sprite != null)
                return sprite;

            return Resources.Load<Sprite>($"Textures/CardGame/Equipment/Debug");
        }

        public override bool Equals(object obj)
        {
            if (obj is EquipmentInfo other)
                return this.equipmentType == other.equipmentType;

            return false;
        }

        public override int GetHashCode()
        {
            return equipmentType.GetHashCode();
        }
    }
}