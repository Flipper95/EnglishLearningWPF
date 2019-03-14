using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    class GroupWordsPresenter
    {
        IGroupWordsView win = null;
        GroupWordsModel model = null;
        List<WordModel> oldWords;
        BorderPresenter border = null;

        public GroupWordsPresenter(IGroupWordsView window, int userId, int groupId)
        {
            win = window;
            border = new BorderPresenter(window);
            model = new GroupWordsModel(userId, groupId);
            //EventManager.RegisterRoutedEvent("ButtonMouseLeftClick", RoutingStrategy.Direct, typeof(RoutedEventHandler), GroupWordsPresenter);
            win.Button_MouseLeftButtonUp += new EventHandler(ButtonMouseLeftClick);
            win.Btn_Add_Click += new EventHandler(ButtonAddClick);
            win.Btn_Exit_Click += new EventHandler(ButtonExitClick);
            win.Btn_Search_Click += win_Btn_Search_Click;
            win.ChBx_Checked += win_ChBx_Checked;
            List<WordModel> words = model.Words;
            UpdateWindow();
            UpdateOldWords();
        }

        //оброблення події яка відповідає за виділення всіх слів на вивчення одним checkbox-ом
        void win_ChBx_Checked(object sender, EventArgs e)
        {
            bool value = win.GetCheckAll();
            foreach (WordModel el in model.Words) {
                el.OnLearning = value;
            }
            UpdateWindow();
        }

        //оброблення натиснення на кнопку пошуку
        void win_Btn_Search_Click(object sender, EventArgs e)
        {
            string[] search = win.GetSearchText();
            model.FindWords(search[0], search[1]);
            UpdateWindow();
            UpdateOldWords();
        }

        //підвантаження наступної чи попередньої сторінки з словами
        void ButtonMouseLeftClick(object sender, EventArgs e)
        {
            FrameworkElement btn = sender as FrameworkElement;
            //Button btn = sender as Button;
            if (btn.Name == "btn_Next")
            {
                if (model.SetPageIndex(true))
                {
                    UpdateWindow();
                    UpdateOldWords();
                }
            }
            else
            {
                if (btn.Name == "btn_Prev")
                {
                    if (model.SetPageIndex(false))
                    {
                        UpdateWindow();
                        UpdateOldWords();
                    }
                }
            }
        }

        //збереження внесених змін, можна вдосконалити код зберігаючи зміни в бд безпосередньо через модель слова (при зміні OnLearning)
        //сортуються старі слова та змінені користувачем, проводяться запити на зміни, оновлюються старі слова
        void ButtonAddClick(object sender, EventArgs e)
        {
            oldWords.Sort((w1, w2) => w1.WordId.CompareTo(w2.WordId));
            model.Words.Sort((w1, w2) => w1.WordId.CompareTo(w2.WordId));
            int addCount = 0;
            int deleteCount = 0;
            for (int i = 0; i < oldWords.Count; i++)
            {
                if (oldWords[i].OnLearning != model.Words[i].OnLearning)
                {
                    if (model.UpdateRow(model.Words[i].WordId, model.Words[i].OnLearning)) {
                        if (model.Words[i].OnLearning) addCount++;
                        else deleteCount++;
                    }
                }
            }
            UpdateOldWords();
            string message = "";
            if(addCount>0) message+="Добавлено "+addCount+" слов.";
            if(deleteCount>0)message+="Удалено "+deleteCount+" слов.";
            win.SendMessage(message);
        }

        void ButtonExitClick(object sender, EventArgs e)
        {
            if (win.SendMessage("Все изменения не будут сохранены, вы действительно хотите выйти?", "Уведомление"))
            {
                (win as Window).Close();
            }
        }

        //оновлення даних вікна, визначається чи будуть доступні кнопки наступної та попередньої сторінки
        void UpdateWindow()
        {
            if (model.GetMaxPageIndex() <= model.GetPageIndex()) win.EnableNext(false);
            else win.EnableNext(true);
            if (model.GetPageIndex() == 1) win.EnablePrev(false);
            else win.EnablePrev(true);

            win.SetData(model.Words);
        }

        //оновлення старих слів
        void UpdateOldWords()
        {
            oldWords = new List<WordModel>();
            for (int i = 0; i < model.Words.Count; i++)//win.DataGrid.Items.Count; i++)
            {
                oldWords.Add(model.Words[i].Clone() as WordModel);//(row.Item as WordModel).Clone() as WordModel);  //TO DO: do iclonable because this just copy link, this should be in updatewindow method i guess
            }
        }

    }
}
