using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data.SqlServerCe;

namespace SystemForEnglishLearning.WordLearning
{
    class WordModel : ICloneable//, INotifyPropertyChanged
    {
        string connectionString = "Data Source=|DataDirectory|\\EnglishLearning.sdf";

        public WordModel(int id, string word) {
            WordId = id;
            Word = word;
        }

        public WordModel(int id, string word, string translate, bool inLearning):this(id,word) {
            Translate = translate;
            OnLearning = inLearning;
        }

        public WordModel(int id, string word, string translate, string partOfSpeech, string transcription, byte[] voice, bool inLearning):this(id, word,translate, inLearning) {
            PartOfSpeech = partOfSpeech;
            Transcription = transcription;
            Voice = voice;
        }

        public int WordId
        {
            get;
            private set;
        }

        public string Word
        {
            get;
            private set;
        }

        public string Translate
        {
            get;
            private set;
        }

        public string PartOfSpeech
        {
            get;
            private set;
        }

        public string Transcription
        {
            get;
            private set;
        }

        public byte[] Voice
        {
            get;
            set;
        }

        private bool _onLearning;

        public bool OnLearning
        {
            get {
                return _onLearning;
            }
            set {
                if (value != _onLearning) {
                    _onLearning = value;
                    //UpdateRow(WordId, _onLearning);
                    //NotifyPropertyChanged();
                }
            }
        }

        public object Clone()
        {
            return new WordModel(WordId, Word, Translate, PartOfSpeech, Transcription, Voice, OnLearning) as object;
        }

    }
}
