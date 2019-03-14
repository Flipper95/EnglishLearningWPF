using System;
using System.Collections.Generic;
using System.Data;
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

namespace SystemForEnglishLearning.Tests
{
    /// <summary>
    /// Interaction logic for TestHistory.xaml
    /// </summary>
    public partial class TestHistory : Window, ITestHistoryView
    {
        TestHistory()
        {
            InitializeComponent();
        }

        TestHistory(double left, double top)
            : this()
        {
            this.Left = left;
            this.Top = top;
        }

        public TestHistory(int userId, double left, double top)
            : this(left, top)
        {
            new TestHistoryPresenter(this, userId);
        }

        public TestHistory(int userId)
            : this()
        {
            new TestHistoryPresenter(this, userId);
        }

        public void SetHistoryData<T>(List<T> data) {
            dataGrid.DataContext = null;
            dataGrid.DataContext = data;
        }

        public event EventHandler Row_DoubleClick = null;
        private void Row_DoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            Row_DoubleClick(sender, e);
        }

        public object GetSelectedRow(object sender) {
            return (sender as DataGridRow).Item;
        }

    }
}
