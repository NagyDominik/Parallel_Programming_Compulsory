using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
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

namespace Parallel_Programming_Compulsory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PrimeGenerator generator = new PrimeGenerator();
        public ObservableCollection<long> dataSource;
        private Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
        }

        public async void StartSequential(object sender, RoutedEventArgs e)
        {
            long start;
            long end;
            if (!long.TryParse(fieldStart.Text, out start))
            {
                MessageBox.Show("Range start is not a number!");
                return;
            }
            if (!long.TryParse(fieldEnd.Text, out end))
            {
                MessageBox.Show("Range end is not a number!");
                return;
            }

            ChangeStatusText("Generating...");
            stopwatch.Start();
            Task gen = Task.Factory.StartNew(() =>
            {
                return generator.GetPrimesSequential(start, end);
            }).ContinueWith((task) =>
            {
                dataSource = new ObservableCollection<long>(task.Result);
            });
            await gen;
            stopwatch.Stop();
            listboxResult.ItemsSource = dataSource;
            ChangeStatusText(string.Format("Runtime: {0} sec", stopwatch.ElapsedMilliseconds / 1000d));
            stopwatch.Reset();
        }

        public async void StartParallel(object sender, RoutedEventArgs e)
        {
        }

        private void ChangeStatusText(string text)
        {
            lblStatus.Content = text;
        }
    }
}