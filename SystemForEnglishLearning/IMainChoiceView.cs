using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning
{
    interface IMainChoiceView : IBorderView
    {
        event EventHandler Border_MouseLeftButtonDown;
    }
}
