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

//using System.Windows.Markup;
//using System.IO;
using System.Windows.Xps.Packaging;

namespace SystemForEnglishLearning.Lections
{
    /// <summary>
    /// Interaction logic for Lection.xaml
    /// </summary>
    public partial class Lection : Window, ILectionView
    {
        Lection()
        {
            InitializeComponent();
        }

        Lection(double left, double top)
            : this()
        {
            this.Left = left;
            this.Top = top;
        }

        public Lection(int userId, int lectionId, List<LectionsModel> lections, double left, double top)
            : this(left, top)
        {
            new LectionPresenter(this, userId, lectionId, lections);
        }

        public Lection(int userId, int lectionId, List<LectionsModel> lections)
            : this()
        {
            new LectionPresenter(this, userId, lectionId, lections);
        }

        private void Button_LeftMouseButtonDown(object sender, MouseEventArgs e)
        {
            Document.Background = (sender as Button).Background;
        }

        public event EventHandler lectionBtn_Click = null;
        private void LectionBtn_LeftMouseButtonDown(object sender, MouseEventArgs e)
        {
            lectionBtn_Click(sender, e);
        }

        public event EventHandler testBtn_Click = null;
        private void TestBtn_LeftMouseButtonDown(object sender, MouseEventArgs e)
        {
            testBtn_Click(sender, e);
        }

        public void SetDataSideMenu(List<LectionsModel> lectionsList)
        {
            Style btnStyle = this.FindResource("btn_Choice") as Style;
            ScrollViewer scroll = new ScrollViewer();
            DynamicElements.SetRowColumnProperties(scroll, 0, 0, 1, 1);
            StackPanel panel = new StackPanel();

            Style testStyle = this.FindResource("btn_Test") as Style;
            Button btn_test = new Button();
            btn_test.MinHeight = 50;
            btn_test.MaxHeight = 50;
            btn_test.Name = "btn_Test";
            btn_test.Background = Brushes.Transparent;
            btn_test.Style = testStyle;
            btn_test.Content = DynamicElements.CreateViewBoxLabel("Тест по уроку", 0);
            panel.Children.Add(btn_test);
            for (int i = 0; i < lectionsList.Count; i++)
            {
                Button btn = new Button();
                btn.MinHeight = 50;
                btn.MaxHeight = 50;
                btn.Style = btnStyle;
                btn.Background = Brushes.Transparent;
                btn.Content = DynamicElements.CreateViewBoxLabel(lectionsList[i].Name, lectionsList[i].Id);
                btn.Tag = lectionsList[i].Id;
                DynamicElements.SetRowColumnProperties(btn, i, 0, 1, 1);
                panel.Children.Add(btn);
            }
            scroll.Content = panel;
            mainGrid.Children.Add(scroll);
        }

        public void SetMainData(FixedDocumentSequence lection)
        {
            Document.Document = lection;
        }

        public int GetNewLectionId(object sender)
        {
            return Convert.ToInt32((sender as Button).Tag);
        }

        public void ContentNullException(string message)
        {
            MessageBox.Show(message);
        }

        public void TestControlEnabled(bool enabled)
        {
            Button btn = LogicalTreeHelper.FindLogicalNode(mainGrid, "btn_Test") as Button;
            if (enabled) btn.Visibility = Visibility.Visible;
            else btn.Visibility = Visibility.Collapsed;
        }

    }
}
