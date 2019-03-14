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

namespace SystemForEnglishLearning
{
    /// <summary>
    /// Interaction logic for MainChoice.xaml
    /// </summary>
    public partial class MainChoice : Window, IMainChoiceView
    {
        public MainChoice()
        {
            InitializeComponent();
        }

        MainChoice(double left, double top): this() {
            this.Left = left;
            this.Top = top;
        }

        public MainChoice(int UserId, double left, double top):this(left, top){
            new ChoicePresenter(this, UserId);
        }

        public MainChoice(int UserId):this() {
            new ChoicePresenter(this, UserId);
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

        public event EventHandler Border_MouseLeftButtonDown = null;
        private void Border_MouseLeftButtonDown_1(object sender, RoutedEventArgs e)
        {
            Border_MouseLeftButtonDown(sender, e);
        }
    }
}
