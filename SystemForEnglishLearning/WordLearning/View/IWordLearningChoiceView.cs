using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning
{
    interface IWordLearningChoiceView : IBorderView
    {
        //вибір пункту
        event EventHandler Border_MouseLeftButtonDown;
        //повернення на попереднє вікно
        event EventHandler Grid_MouseRightButtonDown;
        void SendMessage(string message);
    }
}
