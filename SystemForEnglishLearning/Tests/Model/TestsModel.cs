using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    public class TestsModel
    {
        public TestsModel(int id, string name, int ownerId, string difficult, int taskCount, string type) {
            TestId = id;
            Name = name;
            OwnerId = ownerId;
            Difficult = difficult;
            TaskCount = taskCount;
            Type = type;
            questions = new List<QuestionsModel>();
        }

        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";

        List<QuestionsModel> questions;

        //питання які заповнюються лише при зверненні до них, відразу перемішуються
        public List<QuestionsModel> Questions
        {
            get {
                if (questions.Count == 0)
                {
                    questions = CreateQuestions(TestId);
                    questions = Shuffle.ShuffleList(questions);
                    return questions;
                }
                else {
                    return questions;
                }
            }
            set {
                questions = value;
            }
        }

        public void ClearQuestions() {
            questions.Clear();
        }

        List<QuestionsModel> CreateQuestions(int testId)
        {
            List<QuestionsModel> result = new List<QuestionsModel>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP (@taskCount) QuestionId, QuestText FROM Question WHERE TestId=@id ORDER BY newid();";//, QuestImage
                    cmd.Parameters.AddWithValue("@id", testId);
                    cmd.Parameters.AddWithValue("@taskCount", TaskCount);
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

        public int TestId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int OwnerId
        {
            get;
            private set;
        }

        public string Difficult
        {
            get;
            set;
        }

        public int TaskCount
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }
    }
}
