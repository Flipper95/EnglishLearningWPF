using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Tests
{
    interface ICreateTestView : IBorderView
    {
        //додавання питання
        event EventHandler AddQuest_Click;
        //створення компонентів для нового питання
        void AddNewQuestion(object sender, int questNum);
        //додавання відповіді
        void AddNewAnswer(object sender);
        //встановлення змісту вікна відповідно до вибраного питання
        void SetQuestion(QuestionsModel question);
        //переміщення між питаннями
        event EventHandler Border_MouseLeftButtonDown;
        //подія натискання на нову відповідь
        event EventHandler AddAnswer_Click;
        //збереження створеного питання
        event EventHandler Btn_SaveClick;
        //видалення створеного питання
        event EventHandler Btn_DeleteClick;
        //завершення побудови питань
        event EventHandler Complete_Click;
        //завершення введення додаткової інформації
        event EventHandler End_Click;
        //видалення компонентів питання
        void DeleteQuestion(int number, int questsCount);
        //повернення ідентифікатору поточного питання
        int GetIndex(object sender);
        //повернення створеного питання
        QuestionsModel GetQuestion(out int id);
        void CreateCompleteContent(string[] testTypes, string[] testDifficult);
        bool CheckCompleteFill();
        void GetCompleteValues(out string name, out int count, out string type, out string difficult);
        void SendErrorMessage(string text);
    }
}
