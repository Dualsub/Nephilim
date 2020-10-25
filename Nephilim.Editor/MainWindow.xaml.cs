using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

using Nephilim.Engine.Util;

namespace Nephilim.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Log.SetLoggningMethod(logValue);
            //Task.Run(() => DebugTest());
        }

        public void DebugTest()
        {
            for (int i = 0; i < 10; i++)
            {
                var r = new Random();
                Thread.Sleep(r.Next(100, 5000));
                Log.Print("Random Debug Message.");
            }
        }

        public void logValue(string value)
        {
            var textBox = new TextBlock();
            textBox.Text = value;
            //ConsoleList.Items.Add(textBox);
        }
    }
}
