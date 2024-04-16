using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace ag
{
    internal static class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetAsyncKeyState(int vKey);

        [STAThread]
        static void Main()
        {
            while (true)
        {
            if ((GetAsyncKeyState((int) Keys.ShiftKey) && (GetAsyncKeyState((int) Keys.CapsLock))))
            {
                break;
            }
            System.Threading.Thread.Sleep(1);
        }


            if (CheckInternetConnection())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new NLogin());
            }
            else
            {
                MessageBox.Show("You need WiFi to continue!");
                Environment.Exit(0);
                Application.Exit();
                Application.ExitThread();
            }
        }
         
        public static int goodday = 38473823;
         
        public static bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
