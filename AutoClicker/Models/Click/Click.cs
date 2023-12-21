using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClicker.Models.Click.States;

namespace AutoClicker.Models.Click
{
    internal class Click
    {
        private readonly Interval _interval;
        private readonly Options _options;
        private readonly Repeats _repeats;
        private readonly Position _position;

        public Click()
        {
            _interval = new Interval();
            _options = new Options();
            _repeats = new Repeats();
            _position = new Position();
        }

        public Interval Interval => _interval;

        public Options Options => _options;

        public Repeats Repeats => _repeats;

        public Position Position => _position;
    }
}
