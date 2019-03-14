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
    /// Interaction logic for CreateTest.xaml
    /// </summary>
    public partial class CreateTest : Window, ICreateTestView
    {
        int answerCount;

        CreateTest() {
            InitializeComponent();
        }

        CreateTest(WindowState state)
        {
            InitializeComponent();
            this.WindowState = state;
        }

        CreateTest(double left, double top, WindowState state)
            : this(state)
        {
            this.Left = left;
            this.Top = top;
        }

        public CreateTest(int userId, double left, double top, WindowState state)
            : this(left, top, state)
        {
            new CreateTestPresenter(this, userId);
        }

        public CreateTest(int userId, WindowState state)
            : this(state)
        {
            new CreateTestPresenter(this, userId);
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
            ChangeEnableSaveDelete(true);
            Border_MouseLeftButtonDown(sender, e);
        }

        public event EventHandler Btn_SaveClick = null;
        private void Save_Click_1(object sender, RoutedEventArgs e)
        {
            sp_Questions.IsEnabled = true;
            btn_Complete.IsEnabled = true;
            Btn_SaveClick(sender, e);
        }

        public event EventHandler Btn_DeleteClick = null;
        private void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeEnableSaveDelete(false);
            Btn_DeleteClick(sender, e);
            sp_Questions.IsEnabled = true;
            btn_Complete.IsEnabled = true;
        }

        void CreateMainContent()
        {
        }

        public event EventHandler AddQuest_Click = null;
        private void AddQuest_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeEnableSaveDelete(true);
            AddQuest_Click(sender, e);
        }

        public event EventHandler AddAnswer_Click = null;
        private void AddAnswer_Click_1(object sender, RoutedEventArgs e)
        {
            AddAnswer_Click(sender, e);
        }

        public event EventHandler Complete_Click = null;
        private void Complete_Click_1(object sender, RoutedEventArgs e)
        {
            Complete_Click(sender, e);
        }

        public event EventHandler End_Click = null;
        private void End_Click_1(object sender, RoutedEventArgs e)
        {
            End_Click(sender, e);
        }

        public void AddNewQuestion(object sender, int questNum) {
            Button bt = sender as Button;
            Border bord = new Border();
            bord.Name = "border" + (questNum*-1);
            bord.Tag = questNum;
            bord.Style = this.FindResource("ChoiceBorderStyle") as Style;
            bord.Child = DynamicElements.CreateViewBoxLabel((questNum*-1).ToString(), questNum);
            sp_Questions.Children.Add(bord);
            sp_Questions.Children.Remove(bt);
            sp_Questions.Children.Add(bt);
            sp_Questions.IsEnabled = false;
            btn_Complete.IsEnabled = false;
            SetQuestion(new QuestionsModel(questNum, 0, ""));
        }

        public void SetQuestion(QuestionsModel question) {
            ClearGrid();
            AddRow(grid_QuestData, 1);
            Viewbox box = DynamicElements.CreateViewBoxLabel("Введите вопрос:", 0);
            grid_QuestData.Children.Add(box);
            AddRow(grid_QuestData, 2);
            RichTextBox rich = new RichTextBox();
            rich.Name = "rtb_Question";
            rich.FontSize = 20;
            rich.Tag = question.Id;
            rich.AppendText(question.Text);
            DynamicElements.SetRowColumnProperties(rich, 1, 0, 1, 2);
            grid_QuestData.Children.Add(rich);
            AddRow(grid_QuestData, 4);
            ScrollViewer scroll = new ScrollViewer();
            StackPanel panel = new StackPanel();
            Button btn = DynamicElements.CreateButton("Добавить ответ");
            btn.Style = this.FindResource("btn_AddAnswer") as Style;
            panel.Children.Add(btn);
            scroll.Content = panel;
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            DynamicElements.SetRowColumnProperties(scroll, 3, 1, 1, 4);
            grid_QuestData.Children.Add(scroll);
            answerCount = 0;
            if (question.Answers.Count == 0)
            {
                AddAnswer(btn, new AnswersModel(answerCount, question.Id, "", false));
            }
            else {
                foreach (AnswersModel el in question.Answers) {
                    AddAnswer(btn, el);
                }
            }
        }

        public void AddNewAnswer(object sender) {
            AddAnswer(sender, new AnswersModel(answerCount, 0, "", false));//null
        }

        void AddAnswer(object sender, AnswersModel answer) {
            StackPanel panel = ((sender as Button).Parent as StackPanel);
            Viewbox labelBox = DynamicElements.CreateViewBoxLabel("Ответ №" +(answerCount + 1), 0);
            labelBox.MaxHeight = 40;
            panel.Children.Add(labelBox);
            Viewbox box = new Viewbox();
            box.MaxHeight = 60;
            CheckBox chbox = DynamicElements.CreateCheckBox(answer.Rightness);
            chbox.Name = "answer" + answerCount;
            chbox.ToolTip = "Правильность";
            box.Child = chbox;
            TextBox txtBox = new TextBox();
            txtBox.MinWidth = 150;
            txtBox.MaxWidth = 150;
            txtBox.Text = answer.Text;
            chbox.Content = txtBox;
            panel.Children.Add(box);
            panel.Children.Remove(sender as Button);
            panel.Children.Add(sender as Button);
            answerCount++;
        }

        void ClearGrid(){
            grid_QuestData.Children.Clear();
            grid_QuestData.RowDefinitions.Clear();
        }

        void AddRow(Grid grid, int count) {
            for (int i = 0; i < count; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(row);
            }
        }


        public int GetIndex(object sender)
        {
            return Convert.ToInt32((sender as Border).Tag);
        }

        public QuestionsModel GetQuestion(out int id) {
            QuestionsModel question = null;
            bool emptyAnswer = false;
            RichTextBox rtb = LogicalTreeHelper.FindLogicalNode(this, "rtb_Question") as RichTextBox;
            string content = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
            id = Convert.ToInt32(rtb.Tag);
            question = new QuestionsModel(id, 0, content);//, null
            for (int i = 0; i < answerCount; i++) {
                AnswersModel answer = null;
                CheckBox chbox = LogicalTreeHelper.FindLogicalNode(this, "answer" + i) as CheckBox;
                bool? right = chbox.IsChecked;
                string text = (chbox.Content as TextBox).Text.ToString();
                if (string.IsNullOrEmpty(text)) emptyAnswer = true;
                answer = new AnswersModel(i, id, text, Convert.ToBoolean(right)); //null
                question.Answers.Add(answer);
            }
            if (string.IsNullOrEmpty(content) || content=="\r\n" || emptyAnswer)
            {
                MessageBox.Show("Вопрос и ответы не должны быть пустыми.");
                sp_Questions.IsEnabled = false;
                btn_Complete.IsEnabled = false;
                return null;
            }
            else return question;
        }

        public void DeleteQuestion(int number, int questCount) {
            sp_Questions.Children.Remove(LogicalTreeHelper.FindLogicalNode(sp_Questions, "border" + (number*-1)) as UIElement);
            for (int i = questCount+1; i > (number * -1); i--)
            {
                Border bord = LogicalTreeHelper.FindLogicalNode(sp_Questions, "border" + i) as Border;
                bord.Name = "border" + (i -1);
                bord.Tag = (i*-1 + 1);
                ((bord.Child as Viewbox).Child as Label).Content = (i - 1);
            }
            ClearGrid();
        }

        void ChangeEnableSaveDelete(bool isEnable) {
            btn_Delete.IsEnabled = isEnable;
            btn_Save.IsEnabled = isEnable;
        }

        public void CreateCompleteContent(string[] testTypes, string[] testDifficult)
        {
            mainGrid.Children.Clear();
            mainGrid.ColumnDefinitions.Clear();

            TextBox tb_name = new TextBox();
            CreateCompleteBlock(mainGrid, tb_name, "name", 0, "Название теста");

            TextBox tb_count = new TextBox();
            CreateCompleteBlock(mainGrid, tb_count, "count", 2, "Количество отображаемых вопросов");

            ComboBox cb_type = new ComboBox();
            cb_type.ItemsSource = testTypes;
            cb_type.SelectedIndex = 0;
            cb_type.IsEditable = true;
            CreateCompleteBlock(mainGrid, cb_type, "type", 4, "Выберите или введите тип теста");

            ComboBox cb_difficult = new ComboBox();
            cb_difficult.ItemsSource = testDifficult;
            cb_difficult.SelectedIndex = 0;
            CreateCompleteBlock(mainGrid, cb_difficult, "difficult", 6, "Сложность теста");

            Button btn_end = new Button();
            btn_end.Content = "Завершить";
            btn_end.Style = this.FindResource("btn_End") as Style;
            CreateCompleteBlock(mainGrid, btn_end, "end", 8, "");
        }

        void CreateCompleteBlock(Grid grid, FrameworkElement result, string resultName, int column, string content) {
            Viewbox label = DynamicElements.CreateViewBoxLabel(content, 0);
            DynamicElements.SetRowColumnProperties(label, column, 0, 1, 1);
            grid.Children.Add(label);
            Viewbox container = DynamicElements.CreateViewBox(column+1, 0, 1, 1);
            result.Name = resultName;
            result.MinWidth = 200;
            result.MaxWidth = 200;
            container.Child = result;
            grid.Children.Add(container);
        }

        public bool CheckCompleteFill() {
            bool filled = true;
            int val;
            if(string.IsNullOrWhiteSpace((LogicalTreeHelper.FindLogicalNode(this, "name") as TextBox).Text) ||
                !Int32.TryParse((LogicalTreeHelper.FindLogicalNode(this, "count") as TextBox).Text,out val) ||
                string.IsNullOrWhiteSpace((LogicalTreeHelper.FindLogicalNode(this, "type") as ComboBox).Text))
            {
                filled = false;
            }
            if (!filled) MessageBox.Show("Все поля должны быть заполнены");
            return filled;
        }

        public void GetCompleteValues(out string name, out int count, out string type, out string difficult) {
            name = (LogicalTreeHelper.FindLogicalNode(this, "name") as TextBox).Text;
            count = Convert.ToInt32((LogicalTreeHelper.FindLogicalNode(this, "count") as TextBox).Text);
            type = (LogicalTreeHelper.FindLogicalNode(this, "type") as ComboBox).Text;
            difficult = (LogicalTreeHelper.FindLogicalNode(this, "difficult") as ComboBox).Text;
        }

        public void SendErrorMessage(string text) {
            MessageBox.Show(text);
        }
    }
}
