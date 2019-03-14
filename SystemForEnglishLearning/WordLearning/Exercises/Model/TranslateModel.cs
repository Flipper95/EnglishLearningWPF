using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    class TranslateModel : ExerciseModel//: ICloneable
    {
        //int userId;
        List<WordModel> words;
        //float exerciseScore;

        public List<WordModel> Words
        {
            get
            {
                    return words;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"> type of exercise: "translate", "equivalent", "repeat", "constructor" </param>
        public TranslateModel(int userId, string type)
        {
            //words = new List<WordModel>();
            exerciseScore = 15;
            this.userId = userId;
            words = new List<WordModel>();
            words = CreateQuery(type);
            words = Shuffle.ShuffleList(words);
        }

        //створення запиту відповідно до вправи
        List<WordModel> CreateQuery(string type) {
            string query = "SELECT * FROM [Word] AS w WHERE w.WordId IN (SELECT TOP 25 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
            switch (type)
            {
                case ("translate"): {
                    return RandomWords(query);
                }
                case ("equivalent"): {
                    return RandomWords(query);
                }
                case ("repeat"): {
                    query = "SELECT * FROM [Word] AS w WHERE w.WordId IN (SELECT TOP 25 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent = 100) AND lw.UserId = @user ORDER BY newid())";
                    return RandomWords(query);
                }
                case ("constructor"): {
                    query = "SELECT * FROM [Word] AS w WHERE w.WordId IN (SELECT TOP 5 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
                    return RandomWords(query);
                }
                case ("listening"): {
                    query = "SELECT TOP 5 * FROM [Word] AS w LEFT JOIN [LearningWord] AS lw ON lw.WordId = w.WordId WHERE lw.UserId = @user AND lw.LearnPercent < 100 AND w.Voice IS NOT NULL ORDER BY newid()";
                    //query = "SELECT * FROM [Word] AS w WHERE w.WordId IN ";
                    RandomWords(query);
                    if (words.Count < 5)
                    {
                        query = "SELECT * FROM [Word] AS w WHERE w.WordId IN (SELECT TOP {0} lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
                        int param = 5 - words.Count;
                        string formatQuery = String.Format(query, param);
                        RandomWords(formatQuery);
                        UploadVoice(words);
                    }
                    return words;
                }
                default: { return RandomWords(query); }
            }
        }

        //виконання запиту та повернення листу з словами
        List<WordModel> RandomWords(string query)
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;//"SELECT * FROM [Word] AS w WHERE w.WordId IN (SELECT TOP 25 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
                    //command.CommandText = "SELECT w.*, lw.LearnPercent as learned FROM [Word] AS w LEFT JOIN [LearningWord] AS lw ON w.WordId = lw.WordId WHERE w.WordId IN (SELECT TOP 25 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
                    command.Parameters.AddWithValue("@user", userId);
                    SqlCeDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["WordId"]);
                        string word = dr["Word"].ToString();
                        string translate = dr["Translate"].ToString();
                        string partOfSpeech = dr["PartOfSpeech"].ToString();
                        byte[] voice = new byte[1];
                        try
                        {
                            voice = (byte[])dr["Voice"];
                        }
                        catch { }
                        string transcription = dr["Transcription"].ToString();
                        //float percent = Convert.ToSingle(dr["learned"]);
                        words.Add(new WordModel(id, word, translate, partOfSpeech, transcription, voice, true));
                    }
                }
                connection.Close();
            }
            return words;
        }

        //якщо для аудіювання не знайдено 5 озвучених слів завантажується озвучування
        List<WordModel> UploadVoice(List<WordModel> list)
        {
            VoiceAPI api = new VoiceAPI();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Voice.Length<=1)
                {
                    byte[] voice = api.UploadVoice(list[i].Word);
                    list[i].Voice = voice;
                    if (list[i].Voice.Length<=1)
                    {
                        list.Remove(list[i]);
                    }
                }
            }
            return list;
        }

        //public object Clone() {
        //    return new TranslateModel(userId, words);
        //}
    }
}
