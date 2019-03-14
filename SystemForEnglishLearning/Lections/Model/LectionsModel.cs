using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Lections
{
    public class LectionsModel
    {

        public LectionsModel(int id, string name, int ownerId, string lectionType, string description, byte[] text) {
            Id = id;
            Name = name;
            OwnerId = ownerId;
            Type = lectionType;
            Description = description;
            Text = text;
        }

        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public int OwnerId
        {
            get;
            private set;
        }

        public string Type
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public byte[] Text
        {
            get;
            private set;
        }
    }
}
