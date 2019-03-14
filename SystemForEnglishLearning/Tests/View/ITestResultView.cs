using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    interface ITestResultView : IBorderView
    {
        //встановлення проценту успішності
        void SetPercent(float percent);
        //встановлення основних даних пройденого тесту
        void SetMainData(TestsModel test);
        void ChangeQuestionsSize(List<QuestionsModel> questions);
        //закінчення перегляду результатів
        event EventHandler Border_MouseLeftButtonDown;
        event EventHandler Window_StateChanged;
    }
}
