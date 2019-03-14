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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemForEnglishLearning.Register
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IEnterView
    {
        public MainWindow()
        {
            InitializeComponent();
            new EnterPresenter(this);
        }

        public event EventHandler EnterButton_Click = null;
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            EnterButton_Click(sender, e);
        }

        public event EventHandler RegisterButton_Click = null;
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterButton_Click(sender, e);
        }

        public string LoginText {
            get {
                return Login.Text;
            }
        }

        public string PasswordText {
            get {
                return Password.Password;
            }
        }

        public void SendMessage(string message) {
            MessageBox.Show(message);
        }

    }
}
