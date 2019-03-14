using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemForEnglishLearning.Tests
{
    class CreateTestPresenter
    {
        ICreateTestView window = null;
        CreateTestModel model = null;
        BorderPresenter border = null;
        int questionCount;

        public CreateTestPresenter(ICreateTestView win, int userId) {
            window = win;
            border = new BorderPresenter(win);
            model = new CreateTestModel(userId);
            win.AddQuest_Click += win_AddQuest_Click;
            win.Border_MouseLeftButtonDown += win_Border_MouseLeftButtonDown;
            win.AddAnswer_Click += win_AddAnswer_Click;
            win.Btn_SaveClick += win_Btn_SaveClick;
            win.Btn_DeleteClick += win_Btn_DeleteClick;
            win.Complete_Click += win_Complete_Click;
            win.End_Click += win_End_Click;
            questionCount = 0;
        }

        //отримання додаткових даних про створюваний тест, його переміщення в бд
        void win_End_Click(object sender, EventArgs e)
        {
            if (window.CheckCompleteFill()) {
                string name;
                int count;
                string type;
                string difficult;
               window.GetCompleteValues(out name, out  count, out  type, out  difficult);
               if (!model.SaveTest(name, count, type, difficult))
               {
                   window.SendErrorMessage("Тест не удалось добавить, попробуйте поменять имя теста и попробывать снова");
               }
               else (window as Window).Close();
            }
        }

        //Останній крок для створення теста, вибір типу складності назви ...
        void win_Complete_Click(object sender, EventArgs e)
        {
            string[] types = model.GetTestTypes();
            string[] difficult = new string[] { "Beginner", "Pre-Intermediate", "Intermediate", "Upper-Intermediate", "Advanced", "Proficiency" };
            window.CreateCompleteContent(types, difficult);
        }

        //видаленння тесту (тестова версія)
        void win_Btn_DeleteClick(object sender, EventArgs e)
        {
            int count;
            QuestionsModel question = window.GetQuestion(out count);
            if (question != null)
            {
                count = question.Id;
                int index = model.Test.Questions.FindIndex((w1) => w1.Id == question.Id);
                if (index != -1)
                {
                    model.Test.Questions.RemoveAt(index);
                    if (index <= model.Test.Questions.Count)
                    {
                        for (int i = index; i < model.Test.Questions.Count; i++)
                        {
                            model.Test.Questions[i].Id += 1;
                        }
                    }
                }
            }
            questionCount++;
            window.DeleteQuestion(count,questionCount*-1);
        }

        //Збереження змін до питання, або внесення нового
        void win_Btn_SaveClick(object sender, EventArgs e)
        {
            int count;
            QuestionsModel question = window.GetQuestion(out count);
            if (question != null)
            {
                int index = model.Test.Questions.FindIndex((w1) => w1.Id == question.Id);
                if (index != -1) model.Test.Questions[index] = question;
                else model.Test.Questions.Add(question);
            }
        }

        //добавити відповідь
        void win_AddAnswer_Click(object sender, EventArgs e)
        {
            window.AddNewAnswer(sender);
        }

        //переміщення між питаннями
        void win_Border_MouseLeftButtonDown(object sender, EventArgs e)
        {
            int index = window.GetIndex(sender);
            int count = model.Test.Questions.FindIndex((w1) => w1.Id == index);
            window.SetQuestion(model.Test.Questions[count]);
        }

        //створення нового пустого питання
        void win_AddQuest_Click(object sender, EventArgs e)
        {
            questionCount--;
            window.AddNewQuestion(sender, questionCount);
        }
    }
}
