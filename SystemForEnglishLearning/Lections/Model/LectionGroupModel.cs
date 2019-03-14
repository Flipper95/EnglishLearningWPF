using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Lections
{
    class LectionGroupModel
    {
        public ObservableCollection<LectionsModel> Items { get; set; }
        public string Type { get; set; }

        public LectionGroupModel() {
            Items = new ObservableCollection<LectionsModel>();
        }

    }
}
