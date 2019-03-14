using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Statistics
{
    public class DataModel
    {
        public DataModel(int count, string date){
            Count = count;
            Date = date;
        }

        public int Count { get; set; }

        public string Date { get; set; }
    }
}
