using System;
using System.Drawing;
using System.Runtime.InteropServices;
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

        #region User32.dll methods

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(VirtualKeyStates nVirtKey);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        #endregion User32.dll methods

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
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LBUTTON) & KEY_PRESSED))
                {
                    GetCursorPos(out Point point);
                    return point;
                }
            }
        }

        public static async Task StartClicking(int intervalTime, string selectedBtn, int selectedBtnMode, int repeatMode, Point cursorPosition)
        {
            Cts ??= new CancellationTokenSource();
            var token = Cts.Token;

            WatchToStopClicking();

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
            mouse_event((int)action, x, y, dwData, dwExtraInfo);
        }

        private static void RunLeftClicking(Point cursorPosition, int clicksMode, int intervalTime, CancellationToken token)
        {
            int x = cursorPosition.X;
            int y = cursorPosition.Y;

            for (int i = 0; i < clicksMode; i++)
            {
                SetCursorPos(x, y);
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
                SetCursorPos(x, y);
                Click(MouseEventFlags.Rightdown);
                Click(MouseEventFlags.Rightup);
            }

            Thread.Sleep(intervalTime);

            token.ThrowIfCancellationRequested();
        }

        private static async void WatchToStopClicking()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_F4) & KEY_PRESSED))
                    {
                        Cts.Cancel();
                        break;
                    }
                }
            });
        }

        #endregion [Methods]
    }
}