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
using SystemForEnglishLearning.WordLearning;

namespace SystemForEnglishLearning.Lections
{
    /// <summary>
    /// Interaction logic for LectionChoice.xaml
    /// </summary>
    public partial class LectionChoice : Window, ILectionChoiceView
    {
        LectionChoice()
        {
            InitializeComponent();
            //new LectionChoicePresenter(this, 1);
        }

        LectionChoice(double left, double top)
            : this()
        {
            this.Left = left;
            this.Top = top;
        }

        public LectionChoice(int userId, double left, double top)
            : this(left, top)
        {
            new LectionChoicePresenter(this, userId);
        }

        public LectionChoice(int userId)
        {
            new LectionChoicePresenter(this, userId);
        }

        public void SetData<T>(List<T> data)
        {
            Tree.ItemsSource = data;
        }

        public event EventHandler Item_DoubleClick = null;
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item_DoubleClick(sender, e);
        }

        public object GetChoosenModel(object sender) {
            return (sender as TreeViewItem).Header;
        }

        public object GetGroupModel(object sender) {
            return (sender as TreeViewItem).Header;
        }

        public event EventHandler Border_MouseEnter = null;
        private void Border_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Border_MouseEnter(sender, e);
        }

        public event EventHandler Border_MouseLeave = null;
        private void Border_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Border_MouseLeave(sender, e);
        }

        public void SendMessage(string message) {
            MessageBox.Show(message);
        }

    }
}
