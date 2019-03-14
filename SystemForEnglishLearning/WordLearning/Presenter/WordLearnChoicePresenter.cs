using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SystemForEnglishLearning.WordLearning.Dictionary;
using SystemForEnglishLearning.WordLearning.Exercises;

namespace SystemForEnglishLearning.WordLearning
{
    enum MinWordsCount {
        translate = 25,
        equivalent = 25,
        constructor = 5,
        synonym = 5,
        listening = 5
    }

    class WordLearnChoicePresenter
    {
        IWordLearningChoiceView win = null;
        WordLearnChoiceModel model = null;
        BorderPresenter border;
        int userId;

        public WordLearnChoicePresenter(IWordLearningChoiceView ChoiceWindow, int userId)
        {
            model = new WordLearnChoiceModel(userId);
            win = ChoiceWindow;
            border = new BorderPresenter(win);
            win.Border_MouseLeftButtonDown += new EventHandler(BorderMouseLeftClick);
            win.Grid_MouseRightButtonDown += new EventHandler(GridMouseRightClick);
            this.userId = userId;
        }

        //вибір пункту меню, звертання до моделі, повернення кількості слів, виведення відповідного повідомлення
        void BorderMouseLeftClick(object sender, EventArgs e)
        {
            Window window = win as Window;
            FrameworkElement bord = sender as FrameworkElement;
            int minWordCount = 0;
            int realWordCount = 0;
            switch (bord.Name) {
                case ("wordsBorder"): {
                    Groups groups = new Groups(userId, window.Left, window.Top);
                    groups.WindowState = window.WindowState;
                    groups.Show();
                    window.Close();
                    break; }
                case ("translateVariantBorder"): {
                    minWordCount = (int)Enum.Parse(typeof(MinWordsCount), "translate");
                    realWordCount = model.GetCount("translate");
                    if (realWordCount < minWordCount)
                    {
                        win.SendMessage("Недостаточно слов для упражнения, сейчас у вас " + realWordCount + " слов, а нужно " + minWordCount + " слов.");
                    }
                    else {
                        Equivalent equivalent = new Equivalent(userId, "translate", window.Left, window.Top);
                        equivalent.WindowState = window.WindowState;
                        equivalent.ShowDialog();
                        //win.Close();
                    }
                    //use try catch because window may automatically close if words count < that must be
                    //try
                    //{
                        //equivalent.Show();
                        //win.Close();
                    //}
                    //catch { }
                    break; }
                case ("englishVariantBorder"): {
                    minWordCount = (int)Enum.Parse(typeof(MinWordsCount), "equivalent");
                    realWordCount = model.GetCount("equivalent");
                    if (realWordCount < minWordCount)
                    {
                        win.SendMessage("Недостаточно слов для упражнения, сейчас у вас " + realWordCount + " слов, а нужно " + minWordCount + " слов.");
                    }
                    else
                    {
                        Equivalent equivalent = new Equivalent(userId, "equivalent", window.Left, window.Top);
                        //equivalent.SourceInitialized += (s, a) => equivalent.WindowState = System.Windows.WindowState.Maximized;
                        equivalent.WindowState = window.WindowState;
                        equivalent.ShowDialog();
                        //win.Close();
                    }
                    break;
                }
                case ("listeningBorder"): {
                    minWordCount = (int)Enum.Parse(typeof(MinWordsCount), "listening");
                    realWordCount = model.GetCount("listening");
                    if (realWordCount < minWordCount)
                    {
                        win.SendMessage("Недостаточно слов для упражнения, сейчас у вас " + realWordCount + " слов, а нужно " + minWordCount + " слов.");
                    }
                    else
                    {
                        Equivalent equivalent = new Equivalent(userId, "listening", window.Left, window.Top);
                        equivalent.WindowState = window.WindowState;
                        equivalent.ShowDialog();
                    }
                    break;
                }
                case ("constructorBorder"): {
                    minWordCount = (int)Enum.Parse(typeof(MinWordsCount), "constructor");
                    realWordCount = model.GetCount("constructor");
                    if (realWordCount < minWordCount)
                    {
                        win.SendMessage("Недостаточно слов для упражнения, сейчас у вас " + realWordCount + " слов, а нужно " + minWordCount + " слов.");
                    }
                    else {
                        Equivalent equivalent = new Equivalent(userId, "constructor", window.Left, window.Top);
                        equivalent.WindowState = window.WindowState;
                        equivalent.ShowDialog();
                    }
                    break;
                }
                case ("synonymBorder"): {
                        Equivalent equivalent = new Equivalent(userId, "synonym", window.Left, window.Top);
                        try
                        {
                            equivalent.WindowState = window.WindowState;
                            equivalent.ShowDialog();
                        }
                        catch { }
                    break;
                }
                case ("repeatBorder"): { break; }
                default: { break; }
            }
        }

        void GridMouseRightClick(object sender, EventArgs e) {
            Window window = win as Window;
            MainChoice main = new MainChoice(userId, window.Left, window.Top);
            main.WindowState = window.WindowState;
            main.Show();
            window.Close();
        }
    }
}
