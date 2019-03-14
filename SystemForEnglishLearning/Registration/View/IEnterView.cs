using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Register
{
    interface IEnterView
    {
        //події натиснення на кнопки реєстрації та входу
        event EventHandler EnterButton_Click;
        event EventHandler RegisterButton_Click;
        //властивості логіну та паролю
        string LoginText { get; }
        string PasswordText { get; }
        //Метод призначений для того, щоб відобразити повідомлення
        void SendMessage(string message);
    }
}
