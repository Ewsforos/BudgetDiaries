using Microsoft.Toolkit.Uwp;
using System;
using Windows.UI;

namespace AVL
{
    public static class ColorHelper
    {
        // <summary>
        // Использует цветовой круг для нахождения противоположного цвева
        // </summary>
        // <param name = "color" > Цвет для которого определяется противоположенный</param>
        // <param name = "alpha" > Прозрачность возвращаемого увета</param>
        // <returns>Противоположенный цвет на уветовом круге</returns>
        public static Color GetOpposideColor(this Color color, double alpha = 1)
        {
            var hsl = color.ToHsl();
            var hue = Math.Abs(180 - hsl.H);
            var newColor = Microsoft.Toolkit.Uwp.ColorHelper.FromHsl(hue, hsl.S, hsl.L, alpha);
            return newColor;
        }
    }
}
