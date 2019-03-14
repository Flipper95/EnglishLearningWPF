using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    interface ITestChoiceView
    {
        //вибір певного питання
        event EventHandler Item_DoubleClick;
        //створення власного тесту
        event EventHandler CreateTest_Click;
        event EventHandler Delete_Click;
        //встановлення дани (переліку тестів)
        void SetData<T>(List<T> data, bool userTree);
        //отримання обраної моделі
        object GetChoosenModel(object sender);
        //List<TestsModel> GetGroupModel(object sender);
        int GetIdToDelete(object sender);
        void SendMessage(string message);
    }
}
