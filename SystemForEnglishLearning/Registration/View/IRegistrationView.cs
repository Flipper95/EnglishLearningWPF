using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemForEnglishLearning.Register
{
    interface IRegistrationView
    {
        //подія натиснення на кнопку реєстрації
        event EventHandler RegisterButton_Click;
        //властивості логіну та паролю
        string LoginText { get; }
        string PasswordText { get; }
        //Метод призначений для того, щоб відобразити повідомлення
        void SendMessage(string message);
    }
}
