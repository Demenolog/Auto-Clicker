using System;
using System.Windows.Forms;

namespace AutoClicker.Services.Interfaces
{
    internal interface ITrayIconService : IDisposable
    {
        void Initialize();
        void UpdateStatus();
        void ShowBalloon(string title, string message, ToolTipIcon icon, int timeoutMilliseconds);
    }
}
