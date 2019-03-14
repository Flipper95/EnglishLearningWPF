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

namespace SystemForEnglishLearning.Tests
{
    /// <summary>
    /// Interaction logic for TestChoice.xaml
    /// </summary>
    public partial class TestChoice : Window, ITestChoiceView
    {

        TestChoice()
        {
            InitializeComponent();
        }

        TestChoice(double left, double top)
            : this()
        {
            this.Left = left;
            this.Top = top;
        }

        public TestChoice(int userId, double left, double top)
            : this(left, top)
        {
            new TestChoicePresenter(this, userId);
        }

        public TestChoice(int userId)
        {
            new TestChoicePresenter(this, userId);
        }

        public void SetData<T>(List<T> data, bool userTree)
        {
            if (userTree) myTree.ItemsSource = data;
            else Tree.ItemsSource = data;
        }

        public event EventHandler Item_DoubleClick = null;
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item_DoubleClick(sender, e);
        }

        public event EventHandler Delete_Click = null;
        protected void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить тест?", "Удалить?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Delete_Click(sender, e);
            }
        }

        public int GetIdToDelete(object sender) {
            return Convert.ToInt32((sender as Button).Tag);
        }

        public object GetChoosenModel(object sender)
        {
            return (sender as TreeViewItem).Header;
        }

        public event EventHandler CreateTest_Click = null;
        private void CreateButton_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            CreateTest_Click(sender, e);
            //Create add test window
        }

        public void SendMessage(string message) {
            MessageBox.Show(message);
        }

    }
}
