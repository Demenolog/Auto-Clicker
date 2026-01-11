using System;

namespace AutoClicker.Models.Other
{
    internal static class TextBoxValidation
    {
        public static bool IsPositiveIntNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            return int.TryParse(text, out var value) && value >= 0;
        }

        public static bool IsIntNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            return int.TryParse(text, out _);
        }
    }
}
