using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;

namespace WpfApp3keybordhook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LowLevelKeyboardListener _listener;
        string k;
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string sClassName, string sAppName);
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private IntPtr thiswindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StreamWriter file2 = new StreamWriter("hookcontents.txt", true);
            file2.WriteLine("");
            file2.Close();

            thiswindow = FindWindow(null, "MainWindow");
            RegisterHotKey(thiswindow, 1 , (uint)fsModifiers.Control, (uint)Keys.T);

            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;

            _listener.HookKeyboard();
        }

        public enum fsModifiers
        {
            Alt= 0x0001,
            Control = 0x0002,
            Shift = 0x0004,
            Window = 0x0008,
        }

        private void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            this.textBox_DisplayKeyboardInput.Text += e.KeyPressed.ToString();
            this.k = e.KeyPressed.ToString();
            //this.file.Write(e.KeyPressed.ToString());
            samplebox.Clear();
            samplebox.Text = k;
            Writer(k);
        }

        private void Writer(string key)
        {
            StreamWriter file = new StreamWriter("hookcontents.txt",true);
            file.Write(key);
            file.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UnregisterHotKey(thiswindow, 1);

            _listener.UnHookKeyboard();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        /*protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                MessageBox.Show("wow");
            }
            base.WndProc(ref m);
        }*/
    }
}
