using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;
using static AutoClicker.Models.MouseClass.UnsafeCode.User32;

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

        public static async Task StartClicking(int intervalTime, string selectedBtn, int selectedBtnMode, int repeatMode, Point cursorPosition)
        {
            Cts ??= new CancellationTokenSource();
            var token = Cts.Token;

            Action clickMethod = selectedBtn == "Left" ? 
                () => RunClicking(cursorPosition, selectedBtnMode, intervalTime, MouseEventFlags.Leftdown, MouseEventFlags.Leftup, token) : 
                () => RunClicking(cursorPosition, selectedBtnMode, intervalTime, MouseEventFlags.Rightdown, MouseEventFlags.Rightup, token);

            try
            {
                await Task.Run(() =>
                {
                    if (repeatMode >= 0)
                    {
                        for (int i = 0; i < repeatMode; i++)
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

        private static void Click(MouseEventFlags action, int x = 0, int y = 0, int dwData = 0, int dwExtraInfo = 0)
        {
            mouse_event((int)action, x, y, dwData, dwExtraInfo);
        }

        private static void RunClicking(Point cursorPosition, int clicksMode, int intervalTime, MouseEventFlags downFlag, MouseEventFlags upFlag, CancellationToken token)
        {
            for (int i = 0; i < clicksMode; i++)
            {
                SetCursorPos(cursorPosition.X, cursorPosition.Y);
                Click(downFlag);
                Click(upFlag);
            }

            Thread.Sleep(intervalTime);

            token.ThrowIfCancellationRequested();
        }
        
        #endregion [Methods]
    }
}