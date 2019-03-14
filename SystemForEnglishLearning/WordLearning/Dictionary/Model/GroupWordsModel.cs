using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    class GroupWordsModel
    {
        int userId;
        int groupId;
        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";
        List<WordModel> words;
        int index;
        double maxindex;
        bool find;
        string searchWord;
        string searchTranslate;

        public int GetPageIndex()
        {
            return index;
        }

        public double GetMaxPageIndex()
        {
            return maxindex;
        }

        /// <summary>
        /// Повертає true якщо операція виконана успішно
        /// return true if operation successful
        /// </summary>
        /// <param name="nextOrBack">
        /// true if you need next 100 data values, false if you need previous 100 data values
        /// Якщо необхідно вибрати наступні 100 значень параметр потрібно встановти true, якщо попередні false
        /// </param>
        public bool SetPageIndex(bool nextOrBack)
        {
            if (!nextOrBack && index == 1) return false;
            else
            {
                if (nextOrBack && index > maxindex) return false;
                else
                {
                    if (nextOrBack) index++;
                    else index--;
                    words.Clear();
                    return true;
                }
            }
        }

        public GroupWordsModel(int UserId, int GroupId)
        {
            words = new List<WordModel>();
            userId = UserId;
            groupId = GroupId;
            index = 1;
            MaxIndex();
            find = false;
        }

        //повертає кількість сторінок (в розрахунку 1 сторінка 100 записів)
        void MaxIndex()
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                //connection.ConnectionString = connectionString;
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "Select [WordsCount] from [Group] Where [GroupId]=@group;";
                    cmd.Parameters.AddWithValue("@group", groupId);
                    maxindex = Convert.ToInt32(cmd.ExecuteScalar()) / 100;
                }
                connection.Close();
            }
        }

        //повертає кількість сторінок (в розрахунку 1 сторінка 100 записів), використовується при пошуку слів
        void MaxIndex(string word, string translate) {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "Select COUNT([WordId]) from [Word] Where [GroupId]=@group";
                    if (!string.IsNullOrEmpty(word)) {
                        cmd.CommandText += " AND Word LIKE @word";
                        cmd.Parameters.AddWithValue("@word", word + '%');
                    }
                    if (!string.IsNullOrEmpty(translate)) {
                        cmd.CommandText += " AND Word LIKE @translate";
                        cmd.Parameters.AddWithValue("@translate", translate + '%');
                    }
                    cmd.Parameters.AddWithValue("@group", groupId);
                    maxindex = Convert.ToInt32(cmd.ExecuteScalar()) / 100;
                }
                connection.Close();
            }
        }

        //повертає набір слів, якщо лічильник 0, то відбувається зчитування слів з бд
        public List<WordModel> Words
        {
            get
            {
                if (words.Count == 0)
                {
                    if (!find)
                    {
                        SqlCeConnection connection;
                        SqlCeDataReader dr = GetDataReader(out connection);
                        ReadWords(index, dr, connection);
                        return words;
                    }
                    else {
                        SqlCeConnection connection;
                        SqlCeDataReader dr = GetFindDataReader(out connection,searchWord,searchTranslate);
                        ReadWords(index, dr, connection);
                        return words;
                    }
                }
                else
                    return words;
            }
        }

        //встановлює значення для пошуку
        public void FindWords(string word, string translate)
        {
            SqlCeConnection connection;
            SqlCeDataReader dr = GetFindDataReader(out connection, word, translate);
            ReadWords(index, dr, connection);
            MaxIndex(word, translate);
            index = 1;
            if (!string.IsNullOrEmpty(word) || !string.IsNullOrEmpty(translate))
            {
                searchWord = word;
                searchTranslate = translate;
                find = true;
            }
            else {
                find = false;
            }
        }

        //запит вибирає слова з бд використовуючи критерії пошуку
        SqlCeDataReader GetFindDataReader(out SqlCeConnection connection, string word, string translate) {
            connection = new SqlCeConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            SqlCeCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT w.WordId, w.Word, w.Translate, CASE WHEN EXISTS(SELECT 1 FROM [LearningWord] AS lw WHERE lw.UserId=@user AND lw.WordId=w.WordId) THEN 'true' ELSE 'false' END AS learned FROM [Word] AS w WHERE w.GroupId = @group";
            if (!string.IsNullOrEmpty(word)) {
                cmd.CommandText += " AND w.Word LIKE @word";
                cmd.Parameters.AddWithValue("@word", word + '%');
            }
            if (!string.IsNullOrEmpty(translate)) {
                cmd.CommandText += " AND w.Translate LIKE @translate";
                cmd.Parameters.AddWithValue("@translate", translate+'%');
            }
            cmd.CommandText += " ORDER BY w.WordId OFFSET @startIndex ROWS FETCH NEXT 100 ROWS ONLY";
            //cmd.CommandText = "SELECT w.*, CASE WHEN lw.LearningWordId IS NULL THEN 'false' ELSE 'true' END AS learned FROM [Word] AS w LEFT JOIN [LearningWord] AS lw ON lw.WordId = w.WordId WHERE (lw.UserId = @user OR lw.UserId IS NULL) AND w.GroupId = @group ORDER BY w.WordId OFFSET @startIndex ROWS FETCH NEXT 100 ROWS ONLY;";
            cmd.Parameters.AddWithValue("@user", userId);
            cmd.Parameters.AddWithValue("@group", groupId);
            int startIndex = index * 100 - 100;
            cmd.Parameters.AddWithValue("@startIndex", startIndex);
            SqlCeDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        SqlCeDataReader GetDataReader(out SqlCeConnection connection)
        {
            connection = new SqlCeConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            SqlCeCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT w.WordId, w.Word, w.Translate, CASE WHEN EXISTS(SELECT 1 FROM [LearningWord] AS lw WHERE lw.UserId=@user AND lw.WordId=w.WordId) THEN 'true' ELSE 'false' END AS learned FROM [Word] AS w WHERE w.GroupId = @group ORDER BY w.WordId OFFSET @startIndex ROWS FETCH NEXT 100 ROWS ONLY;";
            //cmd.CommandText = "SELECT w.*, CASE WHEN lw.LearningWordId IS NULL THEN 'false' ELSE 'true' END AS learned FROM [Word] AS w LEFT JOIN [LearningWord] AS lw ON lw.WordId = w.WordId WHERE (lw.UserId = @user OR lw.UserId IS NULL) AND w.GroupId = @group ORDER BY w.WordId OFFSET @startIndex ROWS FETCH NEXT 100 ROWS ONLY;";
            cmd.Parameters.AddWithValue("@user", userId);
            cmd.Parameters.AddWithValue("@group", groupId);
            int startIndex = index * 100 - 100;
            cmd.Parameters.AddWithValue("@startIndex", startIndex);
            SqlCeDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        /// <summary>
        /// Створює лист значень з переданого datareader
        /// Allow to write part of group words to list
        /// </summary>
        /// <returns></returns>
        void ReadWords(int index, SqlCeDataReader dr, SqlCeConnection connection)
        {
            words.Clear();
            while (dr.Read())
            {
                int id = Convert.ToInt32(dr["WordId"]);
                string word = dr["Word"].ToString();
                string translate = dr["Translate"].ToString();
                //int group = Convert.ToInt32(dr["GroupId"]);
                //string partOfSpeech = dr["PartOfSpeech"].ToString();
                //byte[] voice=new byte[1];
                //try
                //{
                //    voice = (byte[])dr["Voice"];
                //}
                //catch { }
                //string transcription = dr["Transcription"].ToString();
                bool inLearning = false;
                string learned = dr["learned"].ToString();
                if (learned == "false")
                {
                    inLearning = false;
                }
                else inLearning = true;

                words.Add(new WordModel(id, word, translate, inLearning));
                //words[words.Count-1].userId = userId;
                //words.Add(new WordModel(id, word, translate, partOfSpeech, transcription, voice, inLearning));
            }
            connection.Close();
        }

        //дозволяє додавати слова на вивчення або видаляти їх для поточного користувача
        public bool UpdateRow(int wordId, bool inLearning)
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    if (inLearning)
                    {
                        cmd.CommandText = "INSERT INTO [LearningWord]([WordId], [UserId], [LearnPercent], [AddedDate]) VALUES(@word,@user,@percent, @addedDate);";
                        cmd.Parameters.AddWithValue("@word", wordId);
                        cmd.Parameters.AddWithValue("@user", userId);
                        cmd.Parameters.AddWithValue("@percent", 0);
                        DateTime addedDate = DateTime.Now;
                        cmd.Parameters.AddWithValue("@addedDate", addedDate.Date);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = "DELETE FROM [LearningWord] WHERE [WordId]=@word AND [UserId]=@user;";
                        cmd.Parameters.AddWithValue("@word", wordId);
                        cmd.Parameters.AddWithValue("@user", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            return true;
        }

    }
}
