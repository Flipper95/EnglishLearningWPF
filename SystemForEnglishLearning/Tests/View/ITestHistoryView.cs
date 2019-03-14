using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    interface ITestHistoryView
    {
        //заповнення вікна данними
        void SetHistoryData<T>(List<T> data);
        //повернення вибраного рядку
        object GetSelectedRow(object sender);
        event EventHandler Row_DoubleClick;
    }
}
