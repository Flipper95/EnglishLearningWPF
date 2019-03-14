using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    public class QuestionsModel
    {

        public QuestionsModel(int id, int testId, string text) {
            Id = id;
            TestId = testId;
            Text = text;
            answers = new List<AnswersModel>();
        }

        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";

        List<AnswersModel> answers;

        //відповіді які заповнюються лише при зверненні до них, відразу перемішуються
        public List<AnswersModel> Answers
        {
            get
            {
                if (answers.Count == 0)
                {
                    answers = CreateAnswers(Id);
                    answers = Shuffle.ShuffleList(answers);
                    return answers;
                }
                else
                {
                    return answers;
                }
            }
            private set{
                answers = value;
            }
        }

        List<AnswersModel> CreateAnswers(int questionId)
        {
            List<AnswersModel> result = new List<AnswersModel>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT AnswerId, QuestionId, AnswerText, Rightness FROM Answer WHERE QuestionId=@id"; //AnswerImage,
                    cmd.Parameters.AddWithValue("@id", questionId);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["AnswerId"]);
                        string text = dr["AnswerText"].ToString();
                        bool rightness = Convert.ToBoolean(dr["Rightness"]);
                        result.Add(new AnswersModel(id, questionId, text, rightness));
                    }
                }
            }
            return result;
        }

        public int Id
        {
            get;
            set;
        }

        public int TestId
        {
            get;
            private set;
        }

        public string Text
        {
            get;
            private set;
        }

    }
}
