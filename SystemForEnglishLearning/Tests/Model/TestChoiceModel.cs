using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class TestChoiceModel
    {
         string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        List<TestsModel> tests;
        List<TestsModel> userTests;
        int userId;
        int constId = 1;

        public TestChoiceModel(int userId)
        {
            tests = new List<TestsModel>();
            userTests = new List<TestsModel>();
            this.userId = userId;
        }

        //стандартні тести
        public List<TestsModel> Tests
        {
            get
            {
                if (tests.Count == 0)
                {
                    tests = CreateTestList(constId);
                    return tests;
                }
                else
                    return tests;
            }
        }

        //тести користувача
        public List<TestsModel> UserTests
        {
            get {
                if (userTests.Count == 0)
                {
                    userTests = CreateTestList(userId);
                    return userTests;
                }
                else
                    return userTests;
            }
        }

        //вибір тестів з бд
        List<TestsModel> CreateTestList(int userId)
        {
            List<TestsModel> list = new List<TestsModel>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand()) {
                    cmd.CommandText = "SELECT TestId, Name, OwnerId, Difficult, TaskCount, TestType FROM Test WHERE OwnerId=@ownerId";
                    cmd.Parameters.AddWithValue("@ownerId", userId);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) { 
                        int id = Convert.ToInt32(dr["TestId"]);
                        string name = dr["Name"].ToString();
                        int ownerId = Convert.ToInt32(dr["OwnerId"]);
                        string difficult = dr["Difficult"].ToString();
                        int taskCount = Convert.ToInt32(dr["TaskCount"]);
                        string type = dr["TestType"].ToString();
                        list.Add(new TestsModel(id, name, ownerId, difficult, taskCount, type));
                    }
                }
            }
            return list;
        }

        public bool DeleteTest(int testId) {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Test WHERE TestId = @testId";
                    cmd.Parameters.AddWithValue("@testId", testId);
                    int number = cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

    }
}
