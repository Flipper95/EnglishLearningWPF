using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class TestHistoryPresenter
    {
        ITestHistoryView win = null;
        TestHistoryModel model = null;
        int userId;

        public TestHistoryPresenter(ITestHistoryView window, int userId)
        {
            this.userId = userId;
            win = window;
            model = new TestHistoryModel(userId);
            win.SetHistoryData(model.History);
            win.Row_DoubleClick += win_Row_DoubleClick;
        }

        //вибір тесту серед історії пройдених
        void win_Row_DoubleClick(object sender, EventArgs e)
        {
            TestsHistoryModel choosen = win.GetSelectedRow(sender) as TestsHistoryModel;
            model.SetTest(choosen.TestId, choosen.Questions, choosen.Answers);
            TestResult result = new TestResult(userId, model.GetTest(), false, (win as System.Windows.Window).WindowState);
            result.Show();
        }
    }
}
