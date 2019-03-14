using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
//using System.Windows.Media;
//using System.Threading;
//using System.Windows.Threading;
//using VoiceRSS_SDK;
//using System.Windows;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    class TranslatePresenter : ExercisePresenter
    {
        Random rand;
        TranslateModel model;
        //TODO: better change all number to variables, for example change 5 to variable answer number

        public TranslatePresenter(IEquivalentView window, int userId) : base(window,userId)
        {
            rand = new Random();
            model = new TranslateModel(userId,"translate");
            win.Variant_MouseLeftButtonDown += new EventHandler(VariantMouseLeftDown);
            GenerateContent();
        }

        protected void VariantMouseLeftDown(object sender, EventArgs e)
        {
            WordModel choosen = GetChoosenWordModel((sender as Border).Tag);
            VariantClick(sender, choosen.Translate, answer.Translate, answer.WordId);
            win.Variant_MouseLeftButtonDown -= new EventHandler(VariantMouseLeftDown);
        }

        protected override void ClearGrid()
        {
                MediaElement mediaEl = (MediaElement)LogicalTreeHelper.FindLogicalNode(win as Window, "MediaEl");
                mediaEl.Source = null;
                Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(win as Window, "DataGrid");
                grid.Children.Clear();
        }

        protected override void NextMouseLeftDown(object sender, EventArgs e)
        {
            ClearGrid();
            win.Variant_MouseLeftButtonDown += new EventHandler(VariantMouseLeftDown);
            GenerateContent();
        }

        protected override void CompleteMouseLeftDown(object sender, EventArgs e)
        {
            model.UpdateScore(rightAnswer);
            win.SendMessage("Ваш результат: " + (rightAnswer.Count) + " из 5.");
            //MessageBox.Show("Ваш результат: " + (rightAnswer.Count) + " из 5.");
            (win as Window).Close();
        }

        void GenerateContent()
        {
            Window window = win as Window;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            if (model.Words.Count != 0)
            {
                //int index = rand.Next(offset, repeatNumber * 5);
                int index = rand.Next(0, 5);
                answer = model.Words[index];
                Viewbox vB = DynamicElements.CreateViewBoxLabel(answer.Word, answer.WordId);
                DynamicElements.SetRowColumnProperties(vB, 1, 0, 1, 2);
                vB.Name = "Answer";
                grid.Children.Add(vB);
                AddTVPS();
                AddContent();
                AddNextComplete(model.Words.Count);
            }
        }

        void AddContent()
         {
             Window window = win as Window;
             Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            Style bordStyle = window.FindResource("VariantStyle") as Style;
            for (int i = 0; i < 5; i++)
            {
                //int ind = rand.Next(offset, repeatNumber * 5 - i); //TO DO:check index maybe need change to repeatNumber*5-i-1
                int ind = rand.Next(0, 5 - i);
                string content = model.Words[ind].Translate;
                Border bord = DynamicElements.CreateBorder(bordStyle, model.Words[ind]);//model.Words[ind].WordId);
                bord.Margin = new Thickness(0, 4, 4, 4);
                bord.Name = "Bord" + i;
                bord.Child = DynamicElements.CreateViewBoxLabel(content, model.Words[ind].WordId);
                DynamicElements.SetRowColumnProperties(bord, i + 1, 1, 1, 1);
                bord.Background = window.FindResource("BrushYellow") as Brush;
                grid.Children.Add(bord);
                model.Words.RemoveAt(ind);
                //bord = CreateOwnBorder(bordStyle, rand.Next(0, repeatNumber * 5);
            }
        }

    }
}
