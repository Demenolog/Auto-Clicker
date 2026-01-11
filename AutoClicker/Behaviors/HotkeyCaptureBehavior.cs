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
            var modifiers = Keyboard.Modifiers;
            var hotKeyBinding = new HotKeyBinding(modifiers, key);

            var tb = (TextBox)sender;
            var binding = tb.GetBindingExpression(TextBox.TextProperty);
            var bindingPath = binding?.ParentBinding?.Path?.Path;

            if (tb.DataContext is HotKeyWindowViewModel viewModel)
            {
                if (bindingPath == nameof(HotKeyWindowViewModel.StartHotKey))
                {
                    viewModel.SetStartHotKey(hotKeyBinding);
                }
                else if (bindingPath == nameof(HotKeyWindowViewModel.StopHotKey))
                {
                    viewModel.SetStopHotKey(hotKeyBinding);
                }
            }

            tb.Text = hotKeyBinding.ToDisplayString();
            tb.CaretIndex = tb.Text.Length;

            e.Handled = true;       // prevent default text input
        }
    }
}
