using System;
using System.Collections.Generic;
using System.Windows.Input;
using AutoClicker.Models.Hotkeys;

namespace AutoClicker.Models.Parsing
{
    internal static class HotKeyParser
    {
        private static readonly IReadOnlyDictionary<string, ModifierKeys> ModifierTokens =
            new Dictionary<string, ModifierKeys>(StringComparer.OrdinalIgnoreCase)
            {
                ["Ctrl"] = ModifierKeys.Control,
                ["Control"] = ModifierKeys.Control,
                ["Shift"] = ModifierKeys.Shift,
                ["Alt"] = ModifierKeys.Alt,
                ["Win"] = ModifierKeys.Windows,
                ["Windows"] = ModifierKeys.Windows
            };

        private static readonly ModifierKeys[] ModifierOrder =
        {
            ModifierKeys.Control,
            ModifierKeys.Shift,
            ModifierKeys.Alt,
            ModifierKeys.Windows
        };

        public static bool TryNormalize(string? text, out string normalized)
        {
            normalized = string.Empty;

            if (!TryParse(text, out var definition))
            {
                return false;
            }

            normalized = FormatDisplay(definition.Modifiers, definition.Key);
            return true;
        }

        public static bool TryParse(string? text, out HotKeyDefinition definition)
        {
            definition = default!;

            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var tokens = text.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (tokens.Length == 0)
            {
                return false;
            }

            var modifiers = ModifierKeys.None;
            Key? key = null;

            foreach (var token in tokens)
            {
                if (ModifierTokens.TryGetValue(token, out var modifier))
                {
                    modifiers |= modifier;
                    continue;
                }

                if (key.HasValue)
                {
                    return false;
                }

                if (!Enum.TryParse(token, true, out Key parsedKey) || parsedKey == Key.None)
                {
                    return false;
                }

                key = parsedKey;
            }

            if (!key.HasValue)
            {
                return false;
            }

            definition = new HotKeyDefinition(modifiers, key.Value);
            return true;
        }

        public static string FormatDisplay(ModifierKeys modifiers, Key key)
        {
            if (key == Key.None)
            {
                return string.Empty;
            }

            var parts = new List<string>();

            foreach (var modifier in ModifierOrder)
            {
                if (modifiers.HasFlag(modifier))
                {
                    parts.Add(ModifierDisplayName(modifier));
                }
            }

            parts.Add(key.ToString());

            return string.Join("+", parts);
        }

        private static string ModifierDisplayName(ModifierKeys modifier)
        {
            return modifier switch
            {
                ModifierKeys.Control => "Control",
                ModifierKeys.Shift => "Shift",
                ModifierKeys.Alt => "Alt",
                ModifierKeys.Windows => "Windows",
                _ => modifier.ToString()
            };
        }
    }
}
