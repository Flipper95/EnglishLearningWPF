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
    class EquivalentPresenter : ExercisePresenter
    {
        Random rand;
        TranslateModel model;

        public EquivalentPresenter(IEquivalentView window, int userId) : base(window,userId)
        {
            rand = new Random();
            model = new TranslateModel(userId,"equivalent");
            win.Variant_MouseLeftButtonDown += new EventHandler(VariantMouseLeftDown);
            GenerateContent();
        }

        protected void VariantMouseLeftDown(object sender, EventArgs e) {
            WordModel choosen = GetChoosenWordModel((sender as Border).Tag);
            VariantClick(sender, choosen.Translate, answer.Translate, choosen.WordId);
            win.Variant_MouseLeftButtonDown -= new EventHandler(VariantMouseLeftDown);
        }

        protected override void ClearGrid()
        {
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
            if (model.Words.Count != 0)
            {
                //int index = rand.Next(offset, repeatNumber * 5);
                int index = rand.Next(0, 5);
                answer = model.Words[index];
                Viewbox vB = DynamicElements.CreateViewBoxLabel(answer.Translate, answer.WordId);
                DynamicElements.SetRowColumnProperties(vB, 1, 0, 1, 2);
                vB.Name = "Answer";
                Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(win as Window, "DataGrid");
                grid.Children.Add(vB);
                AddContent();
                AddNextComplete(model.Words.Count);
            }
        }

        void AddContent()
        {
            Window window = win as Window;
            Style bordStyle = window.FindResource("VariantStyle") as Style;
            for (int i = 0; i < 5; i++)
            {
                //int ind = rand.Next(offset, repeatNumber * 5 - i); //TO DO:check index maybe need change to repeatNumber*5-i-1
                int ind = rand.Next(0, 5 - i);
                string content = model.Words[ind].Word;
                Border bord = DynamicElements.CreateBorder(bordStyle, model.Words[ind]);//model.Words[ind].WordId);
                bord.Margin = new Thickness(0, 4, 4, 4);
                bord.Name = "Bord" + i;
                bord.Child = DynamicElements.CreateViewBoxLabel(content, model.Words[ind].WordId);
                DynamicElements.SetRowColumnProperties(bord, i + 1, 1, 1, 1);
                bord.Background = window.FindResource("BrushYellow") as Brush;
                Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
                grid.Children.Add(bord);
                model.Words.RemoveAt(ind);
                //bord = CreateOwnBorder(bordStyle, rand.Next(0, repeatNumber * 5);
            }
        }


    }
}
