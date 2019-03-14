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

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    /// <summary>
    /// Interaction logic for Equivalent.xaml
    /// </summary>
    public partial class Equivalent : Window, IEquivalentView
    {
        public Equivalent()
        {
            InitializeComponent();
        }

        Equivalent(double left, double top): this() {
            this.Left = left;
            this.Top = top;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public Equivalent(int userId, string type, double left, double top)
            : this(left,top)
        {
            switch (type) {
                case ("translate"): {
                    TranslatePresenter presenter = new TranslatePresenter(this, userId);
                    presenter = null;
                    break;
                }
                case ("equivalent"): {
                    EquivalentPresenter presenter = new EquivalentPresenter(this, userId);
                    break;
                }
                case ("synonym"): {
                    SynonymsPresenter presenter = new SynonymsPresenter(this, userId);
                    break;
                }
                case ("listening"): {
                    ListeningPresenter presenter = new ListeningPresenter(this, userId);
                    break;
                }
                case ("constructor"): {
                    ConstructorPresenter presenter = new ConstructorPresenter(this, userId);
                    break;
                }
            }
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

        public event EventHandler Image_MouseLeftButtonDown = null;
        private void Image_MouseLeftButtonDown_1(object sender, RoutedEventArgs e)
        {
            Image_MouseLeftButtonDown(sender, e);
        }

        public event EventHandler MediaEnded = null;
        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                MediaEnded(sender, e);
            }
            catch (Exception ex) { }
        }

        public event EventHandler Variant_MouseLeftButtonDown = null;
        private void Variant_MouseLeftButtonDown_1(object sender, RoutedEventArgs e)
        {
            Variant_MouseLeftButtonDown(sender, e);
        }

        public event EventHandler Next_MouseLeftButtonDown = null;
        private void Next_MouseLeftButtonDown_1(object sender, RoutedEventArgs e)
        {
            Next_MouseLeftButtonDown(sender, e);
        }

        public event EventHandler Complete_MouseLeftButtonDown = null;
        private void Complete_MouseLeftButtonDown_1(object sender, RoutedEventArgs e)
        {
            Complete_MouseLeftButtonDown(sender, e);
        }

        public event EventHandler Window_Closing = null;
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window_Closing(sender, e);
        }

        public void SendMessage(string message){
            MessageBox.Show(message);
        }

    }
}
