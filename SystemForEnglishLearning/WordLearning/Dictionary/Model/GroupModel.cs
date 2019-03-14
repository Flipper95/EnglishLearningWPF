using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    public class GroupModel
    {
        public GroupModel(int groupId, string name, int wordsCount, string difficult, byte[] image, int ownerId) {
            Group = groupId;
            Name = name;
            WordsCount = wordsCount;
            Difficult = difficult;
            Image = image;
            OwnerId = ownerId;
        }

        public int Group
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public int WordsCount
        {
            get;
            private set;
        }

        public string Difficult
        {
            get;
            private set;
        }

        public byte[] Image
        {
            get;
            private set;
        }

        public int OwnerId
        {
            get;
            private set;
        }
    }
}
