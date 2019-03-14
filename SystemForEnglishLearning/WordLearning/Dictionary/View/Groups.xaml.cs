using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    /// <summary>
    /// Interaction logic for Groups.xaml
    /// </summary>
    public partial class Groups : Window, IGroupsView
    {
        public Groups()
        {
            InitializeComponent();
        }

        Groups(double left, double top): this() {
            this.Left = left;
            this.Top = top;
        }

        public Groups(int userId, double left, double top)
            : this(left,top)
        {
            new GroupsPresenter(this, userId);
        }

        public Groups(int userId)
        {
            new GroupsPresenter(this, userId);
        }

        public event EventHandler Border_MouseEnter = null;
        private void Border_MouseEnter_1(object sender, MouseEventArgs e)
        {
            //Border_MouseEnter = delegate{};
            Border_MouseEnter(sender, e);
        }

        public event EventHandler Border_MouseLeave = null;
        private void Border_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Border_MouseLeave(sender, e);
        }

        public event EventHandler Border_MouseLeftButtonDown = null;
        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Border_MouseLeftButtonDown(sender, e);
        }

        public event EventHandler Grid_MouseRightButtonDown = null;
        private void Grid_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Grid_MouseRightButtonDown(sender, e);
        }

        void SetContent(UIElement content) {
            this.StandardPanel.Children.Add(content);
        }

        public void GenerateContent(List<GroupModel> data) {
            foreach (GroupModel el in data)
            {
                Border bord = new Border();
                bord.Style = this.FindResource("MainBorderStyle") as Style;
                bord.Name = "Border" + el.Group;
                bord.Tag = el.Group;
                Grid grid = DynamicElements.CreateGrid(2, 5, GridUnitType.Auto, GridUnitType.Auto);
                grid.MinWidth = 188;
                Viewbox vB = DynamicElements.CreateViewBox(0, 0, 2, 1);
                Label lb = DynamicElements.CreateLabel(el.Name, 16);
                vB.Child = lb;
                grid.Children.Add(vB);
                string[,] labelVal = new string[,] { { "Слов:", el.WordsCount.ToString() }, { "Сложность:", el.Difficult } };
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        vB = DynamicElements.CreateViewBox(i + 1, j, 1, 1);
                        lb = DynamicElements.CreateLabel(labelVal[i, j], 12);
                        vB.Child = lb;
                        grid.Children.Add(vB);
                    }
                }
                Image img = DynamicElements.CreateImage(el.Image, 3, 0, 2, 1, 100);
                grid.Children.Add(img);
                Border border = DynamicElements.CreateBorder(this.FindResource("LearnBorderStyle") as Style, 4, 0, 2, 1);
                border.Margin = new Thickness(3);
                border.Child = DynamicElements.CreateViewBoxLabel("Изучить", 0);
                grid.Children.Add(border);
                //Button btn = new Button();
                //btn.Style = this.FindResource("ButtonStyle") as Style;
                //DynamicElements.SetRowColumnProperties(btn, 4, 0, 2, 1);
                //grid.Children.Add(btn);
                bord.Child = grid;
                SetContent(bord);
            }
        }

    }
}
