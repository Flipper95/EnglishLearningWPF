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

namespace SystemForEnglishLearning.Tests
{
    /// <summary>
    /// Interaction logic for TestResult.xaml
    /// </summary>
    public partial class TestResult : Window, ITestResultView
    {
        int fontSize;

        TestResult(WindowState state)
        {
            InitializeComponent();
            CreateElements();
            this.WindowState = state;
            fontSize = WindowStateCheck();
        }

        TestResult(double left, double top, WindowState state)
            : this(state)
        {
            this.Left = left;
            this.Top = top;
        }

        public TestResult(int userId, TestsModel test, bool save, double left, double top, WindowState state)
            : this(left, top, state)
        {
            new TestResultPresenter(this, userId, test, save);
        }

        public TestResult(int userId, TestsModel test, bool save, WindowState state)
            : this(state)
        {
            new TestResultPresenter(this, userId, test, save);
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
        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border_MouseLeftButtonDown(sender, e);
            this.Close();
        }

        void CreateElements()
        {

            Border bord = DynamicElements.CreateBorder(this.FindResource("BorderStyle") as Style);
            DynamicElements.SetRowColumnProperties(bord, mainGrid.RowDefinitions.Count - 1, 1, 1, 1);
            Viewbox box = DynamicElements.CreateViewBoxLabel("Выйти", 0);
            bord.Child = box;
            bord.Margin = new Thickness(1, 1, 2, 2);
            mainGrid.Children.Add(bord);
        }

        public void SetPercent(float percent)
        {
            Border bord_percent = new Border();
            bord_percent.Margin = new Thickness(2, 1, 1, 2);
            if (percent > 70)
            {
                bord_percent.Background = Brushes.GreenYellow;
            }
            else if (percent <= 70 && percent > 40)
            {
                bord_percent.Background = Brushes.Yellow;
            }
            else if (percent >= 0 && percent <= 40)
            {
                bord_percent.Background = Brushes.PaleVioletRed;
            }
            Viewbox vb_percent = new Viewbox();
            bord_percent.Child = vb_percent;
            Label lb = new Label();
            lb.Content = "Правильных: " + percent + "%";
            vb_percent.Child = lb;
            DynamicElements.SetRowColumnProperties(bord_percent, mainGrid.RowDefinitions.Count - 1, 0, 1, 1);
            mainGrid.Children.Add(bord_percent);
        }

        public void SetMainData(TestsModel test)
        {
            int questCount = 0;
            foreach (QuestionsModel quest in test.Questions)
            {
                int count = 0;
                Grid grid = DynamicElements.CreateGrid(5, quest.Answers.Count + 3, GridUnitType.Star, GridUnitType.Star);
                RichTextBox rtb = new RichTextBox();
                rtb.Name = "rtb_Question" + quest.Id;
                DynamicElements.SetRowColumnProperties(rtb, count, 1, 4, 2);
                Viewbox number = DynamicElements.CreateViewBoxLabel((questCount+1) + ".", 0);
                DynamicElements.SetRowColumnProperties(number, count, 0, 1, 1);
                grid.Children.Add(number);
                count += 3;
                rtb.AppendText(quest.Text);
                rtb.FontSize = fontSize;
                grid.Children.Add(rtb);
                foreach (AnswersModel answer in quest.Answers)
                {
                    Border bord = new Border();
                    bord.Margin = new Thickness(1, 3, 1, 1);
                    bord.MaxHeight = 100;
                    if (answer.UserChoice == true && answer.Rightness == true)
                    {
                        bord.Background = this.FindResource("Green") as Brush;
                    }
                    else if (answer.UserChoice == true)
                    {
                        bord.Background = Brushes.PaleVioletRed;
                    }
                    else if (answer.Rightness == true)
                    {
                        bord.Background = Brushes.LightGreen;
                    }
                    Viewbox vb = new Viewbox();
                    vb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    CheckBox cb = DynamicElements.CreateCheckBox(answer.UserChoice);
                    vb.Child = cb;
                    cb.Content = answer.Text;
                    bord.Child = vb;
                    DynamicElements.SetRowColumnProperties(bord, count, 1, 3, 1);
                    count++;
                    grid.Children.Add(bord);
                }
                panel.Children.Add(grid);
                questCount++;
            }
        }

        public event EventHandler Window_StateChanged = null;
        private void Window_StateChanged_1(object sender, EventArgs e)
        {
            fontSize = WindowStateCheck();
            Window_StateChanged(sender, e);
        }

        int WindowStateCheck()
        {
            return (this.WindowState == System.Windows.WindowState.Maximized) ? 40 : 15;
        }

        public void ChangeQuestionsSize(List<QuestionsModel> questions) {
            foreach (QuestionsModel el in questions) {
                RichTextBox rtb = LogicalTreeHelper.FindLogicalNode(mainGrid, "rtb_Question"+el.Id) as RichTextBox;
                rtb.FontSize = fontSize;
            }
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Border_MouseLeftButtonDown(sender, e);
        }

    }
}
