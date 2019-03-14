using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    interface IGroupsView : IBorderView
    {
        //вибір певної групи
        event EventHandler Border_MouseLeftButtonDown;
        event EventHandler Grid_MouseRightButtonDown;
        //void SetContent(UIElement content);
        //заповнення контенту
        void GenerateContent(List<GroupModel> data);
    }
}
