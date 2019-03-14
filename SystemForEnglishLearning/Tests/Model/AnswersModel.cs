using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    public class AnswersModel
    {
        public AnswersModel(int id, int questionId, string text, bool rightness)
        { 
            Id = id;
            QuestionId = questionId;
            Text = text;
            Rightness = rightness;
        }

        public int Id
        {
            get;
            private set;
        }

        public int QuestionId
        {
            get;
            private set;
        }

        public string Text
        {
            get;
            private set;
        }

        public bool Rightness
        {
            get;
            private set;
        }

        public bool UserChoice
        {
            get;
            set;
        }
    }
}
