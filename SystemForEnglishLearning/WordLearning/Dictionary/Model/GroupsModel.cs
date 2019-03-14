using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    class GroupsModel
    {
        List<GroupModel> groups;
        string connectionString;

        public GroupsModel(){
            groups = new List<GroupModel>();
            connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        }

        //метод зчитує всі групи загального користування
        SqlCeDataReader ReadGroups(out SqlCeConnection connection) {
            connection = new SqlCeConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            SqlCeCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Select * from [Group] Where [OwnerId]=1;";
            SqlCeDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        /// <summary>
        /// Дозволяє записати всі стандартні групи до листу
        /// Allow to write all standard groups to list
        /// </summary>
        /// <returns></returns>
        void ReadStandardGroups() {
            groups.Clear();
            SqlCeConnection connection;
            SqlCeDataReader dr = ReadGroups(out connection);
            while (dr.Read()) {
                int id = Convert.ToInt32(dr["GroupId"]);
                string name = dr["Name"].ToString();
                int wordsCount = Convert.ToInt32(dr["WordsCount"]);
                string difficult = dr["Difficult"].ToString();
                byte[] image = (byte[])dr["Image"];
                int ownerId = (int)dr["OwnerId"];
                groups.Add(new GroupModel(id, name, wordsCount, difficult, image, ownerId));
            }
            connection.Close();
        }

        public List<GroupModel> Groups {
            get {
                if (groups.Count == 0) {
                    ReadStandardGroups();
                    return groups;
                }
                else
                    return groups;
            }
        }
    }
}
