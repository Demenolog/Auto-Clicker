using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoClicker.Models.MouseClass;
using WpfTest.Infrastructure.Commands;
using WpfTest.ViewModels.Base;

namespace WpfTest.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        #region Title : string - string

        private string _Title;

        public string Title
        {
            get => _Title;
            set => SetField(ref _Title, value);
        }

        #endregion

        public ICommand TestAsync { get; }

        private bool CanTestExecutedAsync(object p) => true;

        private Task OnTestExecuteAsync(object p)
        {
            var point = new Point();

            var task = new Task(() => { MouseClicks.GetCursorPosition(point); });

            task.Wait();

            Title = point.ToString();

            return task;
        }

        public MainWindowViewModel()
        {
            TestAsync = new LambdaCommandAsync(OnTestExecuteAsync, CanTestExecutedAsync);
        }
    }
}