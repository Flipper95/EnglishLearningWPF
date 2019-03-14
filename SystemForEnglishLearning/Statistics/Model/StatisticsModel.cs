using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Statistics
{
    class StatisticsModel
    {

        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        int userId;

        public StatisticsModel(int userId) {
            this.userId = userId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">слово,тест чи кількість доданих слів / word, test or addedWord</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public List<DataModel> GetChosenCount(string type, DateTime start, DateTime end){
            string query;
            end = end.AddDays(1);
            List<DataModel> result = new List<DataModel>();
            switch (type) {
                case ("word"): {
                    query = "SELECT COUNT(LearningWordId) as count, LearnedDate as date FROM LearningWord WHERE UserId=@userId AND LearnPercent = 100 AND LearnedDate BETWEEN @startDate AND @endDate GROUP BY LearnedDate";
                    result = GetCount(query, start, end);
                    break;
                }
                case ("test"): {
                    query = "SELECT COUNT(TestHistoryId) as count, PassDate as date FROM TestHistory WHERE UserId=@userId AND PassDate BETWEEN @startDate AND @endDate GROUP BY PassDate";
                    result = GetCount(query, start, end);
                    break;
                }
                case ("addedWord"): {
                    query = "SELECT COUNT(LearningWordId) as count, AddedDate as date FROM LearningWord WHERE UserId=@userId AND AddedDate BETWEEN @startDate AND @endDate GROUP BY AddedDate";
                    result = GetCount(query, start, end);
                    break;
                }
            }
            return result;
        }

        List<DataModel> GetCount(string query, DateTime start, DateTime end){
            List<DataModel> result = new List<DataModel>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@startDate", start);
                    cmd.Parameters.AddWithValue("@endDate", end);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int count = Convert.ToInt32(dr["count"]);
                        DateTime date = Convert.ToDateTime(dr["date"]);
                        result.Add(new DataModel(count, date.Date.ToShortDateString()));
                    }
                }
            }
            return result;
        }

    }
}
