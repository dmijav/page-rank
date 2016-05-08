using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace PageRankSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string filename;
        private const string pleaseSelect  = "Please select a file";
        private const string wrongFileNameMsg = "Cannot load XLS data";
        private const string AppName = "PageRank";

        private void load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.InitialDirectory = "c:\\";
            dlg.DefaultExt = ".xls";
            dlg.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
              filename = dlg.FileName;
            }
        }

        private async void calculate_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show(pleaseSelect, AppName);
            }
            Dictionary<string, float> view = new Dictionary<string, float>();
            await Task.Run(() => {
                try
                {
                   view = new CalculateHandler().Calculate(filename);
        
                }
                catch (Exception ex)
                {
                    MessageBox.Show(wrongFileNameMsg, AppName);
                }
            });
            DisplayData(view);
        }

        private void DisplayData(Dictionary<string, float> view)
        {
            foreach (KeyValuePair<string, float> el in view)
            {
                textBox.AppendText(el.Key + ":  " + el.Value);
                textBox.AppendText(Environment.NewLine);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
