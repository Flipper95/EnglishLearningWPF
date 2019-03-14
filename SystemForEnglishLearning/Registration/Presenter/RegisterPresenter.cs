using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemForEnglishLearning.Register
{
    class RegisterPresenter
    {
        IRegistrationView win = null;
        EnterRegisterModel model = null;

        public RegisterPresenter(IRegistrationView registerWindow)
        {
            model = new EnterRegisterModel();
            win = registerWindow;
            win.RegisterButton_Click += new EventHandler(registerWindow_RegisterButtonClick);
        }

        //перевірка введених даних та якщо все добре внесення нового користувача
        void registerWindow_RegisterButtonClick(object sender, EventArgs e) {
            if (model.Validate(win.LoginText, win.PasswordText) && model.ValidateLoginUnique(win.LoginText))
            {
                if (model.AddUser(win.LoginText, win.PasswordText))
                {
                    win.SendMessage("Вас успешно зарагестрировано!");
                    (win as Window).Close();
                }
            }
            else {
                win.SendMessage("Логин должен быть уникальным, логин и пароль минимум 5 символов");
            }
        }
    }
}
