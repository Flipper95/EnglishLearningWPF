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

namespace SystemForEnglishLearning.Register
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window, IRegistrationView
    {
        public Registration()
        {
            InitializeComponent();
            new RegisterPresenter(this);
        }

        public event EventHandler RegisterButton_Click = null;
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterButton_Click(sender, e);
        }

        public string LoginText
        {
            get
            {
                return Login.Text;
            }
        }

        public string PasswordText
        {
            get
            {
                return Password.Password;
            }
        }

        public void SendMessage(string message)
        {
            MessageBox.Show(message);
        }

    }
}
