using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AutoClicker.Models.Clicks;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;
using static AutoClicker.Infrastructure.UnsafeCode.User32;

namespace AutoClicker.Models.Mouse
{
    internal static class MouseClicks
    {
        #region [Properties]

        public static CancellationTokenSource? Cts { get; private set; }

        #endregion [Properties]

        #region [Methods]

        public static int GetClickMode(string clickMode)
        {
            return (int)Enum.Parse(typeof(ClickModes), clickMode);
        }

        public static Point GetCurrentCursorPosition()
        {
            GetCursorPos(out Point result);
            return result;
        }

        public static Point GetCursorPosition()
        {
            while (true)
            {
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LBUTTON) & KeyPressed))
                {
                    GetCursorPos(out Point point);
                    return point;
                }
                else if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_ESCAPE) & KeyPressed))
                {
                    return new Point(0, 0);
                }
            }
        }

        public static async Task StartClicking(Click click)
        {
            Cts ??= new CancellationTokenSource();
            var token = Cts.Token;
            var repeats = click.Repeats.TotalTimes;
            Action clickMethod;

            switch (click.Options.Button)
            {
                case "Left":
                    clickMethod = () => ExecuteClicking(click, MouseEventFlags.Leftdown, MouseEventFlags.Leftup, token);
                    break;
                case "Right":
                    clickMethod = () => ExecuteClicking(click, MouseEventFlags.Rightdown, MouseEventFlags.Rightup, token);
                    break;
                default:
                    throw new ArgumentException();
            }

            try
            {
                await Task.Run(() =>
                {
                    if (repeats >= 0)
                    {
                        for (int i = 0; i < repeats; i++)
                        {
                            clickMethod();
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            clickMethod();
                        }
                    }
                }, token);
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }
            finally
            {
                Cts = null;
            }
        }

        public static void StopClicking()
        {
            Cts?.Cancel();
            Cts = null;
        }

        private static void ExecuteClicking(Click click, MouseEventFlags downFlag, MouseEventFlags upFlag, CancellationToken token)
        {
            var clicks = click.Options.GetButtonMode();
            var sleepInterval = click.Interval.TotalTime;
            var x = click.Position.CurrentPosition.X;
            var y = click.Position.CurrentPosition.Y;

            for (int i = 0; i < clicks; i++)
            {
                SetCursorPos(x, y);
                Click(downFlag);
                Click(upFlag);
            }

            Thread.Sleep(sleepInterval);

            token.ThrowIfCancellationRequested();
        }

        private static void Click(MouseEventFlags action, int x = 0, int y = 0, int dwData = 0, int dwExtraInfo = 0)
        {
            mouse_event((int)action, x, y, dwData, dwExtraInfo);
        }

        #endregion [Methods]
    }
}