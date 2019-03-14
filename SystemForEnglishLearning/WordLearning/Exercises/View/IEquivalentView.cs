using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    interface IEquivalentView : IBorderView
    {
        event EventHandler Image_MouseLeftButtonDown;
        //закінчення програвання озвучування
        event EventHandler MediaEnded;
        //натиснення на одну з відповідей
        event EventHandler Variant_MouseLeftButtonDown;
        //вибір наступного блоку
        event EventHandler Next_MouseLeftButtonDown;
        //закінчення вправи та оновлення балів
        event EventHandler Complete_MouseLeftButtonDown;
        event EventHandler Window_Closing;
        void SendMessage(string message);
    }
}
