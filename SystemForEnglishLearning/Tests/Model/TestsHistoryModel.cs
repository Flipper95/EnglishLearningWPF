using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    public class TestsHistoryModel
    {
        public TestsHistoryModel(int historyId, int userId, int testId, DateTime passDate, float Percent, string questions, string answers)
        {
            TestHistoryId = historyId;
            UserId = userId;
            TestId = testId;
            PassDate = passDate;
            SuccessPercent = Percent;
            Questions = questions;
            Answers = answers;
        }

        public TestsHistoryModel(int historyId, int userId, int testId, DateTime passDate, float Percent, string questions, string answers, string testName):
            this(historyId, userId, testId, passDate, Percent, questions, answers)
        {
            TestName = testName;
        }

        public int TestHistoryId
        {
            get;
            private set;
        }

        public int UserId
        {
            get;
            private set;
        }

        public int TestId
        {
            get;
            private set;
        }

        public DateTime PassDate
        {
            get;
            private set;
        }

        public float SuccessPercent
        {
            get;
            private set;
        }

        public string Questions
        {
            get;
            private set;
        }

        public string Answers
        {
            get;
            private set;
        }

        public string TestName
        {
            get;
            set;
        }
    }
}
