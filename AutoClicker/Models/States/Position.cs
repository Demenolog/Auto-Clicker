using System.Drawing;
using AutoClicker.Models.Mouse;
using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;

namespace AutoClicker.Models.Clicks.States
{
    internal class Position
    {
        private readonly Point _position;
        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Position()
        {
            _position = GetPosition();
        }

        public Point CurrentPosition => _position;

        private Point GetPosition()
        {
            var isCurrentLocationSelected = MainWindow.IsCurrentLocationSelected;
            Point position;

            if (isCurrentLocationSelected)
            {
                position = MouseClicks.GetCurrentCursorPosition();
            }
            else
            {
                var x = int.Parse(MainWindow.XAxisTextBox);
                var y = int.Parse(MainWindow.YAxisTextBox);

                position = new Point(x, y);
            }

            return position;
        }
    }
}