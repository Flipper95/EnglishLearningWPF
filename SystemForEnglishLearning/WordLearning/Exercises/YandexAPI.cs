using System;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    class SynonymAPI
    {

        int synonymNumber = 5;
        //public string GetWordSynonym(string word) {

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="ApiName">Yandex, Thesaurus</param>
        /// <returns></returns>
        public List<string> GetAllSynonyms(string word, string ApiName)
        {
            string synonyms = null;
            List<string> result = new List<string>();
            try
            {
                if (ApiName == "Yandex")
                {
                    synonyms = YandexUploadSynonyms(word);
                    result = YandexParse(synonyms);
                }
                else if (ApiName == "Thesaurus")
                {
                    synonyms = ThesaurusUploadSynonyms(word);
                    result = ThesaurusParse(synonyms);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
            return result;
        }

        //отримання синонімів з xml
        List<string> YandexParse(string xml)
        {
            List<string> synonyms = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList elemlist = doc.GetElementsByTagName("syn");
            int endIndex;
            if (elemlist.Count <= synonymNumber) { endIndex = elemlist.Count; }
            else { endIndex = synonymNumber; }
            for (int i = 0; i < endIndex; i++)
            {
                XmlNodeList text = elemlist[i].ChildNodes;
                synonyms.Add(text[0].InnerText);
            }

            return synonyms;
        }

        //завантаження синонімів
        string YandexUploadSynonyms(string word)
        {
            using (var client = new System.Net.WebClient())
            {
                return client.DownloadString("https://dictionary.yandex.net/api/v1/dicservice/lookup?key=dict.1.1.20170423T233402Z.41456fdb00fc6d24.9f39b3d48c80cd270a8a1270056119e899777bc2&lang=en-en&text=" + word);
            }
        }

        string ThesaurusUploadSynonyms(string word)
        {
            using (var client = new System.Net.WebClient())
            {
                return client.DownloadString("http://words.bighugelabs.com/api/2/775f6240f10479aba75051bf2ad1978a/" + word + "/xml");
            }
        }

        List<string> ThesaurusParse(string xml)
        {
            List<string> synonyms = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlReader reader = new XmlNodeReader(doc);
            while (reader.ReadToFollowing("w") && synonyms.Count < synonymNumber)
            {
                reader.MoveToAttribute("r");
                string type = reader.Value;
                reader.MoveToContent();
                string result = reader.ReadInnerXml();
                if (type == "syn")
                {
                    synonyms.Add(result);
                }
            }

            return synonyms;
        }

    }
}
