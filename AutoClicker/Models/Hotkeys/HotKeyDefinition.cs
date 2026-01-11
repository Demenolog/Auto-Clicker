using System.Windows.Input;
using AutoClicker.Models.Parsing;

namespace AutoClicker.Models.Hotkeys
{
    internal sealed record HotKeyDefinition(ModifierKeys Modifiers, Key Key)
    {
        public string ToDisplayString()
        {
            return HotKeyParser.FormatDisplay(Modifiers, Key);
        }
    }
}
