using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SystemForEnglishLearning.WordLearning.Dictionary
{
    /// <summary>
    /// Interaction logic for GroupWords.xaml
    /// </summary>
    public partial class GroupWords : Window, IGroupWordsView
    {
        public GroupWords()
        {
            InitializeComponent();
            //new Dictionary.GroupWordsPresenter(this, 1, 
        }

        public GroupWords(int userId, int groupId)
            : this()
        {
            new GroupWordsPresenter(this, userId, groupId);
        }

        public event EventHandler Button_MouseLeftButtonUp = null;
        private void btn_Next_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            Button_MouseLeftButtonUp(sender, e);
        }

        public event EventHandler Btn_Add_Click = null;
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Btn_Add_Click(sender, e);
            //int count = 0;
            //for (int i = 0; i < Dg.Items.Count; i++)
            //{
            //    DataGridRow row = (DataGridRow)Dg.ItemContainerGenerator
            //                                               .ContainerFromIndex(i);
            //    WordModel mod = row.Item as WordModel;
            //    if (mod.OnLearning) count++;
            //}
            //MessageBox.Show(count.ToString());
        }

        public event EventHandler Btn_Exit_Click = null;
        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Btn_Exit_Click(sender, e);
        }

        public event EventHandler Border_MouseEnter = null;
        private void Border_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Border_MouseEnter(sender, e);
        }

        public event EventHandler Border_MouseLeave = null;
        private void Border_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Border_MouseLeave(sender, e);
        }

        //public event EventHandler Button_MouseLeftButtonDown = null;
        //private void Button_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        //{
        //    Button_MouseLeftButtonDown(sender, e);
        //}

        //private BindingSource customersBindingSource = new BindingSource();

        public void SetData<T>(List<T> data)
        {
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource  = data;
            //DataGrid.ItemsSource = new ObservableCollection<T>(data);
            //BindingList<T> bindList = new BindingList<T>(data);
            //DataGrid.ItemsSource = data;
            //DataGrid.DataContext = null;
            //DataGrid.DataContext = data;
        }

        public void EnablePrev(bool enable)
        {
            btn_Prev.IsEnabled = enable;
        }

        public void EnableNext(bool enable)
        {
            btn_Next.IsEnabled = enable;
        }

        public event EventHandler Btn_Search_Click = null;
        private void btn_Search_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            Btn_Search_Click(sender, e);
        }

        public string[] GetSearchText()
        {
            string[] result = new string[2];
            result[0] = this.tb_Word.Text;
            result[1] = this.tb_Translate.Text;
            return result;
        }

        public bool GetCheckAll() {
            return Convert.ToBoolean(CheckAll.IsChecked);
        }

        public event EventHandler ChBx_Checked = null;
        private void CheckAll_Checked_1(object sender, RoutedEventArgs e)
        {
            ChBx_Checked(sender, e);
        }

        public void SendMessage(string message){
            MessageBox.Show(message);
        }

        public bool SendMessage(string message, string title)
        {
            if (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                return true;
            else return false;
        }

    }
}
