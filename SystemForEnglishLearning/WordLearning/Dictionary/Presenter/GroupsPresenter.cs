using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    class GroupsPresenter
    {
        IGroupsView win = null;
        GroupsModel model = null;
        BorderPresenter border;
        int userId;

        public GroupsPresenter(IGroupsView GroupsWindow, int userId)
        {
            this.userId = userId;
            model = new GroupsModel();
            win = GroupsWindow;
            border = new BorderPresenter(win);
            win.Border_MouseLeftButtonDown += new EventHandler(BorderMouseLeftClick);
            win.Grid_MouseRightButtonDown += new EventHandler(GridMouseRightClick);
            //GenerateContent();
            List<GroupModel> constGroups = model.Groups;
            win.GenerateContent(constGroups);
        }

        //при виборі групи відкривається нове вікно, в яке передається ідентифікатор групи та користувача
        void BorderMouseLeftClick(object sender, EventArgs e)
        {
            FrameworkElement bord = sender as FrameworkElement;
            //Border bord = sender as Border;
            //bord.Tag
            GroupWords childWin = new GroupWords(userId, Convert.ToInt32(bord.Tag));
            childWin.WindowState = (win as Window).WindowState;
            childWin.ShowDialog();
        }

        //при натисненні на пкм відбувається перехід на попереднє вікно
        void GridMouseRightClick(object sender, EventArgs e)
        {
            Window window = win as Window;
            WordLearningChoice parentWin = new WordLearningChoice(userId, window.Left, window.Top);
            parentWin.WindowState = window.WindowState;
            parentWin.Show();
            window.Close();
        }

    }
}
