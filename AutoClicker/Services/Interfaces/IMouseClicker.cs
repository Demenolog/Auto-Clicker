using AutoClicker.Models.Clicks;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace AutoClicker.Services.Interfaces
{
    internal interface IMouseClicker
    {
        bool IsRunning { get; }

        bool IsPaused { get; }

        event Action? ClickingStopped;

        Point GetCurrentCursorPosition();

        bool TryGetCursorPosition(out Point position);

        Task StartClicking(Click click);

        void StopClicking();

        void PauseClicking();

        void ResumeClicking();
    }
}
