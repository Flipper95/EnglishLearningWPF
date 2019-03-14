using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace SystemForEnglishLearning.Lections
{
    class LectionPresenter
    {
        ILectionView window = null;
        LectionModel model = null;
        List<LectionsModel> lections = null;
        int userId;

        public LectionPresenter(ILectionView win, int userId, int lectionId, List<LectionsModel> lections) {
            window = win;
            model = new LectionModel(lectionId, userId);
            lections.Sort((w1, w2) =>  w1.Name.CompareTo(w2.Name));
            this.lections = lections;
            this.userId = userId;
            window.SetDataSideMenu(lections);
            SetTestControl();
            SetLectionContent();
            window.lectionBtn_Click += lectionBtn_Click;
            window.testBtn_Click += window_testBtn_Click;
        }

        //вибір тестування за пройденою лекцією
        void window_testBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Window win = window as System.Windows.Window;
            Tests.Test testWin = new Tests.Test(userId, model.Test, win.Left, win.Top, win.WindowState);
            testWin.ShowDialog();
        }

        void lectionBtn_Click(object sender, EventArgs e)
        {
            int id = window.GetNewLectionId(sender);
            model.SetLectionId(id);
            SetLectionContent();
            SetTestControl();
        }

        //встановлення вмісту лекції, отримання масиву байтів з моделі та створення з них документу
        void SetLectionContent() {
            byte[] byteContent = model.GetLection().Text;
            if (byteContent != null)
            {
                var content = CreateFile(byteContent).GetFixedDocumentSequence();
                window.SetMainData(content);
            }
            else
            {
                window.ContentNullException("Лекция по данной теме отсутствует! Пожалуйста выберите другую");
            }
        }

        //тест відображається якщо він існує за вибраною темою
        void SetTestControl() {
            if (model.Test != null)
            {
                window.TestControlEnabled(true);
            }
            else
            {
                window.TestControlEnabled(false);
            }
        }

        //створення файлу
        XpsDocument CreateFile(byte[] Content)
        {
            byte[] buffer = Content;
            MemoryStream newStream = new MemoryStream(buffer);
            var package = System.IO.Packaging.Package.Open(newStream);
            string inMemoryPackageName = string.Format("memorystream://{0}.xps", Guid.NewGuid());
            Uri packageUri = new Uri(inMemoryPackageName);
            System.IO.Packaging.PackageStore.AddPackage(packageUri, package);
            XpsDocument doc = new XpsDocument(package, System.IO.Packaging.CompressionOption.Maximum, inMemoryPackageName);
            return doc;
        }

    }
}
