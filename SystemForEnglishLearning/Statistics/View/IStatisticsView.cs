using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Statistics
{
    interface IStatisticsView
    {
        //натиснення на історію тестів
        event EventHandler History_Click;
        //вибір іншого типу графіку
        event EventHandler cb_Choiced;
        //встановлення даних графіку
        void SetChartData<T>(List<T> data, string title, string chartText);
        //встановлення даних з типом доступних графіків
        void SetComboBoxData(string[] data);
        void ErrorMessage(string message);
        //повернення вибраного типу
        string GetChosenText(object sender);
    }
}
