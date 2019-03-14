using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemForEnglishLearning.Tests
{
    class TestChoicePresenter
    {
        ITestChoiceView window = null;
        TestChoiceModel model = null;
        List<TestsModel> lections;
        int userId;

        public TestChoicePresenter(ITestChoiceView win, int userId) {
            model = new TestChoiceModel(userId);
            window = win;
            if (model.Tests.Count == 0) {
                window.SendMessage("Тесты отсутствуют");
                (win as Window).Close();
            }
            window.Item_DoubleClick += window_Item_DoubleClick;
            window.CreateTest_Click += window_CreateTest_Click;
            window.Delete_Click += window_Delete_Click;
            win.SetData(CreateTreeData(model.Tests), false);
            win.SetData(CreateTreeData(model.UserTests), true);
            this.userId = userId;
            lections = new List<TestsModel>();
        }

        void window_Delete_Click(object sender, EventArgs e)
        {
            int id = window.GetIdToDelete(sender);
            model.DeleteTest(id);
            (window as Window).Close();
        }

        void window_CreateTest_Click(object sender, EventArgs e)
        {
            Window win = window as Window;
            CreateTest create = new CreateTest(userId, win.Left, win.Top, win.WindowState);
            create.Show();
            win.Close();
        }

        //перехід до вибраного тесту
        void window_Item_DoubleClick(object sender, EventArgs e)
        {
            TestsModel mod = window.GetChoosenModel(sender) as TestsModel;
            if (mod != null)
            {
                Window win = window as Window;
                Test testWindow = new Test(userId, mod, win.Left, win.Top, win.WindowState);
                testWindow.ShowDialog();
            }
        }

        //створення даних для відображення в treeview
        List<TestGroupModel> CreateTreeData(List<TestsModel> data)
        {
            List<TestGroupModel> root = new List<TestGroupModel>();
            data.Sort((w1, w2) => w1.Type.CompareTo(w2.Type));
            string type = "";
            foreach (TestsModel el in data)
            {
                if (el.Type != type)
                {
                    type = el.Type;
                    root.Add(new TestGroupModel());
                    root[root.Count - 1].Type = type;
                }
                root[root.Count - 1].Items.Add(el);
            }
            return root;
        }

    }
}
