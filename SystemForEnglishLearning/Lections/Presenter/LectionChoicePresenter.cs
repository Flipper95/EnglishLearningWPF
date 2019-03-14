using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemForEnglishLearning.Lections
{
    class LectionChoicePresenter
    {
        ILectionChoiceView window = null;
        LectionsChoiceModel model = null;
        BorderPresenter border = null;
        int userId;

        public LectionChoicePresenter(ILectionChoiceView win, int userId) {
            model = new LectionsChoiceModel();
            window = win;
            border = new BorderPresenter(window);
            if (model.Lections.Count == 0) {
                window.SendMessage("Лекции отсутствуют");
                (win as Window).Close();
            }
            window.Item_DoubleClick += Item_DoubleClick;
            win.SetData(CreateTreeData(model.Lections));
            this.userId = userId;
        }

        //вибір певної лекції з набору
        void Item_DoubleClick(object sender, EventArgs e)
        {
            LectionsModel mod = window.GetChoosenModel(sender) as LectionsModel;
            if (mod != null)
            {
                Window win = window as Window;
                Lection lection = new Lection(userId, mod.Id, model.Lections, win.Left, win.Top);
                lection.WindowState = win.WindowState;
                lection.ShowDialog();
            }
            else {
                model.Lections = new List<LectionsModel>((window.GetGroupModel(sender) as LectionGroupModel).Items);
            }
        }

        List<LectionGroupModel> CreateTreeData(List<LectionsModel> data)
        {
            List<LectionGroupModel> root = new List<LectionGroupModel>();
            data.Sort((w1, w2) => w1.Type.CompareTo(w2.Type));
            string type = "";
            foreach (LectionsModel el in data)
            {
                if (el.Type != type)
                {
                    type = el.Type;
                    root.Add(new LectionGroupModel());
                    root[root.Count - 1].Type = type;
                }
                root[root.Count - 1].Items.Add(el);
            }
            return root;
        }

    }
}
