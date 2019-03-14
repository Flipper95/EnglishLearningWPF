using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class TestPresenter
    {
        ITestView window = null;
        TestsModel model = null;
        BorderPresenter border = null;
        int userId;
        int count;
        Dictionary<int, string> answers;

        public TestPresenter(ITestView win, int userId, TestsModel test) {
            window = win;
            model = test;
            this.userId = userId;
            border = new BorderPresenter(win);
            window.SetAllQuestions(model.Questions);
            count = 0;
            SetData();
            answers = new Dictionary<int, string>();
            window.Border_MouseLeftButtonDown += window_Border_MouseLeftButtonDown;
            window.NextBorder_MouseLeftButtonDown += window_NextBorder_MouseLeftButtonDown;
        }

        //натискання на наступне питання
        void window_NextBorder_MouseLeftButtonDown(object sender, EventArgs e)
        {
            int questId = window.GetIndex(sender);
            int qIndex = model.Questions.FindIndex((w1) => w1.Id == questId);
            List<int> userChecked = window.GetCheckedAnswers(model.Questions[qIndex].Answers.Count);
            foreach (AnswersModel el in model.Questions[qIndex].Answers) {
                el.UserChoice = false;
            }
            foreach (int el in userChecked) {
                int index = model.Questions[qIndex].Answers.FindIndex((w1) => w1.Id == el);
                model.Questions[qIndex].Answers[index].UserChoice = true;
            }
            if (userChecked.Count != 0)
            {
                window.SetCompleteColor(questId, true);
            }
            else {
                window.SetCompleteColor(questId, false);
            }
            if (!EndCheck())
            {
                count++;
                SetData();
            }
            else {
                System.Windows.Window win = window as System.Windows.Window;

                TestResult result = new TestResult(userId, model, true, win.Left, win.Top, win.WindowState);
                result.ShowDialog();
                win.Close();
                //in model check if answer rightness && userchecked add to answer string
                //add last answer, convert all answers to two string, insert to db history test
            }
        }

        //переміщення між питаннями
        void window_Border_MouseLeftButtonDown(object sender, EventArgs e)
        {
            int id = window.GetIndex(sender);
            count = model.Questions.FindIndex((w1) => w1.Id == id);
            SetData();
        }

        //задання нової порції даних, тобто питання та групи відповідей
        public void SetData(){
            if (model.Questions.Count != 0)
            {
                window.ClearGrid();
                window.SetQuestion(model.Questions[count]);
                window.SetAnswers(model.Questions[count].Answers);
                bool end = EndCheck();
                window.SetNextButton(end, model.Questions[count].Id);
            }
            else window.SendMessage("Вопросы теста отсутствуют");
        }

        //перевірка на закінчення тесту
        bool EndCheck() {
            return (count == model.TaskCount - 1 || count == model.Questions.Count - 1) ? true : false;
        }

    }
}
