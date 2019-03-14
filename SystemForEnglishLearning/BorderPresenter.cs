using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SystemForEnglishLearning
{
    class BorderPresenter
    {
        IBorderView win = null;

        public BorderPresenter(IBorderView window) {
            win = window;
            win.Border_MouseEnter += new EventHandler(BorderMouseEnter);
            win.Border_MouseLeave += new EventHandler(BorderMouseLeave);
        }

        ~BorderPresenter() {
            win.Border_MouseEnter -= new EventHandler(BorderMouseEnter);
            win.Border_MouseLeave -= new EventHandler(BorderMouseLeave);
        }

        //оброблення події наведення мишки на рамку
        void BorderMouseEnter(object sender, EventArgs e)
        {
            Border bord = sender as Border;
            bord.BorderThickness = new System.Windows.Thickness(2);
        }

        //оброблення події виведення мишки за рамку компоненту
        void BorderMouseLeave(object sender, EventArgs e)
        {
            Border bord = sender as Border;
            bord.BorderThickness = new System.Windows.Thickness(0);
        }
    }
}
