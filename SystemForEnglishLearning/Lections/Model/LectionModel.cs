using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Lections
{
    class LectionModel
    {
        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        LectionsModel lection;
        int constId = 1;
        int lectionId;
        int userId;

        public LectionModel(int lectionId, int userId)
        {
            this.userId = userId;
            this.lectionId = lectionId;
            lection = CreateLection(lectionId);
            Test = CreateTest(lection.Name, userId);
        }

        public Tests.TestsModel Test
        {
            get;
            private set;
        }

        public LectionsModel GetLection()
        {
            if (lection != null && lection.Id == lectionId)
            {
                return lection;
            }
            else
            {
                lection = CreateLection(lectionId);
                Test = CreateTest(lection.Name, userId);
                return lection;
            }
        }

        public void SetLectionId(int lectionId)
        {
            this.lectionId = lectionId;
        }

        LectionsModel CreateLection(int lectionId)
        {
            LectionsModel result = null;
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT LectionId, Name, OwnerId, LectionType, Description, LectionText FROM Lection WHERE LectionId=@id";
                    cmd.Parameters.AddWithValue("@id", lectionId);
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["LectionId"]);
                        string name = dr["Name"].ToString();
                        int ownerId = Convert.ToInt32(dr["OwnerId"]);
                        string type = dr["LectionType"].ToString();
                        string descr = dr["Description"].ToString();
                        byte[] text = dr["LectionText"] as byte[];
                        result = new LectionsModel(id, name, ownerId, type, descr, text);
                    }
                }
            }
            return result;
        }

        //повертається тест для того, щоб можна було відразу пройти тест на знання вибраного матеріалу з лекції
        Tests.TestsModel CreateTest(string testName, int userId) {
            Tests.TestsModel result = null;
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 TestId, Name, OwnerId, Difficult, TaskCount, TestType FROM Test WHERE Name LIKE @name AND (OwnerId=@ownerId OR OwnerId=@adminId) ORDER BY newid()";
                    cmd.Parameters.AddWithValue("@ownerId", userId);
                    cmd.Parameters.AddWithValue("@adminId", constId);
                    cmd.Parameters.AddWithValue("@name", '%' + testName + '%');
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["TestId"]);
                        string name = dr["Name"].ToString();
                        int ownerId = Convert.ToInt32(dr["OwnerId"]);
                        string difficult = dr["Difficult"].ToString();
                        int taskCount = Convert.ToInt32(dr["TaskCount"]);
                        string type = dr["TestType"].ToString();
                        result = new Tests.TestsModel(id, name, ownerId, difficult, taskCount, type);
                    }
                }
            }
            return result;
        }

    }
}
