using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SystemForEnglishLearning
{
    class ChoicePresenter
    {
        IMainChoiceView win = null;
        BorderPresenter border = null;
        //EnterRegisterModel model = null;
        int userId;

        public ChoicePresenter(IMainChoiceView ChoiceWindow, int userId){
            win = ChoiceWindow;
            border = new BorderPresenter(win);
            win.Border_MouseLeftButtonDown += new EventHandler(BorderMouseLeftClick);
            this.userId = userId;
        }

        //вибір відповідного пункту меню, відкривається відповідне вікно
        void BorderMouseLeftClick(object sender, EventArgs e)
        {
            FrameworkElement bord = sender as FrameworkElement;
            Window window = win as Window;
            switch (bord.Name) {
                case ("lectionBorder"): {
                    Lections.LectionChoice choice = new Lections.LectionChoice(userId, window.Left, window.Top);
                    choice.WindowState = window.WindowState;
                    choice.ShowDialog();
                    break;
                }
                case ("testBorder"): {
                    Tests.TestChoice choice = new Tests.TestChoice(userId, window.Left, window.Top);
                    choice.WindowState = window.WindowState;
                    choice.ShowDialog();
                    break;
                }
                case ("dictionaryBorder"): {
                    WordLearning.WordLearningChoice choice = new WordLearning.WordLearningChoice(userId, window.Left, window.Top);
                    choice.WindowState = window.WindowState;
                    choice.Show();
                    window.Close();
                    break; 
                }
                case ("statisticBorder"): {
                    Statistics.Statistics choice = new Statistics.Statistics(userId, window.Left, window.Top);
                    choice.WindowState = window.WindowState;
                    choice.ShowDialog();
                    break;
                }
            }
        }
    }
}
