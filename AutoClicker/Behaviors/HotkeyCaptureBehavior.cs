using AutoClicker.Models.Hotkeys;
using AutoClicker.ViewModels;
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
            var key = e.Key == Key.System ? e.SystemKey : e.Key;
            if (IsModifierKey(key))
            {
                e.Handled = true;       // prevent default text input
                return;
            }

            var modifiers = Keyboard.Modifiers;
            var hotKeyBinding = new HotKeyDefinition(modifiers, key);

            var tb = (TextBox)sender;
            var binding = tb.GetBindingExpression(TextBox.TextProperty);
            var bindingPath = binding?.ParentBinding?.Path?.Path;

            if (tb.DataContext is HotKeyWindowViewModel viewModel)
            {
                if (bindingPath == nameof(HotKeyWindowViewModel.StartHotKeyDisplay))
                {
                    viewModel.SetStartHotKey(hotKeyBinding);
                }
                else if (bindingPath == nameof(HotKeyWindowViewModel.StopHotKeyDisplay))
                {
                    viewModel.SetStopHotKey(hotKeyBinding);
                }
            }

            tb.Text = hotKeyBinding.ToDisplayString();
            tb.CaretIndex = tb.Text.Length;

            e.Handled = true;       // prevent default text input
        }

        private static bool IsModifierKey(Key key)
        {
            return key == Key.LeftShift
                || key == Key.RightShift
                || key == Key.LeftCtrl
                || key == Key.RightCtrl
                || key == Key.LeftAlt
                || key == Key.RightAlt
                || key == Key.LWin
                || key == Key.RWin;
        }
    }
}
