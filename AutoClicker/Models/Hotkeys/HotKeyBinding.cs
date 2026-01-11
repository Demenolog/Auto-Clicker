using System.Windows.Input;

namespace AutoClicker.Models.Hotkeys
{
    internal sealed class HotKeyBinding
    {
        public HotKeyBinding(ModifierKeys modifiers, Key key)
        {
            Modifiers = modifiers;
            Key = key;
        }

        public ModifierKeys Modifiers { get; }

        public Key Key { get; }

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
