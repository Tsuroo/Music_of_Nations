using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public class InterceptKeys
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    public static event EventHandler OnTildePressedThreeTimesFast;
    public static event EventHandler OnRightControlPressedThreeTimesFast;

    private static int tildeClicks = 0;
    private static DateTime timeSinceLastTildeClick = DateTime.Now;

    private static int rightControlClicks = 0;
    private static DateTime timeSinceLastRightControlClick = DateTime.Now;

    /// <summary>
    /// The static constructor for this object.  It only sets the hook and returns.
    /// </summary>
    static InterceptKeys()
    {
        _hookID = SetHook(_proc);
    }

    /// <summary>
    /// Sets the hook for key press events from the operating system.
    /// </summary>
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process currentProcess = Process.GetCurrentProcess())
        using (ProcessModule currentModule = currentProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(currentModule.ModuleName), 0);
        }
    }

    /// <summary>
    /// Called every time a key is pressed, regardless of focus.
    /// </summary>
    /// <param name="nCode"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // If the key is down
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            // Get the current key code
            int vkCode = Marshal.ReadInt32(lParam);

            // If the user clicked the tilde
            if (((Keys)vkCode).Equals(Keys.Oemtilde))
            {
                // If there haven't been any tilde clicks
                if (tildeClicks == 0)
                {
                    // Increment the click count
                    ++tildeClicks;

                    // Set this as the last time clicked
                    timeSinceLastTildeClick = DateTime.Now;
                }
                else
                {
                    // If this click is within 500 milliseconds of the last click
                    if (DateTime.Now < timeSinceLastTildeClick.AddMilliseconds(500))
                    {
                        // Increment the click count
                        ++tildeClicks;

                        // Set this as the last time clicked
                        timeSinceLastTildeClick = DateTime.Now;

                        // If this is the third click
                        if (tildeClicks == 3)
                        {
                            // Fire the event
                            OnTildePressedThreeTimesFast(null, EventArgs.Empty);

                            // Set the tilde counter back to 0
                            tildeClicks = 0;
                        }
                    }
                    else
                    {
                        // set the counter back to 0
                        tildeClicks = 0;
                    }
                }
            }
            // Else, if the user clicked the right control
            else if (((Keys)vkCode).Equals(Keys.RControlKey))
            {
                // If there haven't been any right control clicks
                if (rightControlClicks == 0)
                {
                    // Increment the click count
                    ++rightControlClicks;

                    // Set this as the last time clicked
                    timeSinceLastRightControlClick = DateTime.Now;
                }
                else
                {
                    // If this click is within 500 milliseconds of the last click
                    if (DateTime.Now < timeSinceLastRightControlClick.AddMilliseconds(500))
                    {
                        // Increment the click count
                        ++rightControlClicks;

                        // Set this as the last time clicked
                        timeSinceLastRightControlClick = DateTime.Now;

                        // If this is the third click
                        if (rightControlClicks == 3)
                        {
                            // Fire the event
                            OnRightControlPressedThreeTimesFast(null, EventArgs.Empty);

                            // Set the right control counter back to 0
                            rightControlClicks = 0;
                        }
                    }
                    else
                    {
                        // set the counter back to 0
                        rightControlClicks = 0;
                    }
                }
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}