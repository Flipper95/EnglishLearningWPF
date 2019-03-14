using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class TestHistoryModel
    {
        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        int userId;
        TestsModel test;
        public List<TestsHistoryModel> History
        {
            get;
            private set;
        }

        public TestHistoryModel(int userId)
        {
            this.userId = userId;
            History = CreateTestHistory(userId);
        }

        List<TestsHistoryModel> CreateTestHistory(int userId)
        {
            List<TestsHistoryModel> result = new List<TestsHistoryModel>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT h.TestHistoryId, h.TestId, h.PassDate, h.SuccessPercent, h.Questions, h.Answers, t.Name FROM TestHistory as h LEFT JOIN Test AS t ON h.TestId=t.TestId WHERE h.UserId=@userId";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["TestHistoryId"]);
                        int testId = Convert.ToInt32(dr["TestId"]);
                        DateTime passDate = Convert.ToDateTime(dr["PassDate"]);
                        float percent = Convert.ToSingle(dr["SuccessPercent"]);
                        string questions = dr["Questions"].ToString();
                        string answers = dr["Answers"].ToString();
                        string testName = dr["Name"].ToString();
                        result.Add(new TestsHistoryModel(id, userId, testId, passDate, percent, questions, answers, testName));
                    }
                }
            }
            return result;
        }

        public TestsModel GetTest()
        {
            return test;
        }

        public void SetTest(int testId, string questions, string answers)
        {
            test = CreateTest(testId);
            test.Questions = CreateQuestions(ParseQuests(questions), testId);
            ParseAnswers(answers, test.Questions);
        }

        TestsModel CreateTest(int testId)
        {
            TestsModel result = null;
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT TestId, Name, OwnerId, Difficult, TaskCount, TestType FROM Test WHERE TestId=@testId";
                    cmd.Parameters.AddWithValue("@testId", testId);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["TestId"]);
                        string name = dr["Name"].ToString();
                        int ownerId = Convert.ToInt32(dr["OwnerId"]);
                        string difficult = dr["Difficult"].ToString();
                        int taskCount = Convert.ToInt32(dr["TaskCount"]);
                        string type = dr["TestType"].ToString();
                        result = new TestsModel(id, name, ownerId, difficult, taskCount, type);
                    }
                }
            }
            return result;
        }

        //отримання питань відповідно до отриманих ідентифікаторів, які зберігалися в рядку
        List<QuestionsModel> CreateQuestions(int[] questsId, int testId) {
            List<QuestionsModel> result = new List<QuestionsModel>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    string query = "SELECT QuestionId, QuestText FROM Question WHERE TestId=@testId AND QuestionId IN ({0});";//, QuestImage
                    string formQuery = String.Format(query, String.Join(",", questsId));
                    cmd.CommandText = formQuery;
                    cmd.Parameters.AddWithValue("@testId", testId);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["QuestionId"]);
                        string text = dr["QuestText"].ToString();
                        result.Add(new QuestionsModel(id, testId, text));
                    }
                }
            }
            return result;
        }

        //всі відповіді містяться в рядку, тому їх спочатку необхідно розділити
        int[] ParseQuests(string questions) {
            string[] splitted = questions.Split(' ');
            int[] result = new int[splitted.Length];
            for(int i=0;i<splitted.Length-1;i++){
                result[i] = Convert.ToInt32(splitted[i]);
            }
            return result;
        }

        void ParseAnswers(string answers, List<QuestionsModel> questions) {
            string[] splitted = answers.Split('\n',' ');
            for (int i = 0; i < questions.Count; i++) {
                foreach(AnswersModel answer in questions[i].Answers){
                    if (Array.Exists(splitted, w1 => w1 == answer.Id.ToString()))
                    {
                        answer.UserChoice = true;
                    }
                }
            }
        }

    }
}
