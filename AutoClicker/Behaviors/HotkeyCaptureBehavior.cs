using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoClicker.Behaviors
{
    public class HotkeyCaptureBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            base.OnDetaching();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Build a name like "Ctrl" or "Alt+F5"
            string keyName = e.Key == Key.System ? e.SystemKey.ToString() : e.Key.ToString();

            if (Keyboard.Modifiers != ModifierKeys.None)
                keyName = Keyboard.Modifiers + "+" + keyName;

            var tb = (TextBox)sender;
            tb.Text = keyName;
            tb.CaretIndex = tb.Text.Length;

            // Propagate to ViewModel
            var binding = tb.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();

            e.Handled = true;       // prevent default text input
        }
    }
}