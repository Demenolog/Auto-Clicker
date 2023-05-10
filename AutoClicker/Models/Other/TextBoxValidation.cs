using System;

namespace AutoClicker.Models.Other
{
    internal static class TextBoxValidation
    {
        public static bool IsPositiveIntNumber(string text)
        {
            return !string.IsNullOrEmpty(text) && (int.TryParse(text, out _) && int.Parse(text) >= 0);
        }

        public static bool IsIntNumber(string text)
        {
            return !string.IsNullOrEmpty(text) && int.TryParse(text, out _);
        }
    }
}