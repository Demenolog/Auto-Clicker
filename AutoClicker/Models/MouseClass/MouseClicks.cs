using AutoClicker.Models.MouseClass.DllImport;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.MouseClass
{
    internal static class MouseClicks
    {
        #region [Properties]

        private static string s_buttonToStop = "F5";

        private static CancellationTokenSource s_cts;

        public static CancellationTokenSource Cts
        {
            get => s_cts;
            private set => s_cts = value;
        }

        #endregion [Properties]

        #region [Methods]

        public static int GetClickMode(string clickMode)
        {
            return (int)Enum.Parse(typeof(ClickModes), clickMode);
        }

        public static Point GetCurrentCursorPosition()
        {
            User32.GetCursorPos(out Point result);
            return result;
        }

        public static Point GetCursorPosition()
        {
            while (true)
            {
                if (Convert.ToBoolean(User32.GetKeyState(VirtualKeyStates.VK_LBUTTON) & KEY_PRESSED))
                {
                    User32.GetCursorPos(out Point point);
                    return point;
                }
            }
        }

        public static async Task StartClicking(int intervalTime, string selectedBtn, int selectedBtnMode, int repeatMode, Point cursorPosition)
        {
            Cts ??= new CancellationTokenSource();
            var token = Cts.Token;

            Watcher.WatchToStopClicking();

            try
            {
                await Task.Run(() =>
                {
                    if (selectedBtn == "Left")
                    {
                        if (repeatMode >= 0)
                        {
                            for (int i = 0; i < repeatMode; i++)
                            {
                                RunLeftClicking(cursorPosition, selectedBtnMode, intervalTime, token);
                            }
                        }
                        else
                        {
                            while (true)
                            {
                                RunLeftClicking(cursorPosition, selectedBtnMode, intervalTime, token);
                            }
                        }
                    }
                    else if (selectedBtn == "Right")
                    {
                        if (repeatMode >= 0)
                        {
                            for (int i = 0; i < repeatMode; i++)
                            {
                                RunRightClicking(cursorPosition, selectedBtnMode, intervalTime, token);
                            }
                        }
                        else
                        {
                            while (true)
                            {
                                RunRightClicking(cursorPosition, selectedBtnMode, intervalTime, token);
                            }
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
            Cts.Cancel();
        }

        private static void Click(MouseEventFlags action, int x = 0, int y = 0, int dwData = 0, int dwExtraInfo = 0)
        {
            User32.mouse_event((int)action, x, y, dwData, dwExtraInfo);
        }

        private static void RunLeftClicking(Point cursorPosition, int clicksMode, int intervalTime, CancellationToken token)
        {
            int x = cursorPosition.X;
            int y = cursorPosition.Y;

            for (int i = 0; i < clicksMode; i++)
            {
                User32.SetCursorPos(x, y);
                Click(MouseEventFlags.Leftdown);
                Click(MouseEventFlags.Leftup);
            }

            Thread.Sleep(intervalTime);

            token.ThrowIfCancellationRequested();
        }

        private static void RunRightClicking(Point cursorPosition, int clicksMode, int intervalTime, CancellationToken token)
        {
            int x = cursorPosition.X;
            int y = cursorPosition.Y;

            for (int i = 0; i < clicksMode; i++)
            {
                User32.SetCursorPos(x, y);
                Click(MouseEventFlags.Rightdown);
                Click(MouseEventFlags.Rightup);
            }

            Thread.Sleep(intervalTime);

            token.ThrowIfCancellationRequested();
        }

        #endregion [Methods]
    }
}