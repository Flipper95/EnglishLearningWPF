using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning
{
    interface IBorderView
    {
        event EventHandler Border_MouseEnter;
        event EventHandler Border_MouseLeave;
    }
}
