
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace BusSimulator.Ui.Logic.Utils
{
    public class KeyboardListener : IDisposable
    {
        private readonly IntPtr hookId = IntPtr.Zero;
        private readonly InterceptKeys.LowLevelKeyboardProc hookCallback;

        public event EventHandler<RawKeyEventArgs> KeyDown;
        public event EventHandler<RawKeyEventArgs> KeyUp;

        public KeyboardListener()
        {
            this.hookCallback = this.HookCallback;
            this.hookId = InterceptKeys.SetHook(this.hookCallback);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                return this.HookCallbackInner(nCode, wParam, lParam);
            }
            catch
            {
                Debug.WriteLine("There was some error somewhere...");
            }
            return InterceptKeys.CallNextHookEx(this.hookId, nCode, wParam, lParam);
        }

        private IntPtr HookCallbackInner(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr) InterceptKeys.WM_KEYDOWN)
                {
                    this.HandleEvent(KeyDown, lParam);
                }
                else if (wParam == (IntPtr) InterceptKeys.WM_KEYUP)
                {
                    this.HandleEvent(KeyUp, lParam);
                }
            }

            return InterceptKeys.CallNextHookEx(this.hookId, nCode, wParam, lParam);
        }

        private void HandleEvent(EventHandler<RawKeyEventArgs> handler, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            handler?.Invoke(this, new RawKeyEventArgs(vkCode));
        }

        ~KeyboardListener()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            InterceptKeys.UnhookWindowsHookEx(this.hookId);
        }
    }

    internal static class InterceptKeys
    {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static int WH_KEYBOARD_LL = 13;
        public static int WM_KEYDOWN = 0x0100;
        public static int WM_KEYUP = 0x0101;

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }

    public class RawKeyEventArgs : EventArgs
    {
        public Key Key;

        public RawKeyEventArgs(int VKCode)
        {
            this.Key = KeyInterop.KeyFromVirtualKey(VKCode);
        }
    }

    public delegate void RawKeyEventHandler(object sender, RawKeyEventArgs args);
}
