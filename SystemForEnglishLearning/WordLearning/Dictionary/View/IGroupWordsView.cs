using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    interface IGroupWordsView : IBorderView
    {
        event EventHandler Button_MouseLeftButtonUp;
        //збереження всіх внесених змін
        event EventHandler Btn_Add_Click;
        //закриття вікна
        event EventHandler Btn_Exit_Click;
        //натиснення на кнопку пошуку
        event EventHandler Btn_Search_Click;
        //зміна положення прапору який відмічає відразу всі слова
        event EventHandler ChBx_Checked;
        //встановлення основних даних
        void SetData<T>(List<T> data);
        //доступність переходу на наступну сторінку
        void EnablePrev(bool enable);
        //доступність переходу на попередню сторінку
        void EnableNext(bool enable);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// Англійське слово під індексом 0, переклад з індексом 1
        /// English word string with index 0, translate string with index 1</returns>
        string[] GetSearchText();
        //повернення прапору, що відмічено всі слова
        bool GetCheckAll();
        //відправити повідомлення
        void SendMessage(string message);
        bool SendMessage(string message, string title);
    }
}
