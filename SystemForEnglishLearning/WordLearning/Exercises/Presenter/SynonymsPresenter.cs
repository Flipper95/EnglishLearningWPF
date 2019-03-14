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
    class SynonymsPresenter : ExercisePresenter
    {

        Random rand;
        SynonymsModel model;
        int realNum;
        //TODO: better change all number to variables, for example change 5 to variable answer number

        public SynonymsPresenter(IEquivalentView window, int userId)
            : base(window, userId)
        {
            rand = new Random();
            model = new SynonymsModel(userId);
            realNum = model.Synonyms.Count;
            if (realNum < 2)
            {
                window.SendMessage("Не найдено синонимов для изучаемых слов.");
                //MessageBox.Show("Не найдено синонимов для изучаемых слов.");
                (win as Window).Close();
                return;
            }
            win.Variant_MouseLeftButtonDown += new EventHandler(VariantMouseLeftDown);
            GenerateContent();
        }

        protected void VariantMouseLeftDown(object sender, EventArgs e)
        {
            WordModel choosen = GetChoosenWordModel((sender as Border).Tag);
            VariantClick(sender, choosen.WordId.ToString(), answer.WordId.ToString(), choosen.WordId);
            win.Variant_MouseLeftButtonDown -= new EventHandler(VariantMouseLeftDown);
        }

        protected override void ClearGrid()
        {
            Window window = win as Window;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            MediaElement mediaEl = (MediaElement)LogicalTreeHelper.FindLogicalNode(window, "MediaEl");
            if (mediaEl != null)
            {
                mediaEl.Source = null;
            }
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
            win.SendMessage("Ваш результат: " + (rightAnswer.Count) + " из " + realNum + ".");
            //MessageBox.Show("Ваш результат: " + (rightAnswer.Count) + " из " + realNum + ".");
            (win as Window).Close();
        }

        void GenerateContent()
        {
            if (model.Synonyms.Count != 0)
            {
                Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(win as Window, "DataGrid");
                int index = rand.Next(0, model.Synonyms.Count);
                answer = model.Synonyms[index]; //не явный upcast
                Viewbox vB = DynamicElements.CreateViewBoxLabel(answer.Word, answer.WordId);
                DynamicElements.SetRowColumnProperties(vB, 1, 0, 1, 2);
                vB.Name = "Answer";
                grid.Children.Add(vB);
                AddAnswerBorder(((SynonymWordModel)answer), index); //явный downcast
                AddTVPS();

                AddContent("synonyms", index);
                AddNextComplete(model.Synonyms.Count);
            }
        }

        void AddAnswerBorder(SynonymWordModel synonym, int index)
        {
            Window window = win as Window;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            Style bordStyle = window.FindResource("VariantStyle") as Style;
            string content = Parse(synonym.Synonyms);
            Border bord = DynamicElements.CreateBorder(bordStyle, synonym);//synonym.WordId);
            bord.Margin = new Thickness(0, 4, 4, 4);
            bord.Name = "Bord" + index;
            bord.Child = DynamicElements.CreateViewBoxLabel(content, synonym.WordId);
            DynamicElements.SetRowColumnProperties(bord, index + 1, 1, 1, 1);
            bord.Background = window.FindResource("BrushYellow") as Brush;
            grid.Children.Add(bord);
            model.Synonyms.RemoveAt(index);
        }

        protected void AddContent(string type, int index)
        {
            Window window = win as Window;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            Style bordStyle = window.FindResource("VariantStyle") as Style;
            Border answBorder = (Border)LogicalTreeHelper.FindLogicalNode(window, "Bord" + index);
            int filledInd = Grid.GetRow(answBorder);
            for (int i = 0; i < 5; i++)
            {
                if (i + 1 == filledInd) { }
                else
                {
                    int ind = rand.Next(0, 5 - i);
                    string content = model.Words[ind].Word;
                    Border bord = DynamicElements.CreateBorder(bordStyle, model.Words[ind]);//model.Words[ind].WordId);
                    bord.Margin = new Thickness(0, 4, 4, 4);
                    bord.Name = "Bord" + i;
                    bord.Child = DynamicElements.CreateViewBoxLabel(content, model.Words[ind].WordId);
                    DynamicElements.SetRowColumnProperties(bord, i + 1, 1, 1, 1);
                    bord.Background = window.FindResource("BrushYellow") as Brush;
                    grid.Children.Add(bord);
                    model.Words.RemoveAt(ind);
                }
            }
        }

        string Parse(string synonyms)
        {
            Char delimiter = ' ';
            String[] substrings = synonyms.Split(delimiter);
            int index = rand.Next(0, substrings.Count());
            return substrings[index];
        }

    }
}
