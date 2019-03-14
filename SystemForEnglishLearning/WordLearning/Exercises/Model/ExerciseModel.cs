using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    class ExerciseModel
    {
        protected string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        protected int userId;
        protected float exerciseScore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rightAnswers">
        /// Лист ідентифікаторів WordId з правильними словами
        /// WordId for right answered words
        /// </param>
        public bool UpdateScore(List<int> rightAnswers)
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    string formQuery = "";
                    if (rightAnswers.Count > 0)
                    {
                        string query = "SELECT [LearnPercent],[WordId] FROM [LearningWord] WHERE WordId IN ({0});";
                        formQuery = String.Format(query, String.Join(",", rightAnswers.ToArray()));
                    }
                    else
                    {
                        formQuery = "SELECT [LearnPercent],[WordId] FROM [LearningWord] WHERE WordId IS NULL";
                    }
                    command.CommandText = formQuery;
                    SqlCeDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        float percent = Convert.ToSingle(dr["LearnPercent"]);
                        int wordId = Convert.ToInt32(dr["WordId"]);
                        DateTime date = DateTime.Now;
                        using (SqlCeCommand cmd = connection.CreateCommand())
                        {
                            //TODO: if percent=100 update without learned date
                            if (percent >= (100 - exerciseScore))
                            {
                                percent = 100;
                                cmd.CommandText = "UPDATE [LearningWord] SET [LearnPercent]=@learnPercent, [LearnedDate]=@learnDate, [ExerciseDate]=@exerciseDate WHERE [WordId] = @wordId AND [UserId] = @userId";
                                cmd.Parameters.AddWithValue("@learnDate", date.Date);
                            }
                            else
                            {
                                percent += exerciseScore;
                                cmd.CommandText = "UPDATE [LearningWord] SET [LearnPercent]=@learnPercent, [ExerciseDate]=@exerciseDate WHERE [WordId] = @wordId AND [UserId] = @userId";
                            }
                            cmd.Parameters.AddWithValue("@learnPercent", percent);
                            cmd.Parameters.AddWithValue("@exerciseDate", date.Date);
                            cmd.Parameters.AddWithValue("@wordId", wordId);
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            return true;
        }


    }
}
