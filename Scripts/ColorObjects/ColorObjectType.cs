using UnityEngine;

namespace ColorObjects
{
    public enum ColorObjectType
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Orange,
        White,
        Black,
        Brown,
        Pink,
        Gray,
        Cyan,
        Magenta,
    }

    public static class Utility
    {
        public static Color GetColor(ColorObjectType colorType)
        {
            switch (colorType)
            {
                case ColorObjectType.Red:
                    return Color.red;
                case ColorObjectType.Green:
                    return Color.green;
                case ColorObjectType.Blue:
                    return Color.blue;
                case ColorObjectType.Yellow:
                    return Color.yellow;
                case ColorObjectType.Purple:
                    return Color.magenta;
                case ColorObjectType.Orange:
                    return new Color(1, 0.5f, 0);
                case ColorObjectType.White:
                    return Color.white;
                case ColorObjectType.Black:
                    return Color.black;
                case ColorObjectType.Brown:
                    return new Color(0.5f, 0.25f, 0);
                case ColorObjectType.Pink:
                    return new Color(1, 0.5f, 0.5f);
                case ColorObjectType.Gray:
                    return Color.gray;
                case ColorObjectType.Cyan:
                    return Color.cyan;
                case ColorObjectType.Magenta:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }
    }
}