using AutoClicker.Models.Clicks;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace AutoClicker.Services.Interfaces
{
    internal interface IMouseClicker
    {
        bool IsRunning { get; }

        event Action? ClickingStopped;

        Point GetCurrentCursorPosition();

        Point GetCursorPosition();

        Task StartClicking(Click click);

        void StopClicking();
    }
}
