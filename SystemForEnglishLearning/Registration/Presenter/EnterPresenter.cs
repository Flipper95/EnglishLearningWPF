using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemForEnglishLearning.Register
{
    class EnterPresenter
    {
        IEnterView win = null;
        EnterRegisterModel model = null;

        public EnterPresenter(IEnterView enterWindow)
        {
            model = new EnterRegisterModel();
            win = enterWindow;
            win.EnterButton_Click += new EventHandler(enterWindow_EnterButtonClick);
            win.RegisterButton_Click += new EventHandler(enterWindow_RegisterButtonClick);
        }

        //виклик методів моделі для перевірки логіну та паролю
        void enterWindow_EnterButtonClick(object sender, System.EventArgs e)
        {
            Window window = win as Window;
            if (model.Validate(win.LoginText, win.PasswordText))
            {
                int result = model.CheckUser(win.LoginText, win.PasswordText);
                if (result > 0)
                {
                    MainChoice newWin = new MainChoice(result, window.Left, window.Top);
                    newWin.Show();
                    window.Close();
                }
                else win.SendMessage("Не правильный логин или пароль");
            }
            else win.SendMessage("Логин и пароль должны содержать минимум 5 символов");
        }

        //відкриття форми реєстрації нового користувача
        void enterWindow_RegisterButtonClick(object sender, System.EventArgs e) {
            var register = new Registration();
            register.ShowDialog();
        }
    }
}
