using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class TestResultPresenter
    {
        ITestResultView window = null;
        TestResultModel model = null;
        BorderPresenter border = null;
        //TODO: delete userId variable
        int userId;

        public TestResultPresenter(ITestResultView win, int userId, TestsModel test, bool save) {
            this.userId = userId;
            window = win;
            model = new TestResultModel(userId, test);
            if (save) model.SaveResult();
            border = new BorderPresenter(win);
            window.SetPercent(model.Percent);
            window.SetMainData(model.TestData);
            window.Border_MouseLeftButtonDown += window_Border_MouseLeftButtonDown;
            window.Window_StateChanged += window_Window_StateChanged;
        }

        void window_Window_StateChanged(object sender, EventArgs e)
        {
            window.ChangeQuestionsSize(model.TestData.Questions);
        }

        void window_Border_MouseLeftButtonDown(object sender, EventArgs e)
        {
            model.TestData.ClearQuestions();
        }

        ~TestResultPresenter() {
            model = null;
        }
    }
}
