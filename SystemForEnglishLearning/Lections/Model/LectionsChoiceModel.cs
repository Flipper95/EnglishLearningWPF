using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Lections
{
    class LectionsChoiceModel
    {
        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        List<LectionsModel> lections;

        public LectionsChoiceModel()
        {
            lections = new List<LectionsModel>();
            lections = CreateLectionsList();
        }

        public List<LectionsModel> Lections
        {
            get
            {
                if (lections.Count == 0)
                {
                    lections = CreateLectionsList();
                    return lections;
                }
                else
                    return lections;
            }
            set {
                lections = value;
            }
        }

        //вибір всіх стандартних лекцій
        List<LectionsModel> CreateLectionsList()
        {
            //lections.Clear();
            List<LectionsModel> list = new List<LectionsModel>();
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand()) {
                    cmd.CommandText = "SELECT LectionId, Name, OwnerId, LectionType FROM Lection";
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) { 
                        int id = Convert.ToInt32(dr["LectionId"]);
                        string name = dr["Name"].ToString();
                        int ownerId = Convert.ToInt32(dr["OwnerId"]);
                        string type = dr["LectionType"].ToString();
                        list.Add(new LectionsModel(id, name, ownerId, type, "", new byte[0]));
                    }
                }
            }
            return list;
        }
    }
}
