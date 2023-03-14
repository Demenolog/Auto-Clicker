using AutoClicker.ViewModels.Base;

namespace AutoClicker.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Title : string - title property

        private string? _title = "Test name";

        public string? Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        #endregion Title : string - title property
    }
}