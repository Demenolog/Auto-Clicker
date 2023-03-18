using System;

namespace AutoClicker.Models.Other
{
    internal static class TextBoxValidation
    {
        public static bool IsNumber(string text)
        {
            return Int32.TryParse(text, out _) || text.Length == 0;
        }
    }
}