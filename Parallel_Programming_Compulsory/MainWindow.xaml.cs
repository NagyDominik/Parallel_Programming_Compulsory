using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

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
            ParseFields(out long start, out long end);

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
            lblCount.Content = dataSource.Count;
            ChangeStatusText(string.Format("Runtime: {0} sec", stopwatch.ElapsedMilliseconds / 1000d));
            stopwatch.Reset();
        }

        public async void StartParallel(object sender, RoutedEventArgs e)
        {
            ParseFields(out long start, out long end);

            ChangeStatusText("Generating...");
            stopwatch.Start();
            Task gen = Task.Factory.StartNew(() =>
            {
                return generator.GetPrimesParallel(start, end);
            }).ContinueWith((task) =>
            {
                dataSource = new ObservableCollection<long>(task.Result);
            });
            await gen;
            stopwatch.Stop();
            listboxResult.ItemsSource = dataSource;
            lblCount.Content = dataSource.Count;
            ChangeStatusText(string.Format("Runtime: {0} sec", stopwatch.ElapsedMilliseconds / 1000d));
            stopwatch.Reset();
        }

        private void ChangeStatusText(string text)
        {
            lblStatus.Content = text;
        }

        private void ParseFields(out long start, out long end)
        {
            if (!long.TryParse(fieldStart.Text, out start))
            {
                MessageBox.Show("Range start is not a number!");
            }
            if (!long.TryParse(fieldEnd.Text, out end))
            {
                MessageBox.Show("Range end is not a number!");
            }
            return;
        }
    }
}