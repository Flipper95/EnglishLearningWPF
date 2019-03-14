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
    class ListeningPresenter : ExercisePresenter
    {
        Random rand;
        TranslateModel model;
        bool flag;

        public ListeningPresenter(IEquivalentView window, int userId) : base(window,userId)
        {
            rand = new Random();
            model = new TranslateModel(userId,"listening");
            flag = false;
            GenerateContent();
        }

        protected override void ClearGrid()
        {
            Window window = win as Window;
            MediaElement mediaEl = (MediaElement)LogicalTreeHelper.FindLogicalNode(window, "MediaEl");
            mediaEl.Source = null;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            grid.Children.Clear();
        }

        protected override void NextMouseLeftDown(object sender, EventArgs e)
        {
            Window window = win as Window;
            if (!flag)
            {
                TextBox tB = (TextBox)LogicalTreeHelper.FindLogicalNode(window, "Answer");
                if (tB.Text.ToLower() == answer.Word.ToLower())
                {
                    rightAnswer.Add(answer.WordId);
                    tB.Background = window.FindResource("BrushGreen") as Brush;
                }
                else {
                    tB.Background = window.FindResource("BrushRed") as Brush;
                }
                tB.IsEnabled = false;
                AddRightAnswer();
                (((sender as Border).Child as Viewbox).Child as Label).Content = "Следующее";
                flag = true;
            }
            else
            {
                ClearGrid();
                //win.Variant_MouseLeftButtonDown += new EventHandler(VariantMouseLeftDown);
                GenerateContent();
                flag = false;
            }
        }

        protected override void CompleteMouseLeftDown(object sender, EventArgs e)
        {
            Window window = win as Window;
            if (!flag)
            {
                TextBox tB = (TextBox)LogicalTreeHelper.FindLogicalNode(window, "Answer");
                if (tB.Text.ToLower() == answer.Word)
                {
                    rightAnswer.Add(answer.WordId);
                    tB.Background = window.FindResource("BrushGreen") as Brush;
                }
                tB.IsEnabled = false;
                flag = true;
                AddRightAnswer();
                (((sender as Border).Child as Viewbox).Child as Label).Content = "Завершить";
            }
            else
            {
                model.UpdateScore(rightAnswer);
                win.SendMessage("Ваш результат: " + (rightAnswer.Count) + " из 5.");
                //MessageBox.Show("Ваш результат: " + (rightAnswer.Count) + " из 5.");
                flag = false;
                window.Close();
            }
        }

        //protected override void VariantMouseLeftDown(object sender, EventArgs e)
        //{
        //}

        void GenerateContent()
        {
            if (model.Words.Count != 0)
            {
            //    //int index = rand.Next(offset, repeatNumber * 5);
                Window window = win as Window;
                Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
                int index = rand.Next(0, model.Words.Count);
                answer = model.Words[index];
                AddVoice();
                Border mediaBord = (Border)LogicalTreeHelper.FindLogicalNode(window, "Speech");
                DynamicElements.SetRowColumnProperties(mediaBord, 1, 0, 2, 2);
                Viewbox vB = new Viewbox();
                DynamicElements.SetRowColumnProperties(vB, 4, 0, 2, 1);
                TextBox tB = new TextBox();
                tB.MinWidth = 100;
                vB.Child = tB;
                tB.Name = "Answer";
                tB.MinWidth = 100;
                tB.MaxWidth = 100;
                grid.Children.Add(vB);
                model.Words.RemoveAt(index);
                AddNextComplete(model.Words.Count);
                if (model.Words.Count != 0)
                {
                    ((Border)LogicalTreeHelper.FindLogicalNode(window, "NextBord")).Visibility = Visibility.Visible;
                }
                else {
                    ((Border)LogicalTreeHelper.FindLogicalNode(window, "CompleteBord")).Visibility = Visibility.Visible;
                }
            }
        }

        void AddRightAnswer() {
            Window window = win as Window;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            Border bord = DynamicElements.CreateBorder(window.FindResource("BorderStyle") as Style, answer.WordId);
            bord.Margin = new Thickness(0, 4, 4, 4);
            bord.Background = window.FindResource("BrushYellow") as Brush;
            Viewbox vB = DynamicElements.CreateViewBoxLabel(answer.Word +" - " + answer.Translate, answer.WordId);
            DynamicElements.SetRowColumnProperties(bord, 5, 0, 2, 1);
            bord.Child = vB;
            grid.Children.Add(bord);
        }

    }
}
