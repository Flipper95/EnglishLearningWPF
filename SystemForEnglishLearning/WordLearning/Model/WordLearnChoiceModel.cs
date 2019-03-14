using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning
{
    class WordLearnChoiceModel
    {
        readonly string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        int userId;

        public WordLearnChoiceModel(int userId) {
            this.userId = userId;
        }

        /// <summary>
        /// Повертає кількість слів для поточного користувача, якщо кількість менше ніж потрібно, можна відіслати певне повідомлення або згенерувати помилку
        /// Return number of words for specific user, if number of words less that need for exercise then you can generate some error
        /// </summary>
        /// <param name="type">type of exercise: "translate", "equivalent", "constructor", "synonym" etc </param>
        /// <returns>return -1 if you type of exercise wrong</returns>
        public int GetCount(string type) {
            string query = "SELECT COUNT(WordId) FROM [LearningWord] WHERE ([LearnPercent] < 100) AND [UserId] = @user";
            switch(type){
                case ("translate"): {
                    return SampleCount(query);
                }
                case ("equivalent"): {
                    return SampleCount(query);
                }
                case ("synonym"): {
                    return SampleCount(query);
                }
                case ("listening"): {
                    //TODO: return query if you want check is there any 5 words with voice before start exercise, if not you can`t do listening exercise
                    //query = "SELECT COUNT(w.WordId) FROM [Word] AS w LEFT JOIN [LearningWord] AS lw ON lw.WordId = w.WordId WHERE lw.UserId = @user AND lw.LearnPercent < 100 AND w.Voice IS NOT NULL";
                    return SampleCount(query);
                }
                case ("constructor"): {
                    return SampleCount(query);
                }
                default:{
                    return -1;
                }
            }
        }

        //Виконує запит
        public int SampleCount(string query) {
            int result;
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@user", userId);
                    result = (int)command.ExecuteScalar();
                }
                connection.Close();
                return result;
            }
        }

    }
}
