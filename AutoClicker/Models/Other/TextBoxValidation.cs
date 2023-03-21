using System;

namespace AutoClicker.Models.Other
{
    internal static class TextBoxValidation
    {
        public static bool IsIntegerNumber(string text)
        {
            return int.TryParse(text, out _) || text.Length == 0;
        }
    }
}