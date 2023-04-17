using AutoClicker.Models.MouseClass;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfTest.Infrastructure.Commands;
using WpfTest.ViewModels.Base;

namespace WpfTest.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region TextBoxOne : string - test

        private string _TextBoxOne = "0";

        public string TextBoxOne
        {
            get => _TextBoxOne;
            set => SetField(ref _TextBoxOne, value);
        }

        #endregion TextBoxOne : string - test

        #region ButtonVisible : bool - test

        private bool _isButtonEnable = true;

        public bool IsButtonEnable
        {
            get => _isButtonEnable;
            set => SetField(ref _isButtonEnable, value);
        }

        #endregion ButtonVisible : bool - test

        #region TextBoxTwo : string - test

        private string _TextBoxTwo = "0";

        public string TextBoxTwo
        {
            get => _TextBoxTwo;
            set => SetField(ref _TextBoxTwo, value);
        }

        #endregion TextBoxTwo : string - test

        #region Title : string - string

        private string _Title = "Not empty";

        public string Title
        {
            get => _Title;
            set => SetField(ref _Title, value);
        }

        #endregion Title : string - string

        #region Async Command

        public ICommand TestAsync { get; }

        private bool CanTestExecutedAsync(object p) => true;

        private async Task OnTestExecuteAsync(object p)
        {
            await Task.Run((() =>
            {
                var point = MouseClicks.GetCursorPosition();

                TextBoxOne = point.X.ToString();
                TextBoxTwo = point.Y.ToString();
            }));

            IsButtonEnable = true;
        }

        #endregion Async Command

        public ICommand Test { get; }

        private bool CanTestExecuted(object p) => true;

        private async void OnTestExecute(object p)
        {
            IsButtonEnable = false;

            await Task.Run((() =>
            {
                var point = MouseClicks.GetCursorPosition();

                TextBoxOne = point.X.ToString();
                TextBoxTwo = point.Y.ToString();
            }));

            IsButtonEnable = true;
        }

        public MainWindowViewModel()
        {
            TestAsync = new LambdaCommandAsync(OnTestExecuteAsync, CanTestExecutedAsync);

            Test = new LambdaCommand(OnTestExecute, CanTestExecuted);
        }
    }
}