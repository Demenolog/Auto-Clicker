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

<<<<<<< HEAD
=======
        private static readonly object LockObject = new(); // Lock for thread safety
        private static bool s_isRunning; // Tracks if a task is running or stopping
>>>>>>> master
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
<<<<<<< HEAD
            Cts ??= new CancellationTokenSource();
=======
            lock (LockObject)
            {
                if (s_isRunning) return; // Prevent multiple tasks
                s_isRunning = true;
                Cts = new CancellationTokenSource();
            }

>>>>>>> master
            var token = Cts.Token;

            Action clickMethod = selectedBtn == "Left" ? 
                () => ExecuteClicking(cursorPosition, selectedBtnMode, intervalTime, MouseEventFlags.Leftdown, MouseEventFlags.Leftup, token) : 
                () => ExecuteClicking(cursorPosition, selectedBtnMode, intervalTime, MouseEventFlags.Rightdown, MouseEventFlags.Rightup, token);

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
<<<<<<< HEAD
                Cts = null;
=======
                lock (LockObject)
                {
                    Cts = null;
                    s_isRunning = false; // Allow new tasks to start
                }
>>>>>>> master
            }
        }

        public static void StopClicking()
        {
<<<<<<< HEAD
            Cts?.Cancel();
            Cts = null;
=======
            lock (LockObject)
            {
                if (!s_isRunning) return; // No task is running
                Cts?.Cancel();
                s_isRunning = false; // Mark as not running
            }
>>>>>>> master
        }

        private static void Click(MouseEventFlags action, int x = 0, int y = 0, int dwData = 0, int dwExtraInfo = 0)
        {
            mouse_event((int)action, x, y, dwData, dwExtraInfo);
        }

        private static void ExecuteClicking(Point cursorPosition, int clicksMode, int intervalTime, MouseEventFlags downFlag, MouseEventFlags upFlag, CancellationToken token)
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