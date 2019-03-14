using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    class SynonymsModel : ExerciseModel
    {
        List<SynonymWordModel> synonyms;
        List<SynonymWordModel> words;

        public List<SynonymWordModel> Synonyms
        {
            get
            {
                return synonyms;
            }
        }

        public List<SynonymWordModel> Words
        {
            get
            {
                return words;
            }
        }

        public SynonymsModel(int userId)
        {
            //words = new List<WordModel>();
            exerciseScore = 15;
            this.userId = userId;
            synonyms = new List<SynonymWordModel>();
            synonyms = GetRandomSynonyms();
            synonyms = Shuffle.ShuffleList(synonyms);
            words = new List<SynonymWordModel>();
            words = GetRandomWords();
            words = Shuffle.ShuffleList(words);
        }

        //Повертає випадкові 5 синонімів, якщо таких не знаходить відразу намагається завантажити
        List<SynonymWordModel> GetRandomSynonyms()
        {
            List<SynonymWordModel> result;
            string query = "SELECT w.*, syn.Synonyms FROM [Word] AS w, [Synonym] AS syn WHERE syn.FirstWordId = w.WordId AND w.WordId IN (SELECT TOP 5 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
            //string query = "SELECT TOP 5 w.*, syn.Synonyms FROM [Word] AS w LEFT JOIN [Synonym] AS syn ON syn.FirstWordId = w.WordId LEFT JOIN [LearningWord] as lw ON lw.WordId = w.WordId WHERE lw.WordId = syn.FirstWordId AND lw.LearnPercent < 100 AND lw.UserId = @user ORDER BY newid()";
            //string query = "SELECT TOP 5 w.*, syn.Synonyms FROM [Word] AS w LEFT JOIN [Synonym] AS syn LEFT JOIN [LearningWord] as lw ON lw.WordId = syn.FirstWordId AND lw.LearnPercent < 100 AND lw.UserId = @user ON syn.FirstWordId = w.WordId ORDER BY newid()";
            result = GenerateAnswerWords(query, true);
            if (result.Count < 5)
            {
                query = "SELECT * FROM [Word] AS w WHERE w.WordId IN (SELECT TOP {0} lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
                int param = 5 - result.Count;
                string formatQuery = String.Format(query, param);
                GenerateAnswerWords(formatQuery, true);
                FindSynonyms(result);
            }
            return result;
        }

        //Повертає випадкові 20 слів, для неправильних відповідей
        List<SynonymWordModel> GetRandomWords()
        {
            string query;
            query = "SELECT TOP 20 w.WordId, w.Word FROM [Word] AS w ORDER BY newid()";
            return GenerateRandomWords(query);
        }

        //Приймає сформований запит та виконує його (обмеження в використанні 1 параметру)
        List<SynonymWordModel> GenerateAnswerWords(string query, bool answer)
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;//"SELECT w.*, syn.Synonyms FROM [Word] AS w LEFT JOIN [Synonym] AS syn ON syn.FirstWordId = w.WordId WHERE w.WordId IN (SELECT TOP 5 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.WordId=syn.WordId lw.UserId = @user ORDER BY newid())";
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
                        string syn = "";
                        try
                        {
                            syn = dr["Synonyms"].ToString();
                        }
                        catch { }
                        synonyms.Add(new SynonymWordModel(id, word, translate, partOfSpeech, transcription, voice, true, syn));
                    }
                }
                connection.Close();
            }
            return synonyms;
        }

        //Випадкові слова, можна навіть повертати не всю частину звязану з даним словом, головне 2 поля слова та ідентифікатору
        List<SynonymWordModel> GenerateRandomWords(string query)
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;//"SELECT w.*, syn.Synonyms FROM [Word] AS w LEFT JOIN [Synonym] AS syn ON syn.FirstWordId = w.WordId WHERE w.WordId IN (SELECT TOP 5 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.WordId=syn.WordId lw.UserId = @user ORDER BY newid())";
                    //command.CommandText = "SELECT w.*, lw.LearnPercent as learned FROM [Word] AS w LEFT JOIN [LearningWord] AS lw ON w.WordId = lw.WordId WHERE w.WordId IN (SELECT TOP 25 lw.WordId FROM [LearningWord] AS lw WHERE (lw.LearnPercent < 100) AND lw.UserId = @user ORDER BY newid())";
                    SqlCeDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = Convert.ToInt32(dr["WordId"]);
                        string word = dr["Word"].ToString();
                        words.Add(new SynonymWordModel(id, word));
                    }
                }
                connection.Close();
            }
            return words;
        }

        //Отримати синоніми з допомогою API
        List<SynonymWordModel> FindSynonyms(List<SynonymWordModel> list)
        {
            SynonymAPI api = new SynonymAPI();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(list[i].Synonyms))
                {
                    List<string> syn = api.GetAllSynonyms(list[i].Word, "Thesaurus");
                    string concat = String.Join(" ", syn.ToArray());
                    list[i].Synonyms = concat;
                    if (!string.IsNullOrEmpty(list[i].Synonyms))
                    {
                        InsertSynonym(list[i].Synonyms, list[i].WordId);
                    }
                    else
                    {
                        list.Remove(list[i]);
                    }

                }
            }
            //clone
            return list;
        }

        //Запам'ятовування синонімів на майбутнє (запис в бд) 
        bool InsertSynonym(string syn, int wordId)
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [Synonym]([FirstWordId],[Synonyms]) VALUES (@wordId, @synonyms);";
                    cmd.Parameters.AddWithValue("@wordId", wordId);
                    cmd.Parameters.AddWithValue("@synonyms", syn);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch {}
                }
            }
            return true;
        }

    }
}
