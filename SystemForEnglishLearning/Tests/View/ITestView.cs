using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    interface ITestView : IBorderView
    {
        event EventHandler Border_MouseLeftButtonDown;
        event EventHandler NextBorder_MouseLeftButtonDown;
        //встановлення поточного питання
        void SetQuestion(QuestionsModel quest);
        //встановлення всіх питань (panel з питаннями)
        void SetAllQuestions(List<QuestionsModel> questions);
        //встановлення варіантів відповідей
        void SetAnswers(List<AnswersModel> answers);
        //створення кнопки для переходу на наступне питання
        void SetNextButton(bool end, int questionId);
        //встановлення кольору для прйоденого питання
        void SetCompleteColor(int id, bool answered);
        int GetIndex(object sender);
        //отримання відповідей які дав користувач
        List<int> GetCheckedAnswers(int answerCount);
        void SendMessage(string message);
        void ClearGrid();
    }
}
