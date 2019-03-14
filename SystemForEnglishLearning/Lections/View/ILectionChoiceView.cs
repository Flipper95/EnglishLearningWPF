using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Lections
{
    interface ILectionChoiceView : IBorderView
    {
        //вибір певної лекції
        event EventHandler Item_DoubleClick;
        //встановлення переліку лекцій
        void SetData<T>(List<T> data);
        //отримання обраної моделі
        object GetChoosenModel(object sender);
        //отримання групи лекцій (щоб відразу можна було пройти схожі за типом лекції)
        object GetGroupModel(object sender);
        void SendMessage(string message);
    }
}
