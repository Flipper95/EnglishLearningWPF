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

namespace SystemForEnglishLearning.WordLearning
{
    /// <summary>
    /// Interaction logic for WordLearningChoice.xaml
    /// </summary>
    public partial class WordLearningChoice : Window, IWordLearningChoiceView
    {
        public WordLearningChoice()
        {
            InitializeComponent();
        }

        WordLearningChoice(double left, double top): this() {
            this.Left = left;
            this.Top = top;
        }

        public WordLearningChoice(int userId, double left, double top)
            : this(left,top)
        {
            new WordLearnChoicePresenter(this, userId);
        }

        public WordLearningChoice(int userId)
        {
            new WordLearnChoicePresenter(this, userId);
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

        public event EventHandler Grid_MouseRightButtonDown = null;
        private void Grid_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Grid_MouseRightButtonDown(sender, e);
        }

        public void SendMessage(string message) {
            MessageBox.Show(message);
        }
    }
}
