using System.Windows.Input;

namespace AutoClicker.Models.Hotkeys
{
    internal sealed record HotKeyDefinition(ModifierKeys Modifiers, Key Key)
    {
        public string ToDisplayString()
        {
            if (Modifiers == ModifierKeys.None)
            {
                return Key.ToString();
            }

            var modifiersText = Modifiers.ToString().Replace(", ", "+");
            return $"{modifiersText}+{Key}";
        }
    }
}
