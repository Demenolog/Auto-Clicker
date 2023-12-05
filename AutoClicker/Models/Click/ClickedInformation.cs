using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClicker.Models.Click.States;

namespace AutoClicker.Models.Click
{
    internal class ClickedInformation
    {
        private Intervals _inrerval;
        private Options _option;
        private Repeats _repeat;
        private Positions _position;

        public ClickedInformation()
        {
            _inrerval = new Intervals();
        }
    }
}
