using System;

namespace AutoClicker.Models.Other
{
    internal static class TextBoxValidation
    {
        public static bool IsPositiveIntNumber(string text)
        {
            return (int.TryParse(text, out _) && int.Parse(text) > 0) || text.Length == 0;
        }
    }
}