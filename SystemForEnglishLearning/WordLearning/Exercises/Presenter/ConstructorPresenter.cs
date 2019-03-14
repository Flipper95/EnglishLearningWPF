using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    //TODO: refactor this class later, variable names etc
    class ConstructorPresenter
    {
        TranslateModel model = null;
        IEquivalentView window = null;
        BorderPresenter borderPresenter = null;
        List<int> rightAnswers;
        bool flag;
        WordModel answer;
        char[] userAnswer;
        Random rand;

        public ConstructorPresenter(IEquivalentView window, int userId)
        {
            this.window = window;
            borderPresenter = new BorderPresenter(window);
            rand = new Random();
            rightAnswers = new List<int>();
            flag = false;
            userAnswer = new char[1];
            model = new TranslateModel(userId, "constructor");
            window.Window_Closing += new EventHandler(WindowClosing);
            window.Next_MouseLeftButtonDown += new EventHandler(NextMouseLeftDown);
            window.Variant_MouseLeftButtonDown += new EventHandler(VariantMouseLeftDown);
            window.Complete_MouseLeftButtonDown += new EventHandler(CompleteMouseLeftDown);
            GenerateContent();
        }

        void WindowClosing(object sender, EventArgs e)
        {
            model = null;
            window.Window_Closing -= new EventHandler(WindowClosing);
        }

        void NextMouseLeftDown(object sender, EventArgs e)
        {
            Window win = window as Window;
            if (!flag)
            {
                AddAnswer();
                (((sender as Border).Child as Viewbox).Child as Label).Content = "Следующее";
                flag = true;
                DisableGrids();
            }
            else
            {
                (LogicalTreeHelper.FindLogicalNode(window as Window, "DataGrid") as Grid).Children.Clear();
                GenerateContent();
                flag = false;
            }
        }

        void CompleteMouseLeftDown(object sender, EventArgs e)
        {
            Window win = window as Window;
            if (!flag)
            {
                AddAnswer();
                (((sender as Border).Child as Viewbox).Child as Label).Content = "Завершить";
                flag = true;
                DisableGrids();
            }
            else
            {
                model.UpdateScore(rightAnswers);
                window.SendMessage("Ваш результат: " + (rightAnswers.Count) + " из 5.");
                //MessageBox.Show("Ваш результат: " + (rightAnswers.Count) + " из 5.");
                flag = false;
                win.Close();
            }
        }

        void AddAnswer() {
            Window win = window as Window;
            AddRightAnswer();
            Border box = (Border)LogicalTreeHelper.FindLogicalNode(window as Window, "Answer");
            if (new string(userAnswer) == answer.Word)
            {
                rightAnswers.Add(answer.WordId);
                box.Background = win.FindResource("BrushGreen") as Brush;
            }
            else
            {
                box.Background = win.FindResource("BrushRed") as Brush;
            }
        }

        void DisableGrids() {
            Grid target = LogicalTreeHelper.FindLogicalNode(window as Window, "TargetGrid") as Grid;
            target.IsEnabled = false;
            target = LogicalTreeHelper.FindLogicalNode(window as Window, "QuestGrid") as Grid;
            target.IsEnabled = false;
        }

        void AddRightAnswer()
        {
            Window win = window as Window;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(win, "DataGrid");
            Border bord = DynamicElements.CreateBorder(win.FindResource("BorderStyle") as Style, 5, 0, 2, 1);
            bord.Tag = answer.WordId;
            bord.Background = win.FindResource("BrushYellow") as Brush;
            Viewbox vb = DynamicElements.CreateViewBoxLabel(answer.Word + " - " + answer.Translate, answer.WordId);
            bord.Name = "Answer";
            bord.Child = vb;
            grid.Children.Add(bord);
        }

        void VariantMouseLeftDown(object sender, EventArgs e)
        {
            FrameworkElement border = sender as FrameworkElement;
            if ((border.Parent as Grid).Name == "QuestGrid")
            {
                int index = ChangeBordGrid(border, "TargetGrid");
                userAnswer[index] = Convert.ToChar((((border as Border).Child as Viewbox).Child as Label).Content);
            }
            else
            {
                int index = Grid.GetColumn(border);
                userAnswer[index] = ' ';
                ChangeBordGrid(border, "QuestGrid");
            }
        }

        int ChangeBordGrid(FrameworkElement sender, string gridName) {
            Grid target = LogicalTreeHelper.FindLogicalNode(window as Window, gridName) as Grid;
            (sender.Parent as Grid).Children.Remove(sender);
            int index = FindIndex(target);
            Grid.SetColumn(sender, index);
            target.Children.Add(sender);
            return index;
        }

        int FindIndex(Grid grid)
        {
            int result = -1;
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var element = grid.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetColumn(e) == i && Grid.GetRow(e) == 0);
                if (element == null)
                {
                    result = i;
                    return result;
                }
            }
            return 0;
        }

        void GenerateContent()
        {
            if (model.Words.Count != 0)
            {
                Window win = window as Window;
                Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(win, "DataGrid");
                int index = rand.Next(0, model.Words.Count);
                answer = model.Words[index];
                userAnswer = new char[answer.Word.Length];
                Viewbox translate = DynamicElements.CreateViewBoxLabel(answer.Translate, answer.WordId);
                DynamicElements.SetRowColumnProperties(translate, 1, 0, 2, 1);
                grid.Children.Add(translate);
                Grid targetGrid = DynamicElements.CreateGrid(answer.Word.Length, 1, GridUnitType.Star, GridUnitType.Star);
                targetGrid.Name = "TargetGrid";
                DynamicElements.SetRowColumnProperties(targetGrid, 2, 0, 2, 1);
                grid.Children.Add(targetGrid);
                Grid questGrid = DynamicElements.CreateGrid(answer.Word.Length, 1, GridUnitType.Star, GridUnitType.Star);
                questGrid.Name = "QuestGrid";
                DynamicElements.SetRowColumnProperties(questGrid, 4, 0, 2, 1);
                grid.Children.Add(questGrid);
                FillGrid(Shuffle(answer.Word), questGrid);
                model.Words.RemoveAt(index);
                AddNextComplete(model.Words.Count);
                if (model.Words.Count != 0)
                {
                    ((Border)LogicalTreeHelper.FindLogicalNode(win, "NextBord")).Visibility = Visibility.Visible;
                }
                else
                {
                    ((Border)LogicalTreeHelper.FindLogicalNode(win, "CompleteBord")).Visibility = Visibility.Visible;
                }
            }
        }

        char[] Shuffle(string word) {
            Random rand = new Random();
            char[] result = word.ToCharArray();
            for (int i = 0; i < word.Length; i++)
            {
                int index = rand.Next(i, word.Length);
                char temp = result[i];
                result[i] = result[index];
                result[index] = temp;
            }
            return result;
        }

        void FillGrid(char[] word, Grid grid){
            for (int i = 0; i < word.Length;i++ )
            {
                Border bord = DynamicElements.CreateBorder((window as Window).FindResource("VariantStyle") as Style, 0, i, 1, 1);
                bord.Background = (window as Window).FindResource("BrushYellow") as Brush;
                bord.Margin = new Thickness(3);
                Viewbox vb = DynamicElements.CreateViewBoxLabel(word[i] + "", answer.WordId);
                bord.Child = vb;
                grid.Children.Add(bord);
            }
        }

        protected void AddNextComplete(int WordsCount)
        {
            Border nextBord;
            Style bordStyle;
            Window win = window as Window;
            if (WordsCount != 0)
            {
                bordStyle = win.FindResource("NextStyle") as Style;
                nextBord = DynamicElements.CreateBorder(bordStyle);
                nextBord.Child = DynamicElements.CreateViewBoxLabel("Дальше", 0);
                nextBord.Name = "NextBord";
            }
            else
            {
                bordStyle = win.FindResource("CompleteStyle") as Style;
                nextBord = DynamicElements.CreateBorder(bordStyle);
                nextBord.Child = DynamicElements.CreateViewBoxLabel("Готово", 0);
                nextBord.Name = "CompleteBord";
            }
            nextBord.Margin = new Thickness(0, 4, 4, 4);
            DynamicElements.SetRowColumnProperties(nextBord, 7, 1, 1, 1);
            nextBord.Visibility = Visibility.Collapsed;
            nextBord.Background = win.FindResource("BrushBlue") as Brush;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(win, "DataGrid");
            grid.Children.Add(nextBord);
        }

    }
}
