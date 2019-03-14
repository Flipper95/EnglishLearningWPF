using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Register
{
    class EnterRegisterModel
    {

        //Даний метод призначено для створення нового користувача
        public bool AddUser(string login, string password) {
            SqlCeConnection con = new SqlCeConnection();
            con.ConnectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
            Guid uGuid = System.Guid.NewGuid();
            string hashedPass = HashSHA(password + uGuid.ToString());
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO [User](Login,Password,UserGuid) VALUES(@login, @password, @userguid)", con);
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@password", hashedPass);
            cmd.Parameters.AddWithValue("@userguid", uGuid);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }

        //Валідація логіну та паролю, вони не повинні бути пустими та повинні мати не менше 5 символів
        public bool Validate(string login, string password) {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) 
                || login.Length < 5 || password.Length < 5)
                return false;
            else return true;
        }

        //перевірка логіну на унікальність через селект
        public bool ValidateLoginUnique(string login) {
            SqlCeConnection con = new SqlCeConnection();
            con.ConnectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
            SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(Login) FROM [User] WHERE Login=@login", con);
            cmd.Parameters.AddWithValue("@login", login);
            con.Open();
            object result = cmd.ExecuteScalar();
            if ((int)result >= 1) return false;
            else return true;
        }

        //Перевірка правильності введення логіну та паролю, якщо такий користувач існує в базі повертається його ідентифікатор
        public int CheckUser(string login, string password) {
            SqlCeConnection con = new SqlCeConnection();
            con.ConnectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
            SqlCeCommand cmd = new SqlCeCommand("SELECT UserId, Password, UserGuid FROM [User] WHERE Login=@login", con);
            cmd.Parameters.AddWithValue("@login", login);
            con.Open();
            SqlCeDataReader dr = cmd.ExecuteReader();
            int result = 0;
            while (dr.Read()) {
                int userId = Convert.ToInt32(dr["UserId"]);
                string basePassword = Convert.ToString(dr["Password"]);
                string baseUserGuid = Convert.ToString(dr["UserGuid"]);
                string hashPass = HashSHA(password + baseUserGuid);
                if (hashPass == basePassword) result = userId;
            }
            con.Close();
            return result;
        }

        //використання методу шифрування sha(secure hash algorithm)
        string HashSHA(string value) {
            var sha = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha.ComputeHash(Encoding.Unicode.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
