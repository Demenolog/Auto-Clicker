using System.Drawing;
using AutoClicker.Models.Clicks;

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
            var x = int.Parse(config.XAxis);
            var y = int.Parse(config.YAxis);

            return new Point(x, y);
        }
    }
}
