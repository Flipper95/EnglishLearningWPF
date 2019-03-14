using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class CreateTestModel
    {
        int userId;
        int constId = 1;
        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";

        public TestsModel Test
        {
            get;
            set;
        }

        public CreateTestModel(int userId) {
            this.userId = userId;
            Test = new TestsModel(0, "", userId, "", 10, "");
        }

        //повернути типи тестів для того щоб користувач вибрав тип тесту який створює
        public string[] GetTestTypes() {
            List<string> result = new List<string>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT DISTINCT TestType FROM Test WHERE OwnerId=@id OR OwnerId=@userId";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@id", constId);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string type = dr["TestType"].ToString();
                        result.Add(type);
                    }
                }
            }
            return result.ToArray();
        }

        public bool SaveTest(string name, int count, string type, string difficult) {
            bool result=true;
            Test.Name = name;
            Test.TaskCount = count;
            Test.Type = type;
            Test.Difficult = difficult;
            Test.TestId = InsertTest(out result);
            if (result)
            {
                foreach (QuestionsModel quest in Test.Questions)
                {
                    int questId = InsertQuestion(Test.TestId, quest);
                    InsertAnswer(questId, quest.Answers);
                }
                return result;
            }
            else return result;
        }

        //збереженян тесту та повернення автоматичного ідентифікатору
        int InsertTest(out bool success) {
            int resultId=0;
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [Test]([Name], [OwnerId], [Difficult], [TaskCount], [TestType]) VALUES(@name,@ownerId,@difficult,@count, @type);";
                    cmd.Parameters.AddWithValue("@name", Test.Name);
                    cmd.Parameters.AddWithValue("@ownerId", userId);
                    cmd.Parameters.AddWithValue("@difficult", Test.Difficult);
                    cmd.Parameters.AddWithValue("@count", Test.TaskCount);
                    cmd.Parameters.AddWithValue("@type", Test.Type);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT @@IDENTITY";
                        resultId = Convert.ToInt32(cmd.ExecuteScalar());
                        success = true;
                    }
                    catch (Exception e) {
                        success = false;
                    }
                }
                connection.Close();
            }
            return resultId;
        }

        //Вставити питання та повернути ідентифікатор
        int InsertQuestion(int testId, QuestionsModel quest) {
            int resultId;
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [Question]([TestId], [QuestText]) VALUES(@testId,@text);"; //OUTPUT INSERTED.QuestionId not working in sql server compact
                    cmd.Parameters.AddWithValue("@testId", testId);
                    cmd.Parameters.AddWithValue("@text", quest.Text);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "SELECT @@IDENTITY";
                    resultId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return resultId;
        }

        void InsertAnswer(int questId, List<AnswersModel> answers) {

            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [Answer]([QuestionId], [AnswerText], [Rightness]) VALUES(@questId, @text, @right)";
                    foreach (AnswersModel answer in answers) {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@questId", questId);
                        cmd.Parameters.AddWithValue("@text", answer.Text);
                        cmd.Parameters.AddWithValue("@right", answer.Rightness);
                        cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

    }
}
