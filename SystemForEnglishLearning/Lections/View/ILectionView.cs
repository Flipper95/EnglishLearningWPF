using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SystemForEnglishLearning.Lections
{
    interface ILectionView
    {
        //створення бокового меню з вибором подібної лекції
        void SetDataSideMenu(List<LectionsModel> lectionsList);
        //встановлення вмісту лекції
        void SetMainData(FixedDocumentSequence lection);
        //вибір іншої лекції
        event EventHandler lectionBtn_Click;
        //вибір тестування за пройденою лекцією
        event EventHandler testBtn_Click;
        //повернення ідентифікатору нової лекції
        int GetNewLectionId(object sender);
        //встановлює видимість тесту
        void TestControlEnabled(bool enabled);
        void ContentNullException(string message);
    }
}
