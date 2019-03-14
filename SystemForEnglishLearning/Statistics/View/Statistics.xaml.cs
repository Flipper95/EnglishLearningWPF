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
using System.Windows.Shapes;

namespace SystemForEnglishLearning.Statistics
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window, IStatisticsView
    {
        public Statistics()
        {
            InitializeComponent();
        }

        Statistics(double left, double top)
            : this()
        {
            this.Left = left;
            this.Top = top;
        }

        public Statistics(int userId, double left, double top)
            : this(left, top)
        {
            new StatisticsPresenter(this, userId);
        }

        public Statistics(int userId)
            : this()
        {
            new StatisticsPresenter(this, userId);
        }

        public event EventHandler History_Click = null;
        private void History_Click_1(object sender, RoutedEventArgs e)
        {
            History_Click(sender, e);
        }

        public void SetChartData<T>(List<T> data, string title, string chartText) {
            Chart.ItemsSource = data;
            Chart.DataContext = data;
            Chart.Title = title;
            chart_Text.Text = chartText;
        }

        public void SetComboBoxData(string[] data) {
            cb_Choice.ItemsSource = data;
        }

        public void ErrorMessage(string message) {
            MessageBox.Show(message);
        }

        public string GetChosenText(object sender) {
            return (sender as ComboBox).SelectedItem as string;
        }

        public event EventHandler cb_Choiced = null;
        private void cb_Choice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_Choiced(sender, e);
        }

    }
}
