using System.Drawing;
using AutoClicker.Models.Clicks;
using AutoClicker.Models.Mouse;

namespace AutoClicker.Models.Clicks.States
{
    internal class Position
    {
        private readonly Point _position;

        public Position(ClickPositionConfig config)
        {
            _position = GetPosition(config);
        }

        public Point CurrentPosition => _position;

        private Point GetPosition(ClickPositionConfig config)
        {
            var isCurrentLocationSelected = config.IsCurrentLocationSelected;
            Point position;

            if (isCurrentLocationSelected)
            {
                position = MouseClicks.GetCurrentCursorPosition();
            }
            else
            {
                var x = int.Parse(config.XAxis);
                var y = int.Parse(config.YAxis);

                position = new Point(x, y);
            }

            return position;
        }
    }
}
