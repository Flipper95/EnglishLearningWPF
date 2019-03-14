using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class TestResultModel
    {
        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        int userId;
        string[] storyData;

        public float Percent
        {
            get;
            private set;
        }

        public TestResultModel(int userId, TestsModel data) {
            TestData = data;
            this.userId = userId;
            float percent;
            storyData = GetRightAnswer(TestData, out percent);
            Percent = percent;
        }

        public void SaveResult()
        {
            SaveResults(TestData, Percent, storyData[1], storyData[0]);
        }

        public TestsModel TestData
        {
            get;
            private set;
        }

        void SaveResults(TestsModel test, float percent, string answers, string questions) {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                        cmd.CommandText = "INSERT INTO [TestHistory]([UserId], [TestId], [PassDate], [SuccessPercent], [Questions], [Answers]) VALUES(@userId,@testId,@passDate,@percent, @question, @answer);";
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@testId", test.TestId);
                        cmd.Parameters.AddWithValue("@passDate", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@percent", percent);
                        cmd.Parameters.AddWithValue("@question", questions);
                        cmd.Parameters.AddWithValue("@answer", answers);
                        cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        //доволі складний метод, який в майбутньому необхідно буде переробити
        //result - відповіді, питання allFalse - показує, що правильної відповіді в питанні не має, 
        //answerCount - кількість правильних відповідей, rAnswerCount - кількість правильних питань які дав користувач
        //спочатку відбувається прохід по всім питанням, для кожного питання перебираються всі відповіді
        string[] GetRightAnswer(TestsModel data, out float percent) {
            string[] result = new string[2]{"",""};
            bool allFalse = true;
            int answerCount = 0;
            int rAnswerCount = 0;
            int allCount = 0;
            foreach (QuestionsModel quest in data.Questions) {
                answerCount = 0;
                rAnswerCount = 0;
                foreach (AnswersModel answer in quest.Answers) {
                    if (answer.Rightness == true && answer.UserChoice == true)
                    {
                        allFalse = false;
                        result[1] += answer.Id + " ";
                        rAnswerCount++;
                        answerCount++;
                    }
                    else if (answer.Rightness == true) {
                        answerCount++;
                        allFalse = false;
                    }
                    else if (answer.UserChoice == true) {
                        allFalse = false;
                        result[1] += answer.Id + " ";
                        rAnswerCount--;
                    }
                }
                    result[0] += quest.Id+" ";
                    result[1] += "\n";
                    if (rAnswerCount < 0)
                    {
                        rAnswerCount = 0;
                    }
                    else if (answerCount != 0)
                    {
                        allCount += (rAnswerCount * 100) / answerCount;
                    }

                if (allFalse) {
                    if (allFalse)
                    {
                        allCount += 100;
                    }
                }
                allFalse = true;
            }
            percent = allCount / data.Questions.Count;
            return result;
        }

    }
}
