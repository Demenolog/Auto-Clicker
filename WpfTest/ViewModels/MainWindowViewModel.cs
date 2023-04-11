using System.Drawing;
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

        public ICommand TestAsync { get; }

        private bool CanTestExecutedAsync(object p) => true;

        private async Task<Point> OnTestExecuteAsync(object p)
        {
            var task = new Task<Point>((() => MouseClicks.GetCursorPosition()));
            
            task.Start(); 
            
            var point = await task;
            
            TextBoxOne = point.X.ToString();
            TextBoxTwo = point.Y.ToString();

            return new Point();
        }

        public MainWindowViewModel()
        {
            TestAsync = new LambdaCommandAsync(OnTestExecuteAsync, CanTestExecutedAsync);
        }
    }
}