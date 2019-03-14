using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    class TestGroupModel
    {
        public ObservableCollection<TestsModel> Items { get; set; }
        public string Type { get; set; }

        public TestGroupModel() {
            Items = new ObservableCollection<TestsModel>();
        }
    }
}
