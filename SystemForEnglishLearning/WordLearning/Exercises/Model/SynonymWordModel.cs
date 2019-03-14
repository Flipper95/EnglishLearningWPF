using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    class SynonymWordModel : WordModel
    {
        public SynonymWordModel(int id, string word, string translate, string partOfSpeech, string transcription, byte[] voice, bool inLearning, string synonyms) : base(id, word, translate, partOfSpeech, transcription, voice, inLearning) {
            Synonyms = synonyms;
        }

        public SynonymWordModel(int id, string word) : base(id, word) { }

        public string Synonyms
        {
            get;
            set;
        }
    }
}
