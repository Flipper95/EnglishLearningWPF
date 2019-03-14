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
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window, ITestView
    {

        Grid grid;
        int questFontSize;

        Test(WindowState state)
        {
            InitializeComponent();
            grid = DynamicElements.CreateGrid(4, 0, GridUnitType.Star, GridUnitType.Star);
            DynamicElements.SetRowColumnProperties(grid, 0, 1, mainGrid.ColumnDefinitions.Count - 1, mainGrid.RowDefinitions.Count - 1);
            mainGrid.Children.Add(grid);
            this.WindowState = state;
            questFontSize = WindowStateCheck();
        }

        Test(double left, double top, WindowState state)
            : this(state)
        {
            this.Left = left;
            this.Top = top;
        }

        public Test(int userId, TestsModel test, double left, double top, WindowState state)
            : this(left, top, state)
        {
            new TestPresenter(this, userId, test);
        }

        public Test(int userId, TestsModel test, WindowState state)
            : this(state)
        {
            new TestPresenter(this, userId, test);
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
        }

        public event EventHandler NextBorder_MouseLeftButtonDown = null;
        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            NextBorder_MouseLeftButtonDown(sender, e);
        }

        public void ClearGrid() {
            grid.RowDefinitions.Clear();
            grid.Children.Clear();
            mainGrid.Children.Remove(LogicalTreeHelper.FindLogicalNode(mainGrid, "NextBorder") as UIElement);
        }

        public void SetAllQuestions(List<QuestionsModel> questions) {
            ScrollViewer scroll = new ScrollViewer();
            DynamicElements.SetRowColumnProperties(scroll, 0, 0, 1, mainGrid.RowDefinitions.Count);
            StackPanel panel = new StackPanel();
            for (int i = 0; i < questions.Count; i++) {
                Style bordStyle = this.FindResource("ChoiceBorderStyle") as Style;
                Border bord = DynamicElements.CreateBorder(bordStyle);
                bord.MaxHeight = 50;
                bord.MinHeight = 50;
                bord.Name = "questionsBord" + questions[i].Id;
                bord.Margin = new Thickness(0,2,0,0);
                bord.Tag = questions[i].Id;
                Viewbox box = DynamicElements.CreateViewBoxLabel((i+1).ToString(), questions[i].Id);
                bord.Child = box;
                panel.Children.Add(bord);
            }
            scroll.Content = panel;
            mainGrid.Children.Add(scroll);
        }

        public void SetQuestion(QuestionsModel quest)
        {
            AddRow(grid);
            AddRow(grid);
            AddRow(grid);
            RichTextBox rich = new RichTextBox();
            rich.Name = "rtb_Question";
            rich.FontSize = questFontSize;
            rich.Background = Brushes.Transparent;
            rich.AppendText(quest.Text);
            DynamicElements.SetRowColumnProperties(rich, 0, 0, grid.ColumnDefinitions.Count, 3);
            grid.Children.Add(rich);
        }

        public void SetAnswers(List<AnswersModel> answers)
        {
            int count = grid.RowDefinitions.Count;
            int i = 1;
            foreach (AnswersModel answer in answers)
            {
                AddRow(grid);

                Viewbox vb = new Viewbox();
                DynamicElements.SetRowColumnProperties(vb, count, 0, grid.ColumnDefinitions.Count, 1);
                CheckBox cb = DynamicElements.CreateCheckBox(answer.UserChoice);
                cb.Name = "Checkbox" + i;
                i++;
                cb.Tag = answer.Id;
                cb.Content = answer.Text;
                vb.Child = cb;
                vb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                grid.Children.Add(vb);

                count++;
            }

        }

        public void SetNextButton(bool end, int questionId) {
            Style bordStyle = this.FindResource("NextBorderStyle") as Style;
            Border bord = DynamicElements.CreateBorder(bordStyle, mainGrid.RowDefinitions.Count, 1, mainGrid.ColumnDefinitions.Count-1, 1);
            bord.Name = "NextBorder";
            bord.Margin = new Thickness(5);
            bord.Tag = questionId;
            string text = null;
            if (!end)
            {
                text = "Следующее";
            }
            else
            {
                text = "Закончить тест";
            }
            Viewbox box = DynamicElements.CreateViewBoxLabel(text, questionId);
            bord.Child = box;
            mainGrid.Children.Add(bord);
        }

        void AddRow(Grid grid)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(row);
        }

        public int GetIndex(object sender) {
            return Convert.ToInt32((sender as Border).Tag);
        }

        public List<int> GetCheckedAnswers(int answerCount) {
            List<int> checkedId = new List<int>();
            for (int i = 0; i < answerCount; i++) {
                CheckBox box = LogicalTreeHelper.FindLogicalNode(grid, "Checkbox" + (i + 1)) as CheckBox;
                if (box.IsChecked == true) {
                    checkedId.Add(Convert.ToInt32(box.Tag));
                }
            }
            return checkedId;
        }

        private void Window_StateChanged_1(object sender, EventArgs e)
        {
            questFontSize = WindowStateCheck();
            RichTextBox rtb = LogicalTreeHelper.FindLogicalNode(grid, "rtb_Question") as RichTextBox;
            rtb.FontSize = questFontSize;
        }

        int WindowStateCheck() {
            return (this.WindowState == System.Windows.WindowState.Maximized) ? 40 : 15;
        }

        public void SetCompleteColor(int id, bool answered) {
            Border bord = LogicalTreeHelper.FindLogicalNode(mainGrid, "questionsBord" + id) as Border;
            if (answered)
            {
                bord.Background = Brushes.PaleGoldenrod;
            }
            else bord.Background = this.FindResource("BlueBrush") as Brush;
        }

        public void SendMessage(string message) {
            MessageBox.Show(message);
        }

    }
}
