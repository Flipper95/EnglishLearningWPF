using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Statistics
{
    class StatisticsPresenter
    {
        IStatisticsView win = null;
        StatisticsModel model = null;
        int userId;

        public StatisticsPresenter(IStatisticsView window, int userId)
        {
            this.userId = userId;
            win = window;
            win.cb_Choiced += win_cb_Choiced;
            model = new StatisticsModel(userId);
            SetWordStatistics(DateTime.Now.AddDays(-7).Date, DateTime.Now.Date);
            string[] data = new string[] { "Выучено слов", "Пройдено тестов", "Добавлено слов" };
            win.SetComboBoxData(data);
            win.History_Click += win_History_Click;
        }

        void win_cb_Choiced(object sender, EventArgs e)
        {
            string text = win.GetChosenText(sender);
            DateTime start = DateTime.Now.AddDays(-7).Date; //TODO: user date
            DateTime end = DateTime.Now.Date;
            CheckAndSwapDate(ref start, ref end);
            switch (text)
            {
                case ("Выучено слов"):
                    {
                        SetWordStatistics(start, end);
                        break;
                    }

                case ("Пройдено тестов"):
                    {
                        SetTestStatistics(start, end);
                        break;
                    }

                case ("Добавлено слов"):
                    {
                        SetAddedStatistics(start, end);
                        break;
                    }
            }
        }

        void win_History_Click(object sender, EventArgs e)
        {
            System.Windows.Window window = win as System.Windows.Window;
            Tests.TestHistory history = new Tests.TestHistory(userId, window.Left, window.Top);
            history.WindowState = window.WindowState;
            history.Show();
            window.Close();
        }

        //встановлення легенди графіку та напису
        void SetWordStatistics(DateTime start, DateTime end)
        {
            List<DataModel> data = model.GetChosenCount("word",start, end);
            win.SetChartData(data, "Слов", "Количество выученых слов за 7 дней");
        }

        void SetAddedStatistics(DateTime start, DateTime end)
        {
            List<DataModel> data = model.GetChosenCount("addedWord", start, end);
            win.SetChartData(data, "Слов", "Количество добавленных слов за 7 дней");
        }

        void SetTestStatistics(DateTime start, DateTime end)
        {
            List<DataModel> data = model.GetChosenCount("test",start, end);
            win.SetChartData(data, "Тестов", "Количество пройденых тестов за 7 дней");
        }

        //зміна дати, щоб перший параметр був раніше за другий
        void CheckAndSwapDate(ref DateTime t1, ref DateTime t2)
        {
            if (DateTime.Compare(t1, t2) > 0)
            {
                DateTime temp = t1;
                t1 = t2;
                t2 = temp;
            }
        }

    }
}
