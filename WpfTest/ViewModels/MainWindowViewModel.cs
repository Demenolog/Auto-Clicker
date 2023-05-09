using System.Windows;
using System.Windows.Input;
using WpfTest.Infrastructure.Commands;
using WpfTest.Services;
using WpfTest.ViewModels.Base;

namespace WpfTest.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Title : string - string

        private string _title = "Not empty";

        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        #endregion Title : string - string

        #region ButtonVisible : bool - test

        private bool _isButtonEnable = true;

        public bool IsButtonEnable
        {
            get => _isButtonEnable;
            set => SetField(ref _isButtonEnable, value);
        }

        #endregion ButtonVisible : bool - test

        #region TextBoxOne : string - test

        private string _textBoxOne = "F1";

        public string TextBoxOne
        {
            get => _textBoxOne;
            set => SetField(ref _textBoxOne, value);
        }

        #endregion TextBoxOne : string - test

        #region TextBoxTwo : string - test

        private string _textBoxTwo = "F2";

        public string TextBoxTwo
        {
            get => _textBoxTwo;
            set => SetField(ref _textBoxTwo, value);
        }

        #endregion TextBoxTwo : string - test

        #region Test command

        public ICommand Test { get; }

        private bool CanTestExecuted(object p) => true;

        internal void OnTestExecute(object p)
        {
            MessageBox.Show("kEK");
        }

        #endregion Test command

        public ICommand OpenChangeHotKeysWindow { get; }

        private bool CanOpenChangeHotKeysWindowExecuted(object p) => true;

        private void OnOpenChangeHotKeysWindowExecute(object p)
        {
            HotKeysWindowService.Create();

            HotKeysWindowService.Show();
        }

        public MainWindowViewModel()
        {
            Test = new LambdaCommand(OnTestExecute, CanTestExecuted);

            OpenChangeHotKeysWindow = new LambdaCommand(OnOpenChangeHotKeysWindowExecute, CanOpenChangeHotKeysWindowExecuted);
        }
    }
}