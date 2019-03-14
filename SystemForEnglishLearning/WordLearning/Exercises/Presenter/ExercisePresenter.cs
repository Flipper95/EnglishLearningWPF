using System;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
//using SystemForEnglishLearning.WordLearning.Dictionary;
using VoiceRSS_SDK;
using System.IO;

namespace SystemForEnglishLearning.WordLearning.Exercises
{
    abstract class ExercisePresenter
    {
        //protected Equivalent win = null;
        protected IEquivalentView win = null;
        BorderPresenter border = null;
        //protected TranslateModel model = null;
        protected WordModel answer;
        object block = new object();
        protected List<int> rightAnswer;
        Random rand;

        public ExercisePresenter(IEquivalentView window, int userId)
        {
            rand = new Random();
            win = window;
            border = new BorderPresenter(win);
            rightAnswer = new List<int>();
            win.Image_MouseLeftButtonDown += new EventHandler(ImageMouseLeftClick);
            win.MediaEnded += new EventHandler(MediaEnded);
            //win.Variant_MouseLeftButtonDown += new EventHandler(VariantMouseLeftDown);
            win.Next_MouseLeftButtonDown += new EventHandler(NextMouseLeftDown);
            win.Complete_MouseLeftButtonDown += new EventHandler(CompleteMouseLeftDown);
            win.Window_Closing += new EventHandler(WindowClosing);
        }

        void WindowClosing(object sender, EventArgs e)
        {
            border = null;
            win.Image_MouseLeftButtonDown -= new EventHandler(ImageMouseLeftClick);
            win.MediaEnded -= new EventHandler(MediaEnded);
            win.Next_MouseLeftButtonDown -= new EventHandler(NextMouseLeftDown);
            win.Complete_MouseLeftButtonDown -= new EventHandler(CompleteMouseLeftDown);
            win.Window_Closing -= new EventHandler(WindowClosing);
            ClearGrid();
            answer = null;
            win = null;
        }

        protected abstract void ClearGrid();
        //{
        //    MediaElement mediaEl = (MediaElement)LogicalTreeHelper.FindLogicalNode(win.DataGrid, "MediaEl");
        //    mediaEl.Source = null;
        //    win.DataGrid.Children.Clear();
        //}

        //створення потоку для програвання озвучування слова
        void ImageMouseLeftClick(object sender, EventArgs e)
        {
            Thread thread = new Thread(Play);
            thread.IsBackground = true;
            thread.Start();
        }

        protected void VariantClick(object sender, string value1, string value2, int wordId)
        {
            Window window = win as Window;
            Border bord = sender as Border;
            if (value1 != value2)
            {
                bord.Background = window.FindResource("BrushRed") as Brush;
            }
            else
            {
                bord.Background = window.FindResource("BrushGreen") as Brush;
                rightAnswer.Add(wordId);
            }
            for (int i = 0; i < 5; i++)
            {
                Border border = (Border)LogicalTreeHelper.FindLogicalNode(window, "Bord" + i);
                //if (value1 == value2)
                //{
                //    border.Background = window.FindResource("BrushGreen") as Brush;
                //}
                border.Style = null;
            }
            //win.Variant_MouseLeftButtonDown -= new EventHandler(VariantMouseLeftDown);
            NextCompleteVisibility(window);
        }

        void NextCompleteVisibility(DependencyObject linkedObj) {
            Border nextBord = (Border)LogicalTreeHelper.FindLogicalNode(linkedObj, "NextBord");
            Border completeBord = (Border)LogicalTreeHelper.FindLogicalNode(linkedObj, "CompleteBord");
            if (nextBord != null)
            {
                nextBord.Visibility = Visibility.Visible;
            }
            if (completeBord != null)
            {
                completeBord.Visibility = Visibility.Visible;
            }
        }

        protected WordModel GetChoosenWordModel(object content){
            return content as WordModel;
        }

        protected abstract void NextMouseLeftDown(object sender, EventArgs e);

        protected abstract void CompleteMouseLeftDown(object sender, EventArgs e);

        //встановлення голосу
        protected void SetVoice()
        {
            Thread thread = new Thread(WriteFile);
            thread.IsBackground = true;
            thread.Start();
        }

        //запис файлу
        void WriteFile()
        {
            Window window = win as Window;
            string fileName = "..\\voice.mp3";
            System.IO.FileStream _FileStream;
            try
            {
                using (_FileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    if (answer != null)
                    {
                        _FileStream.Write(answer.Voice, 0, answer.Voice.Length);
                        window.Dispatcher.BeginInvoke(new ThreadStart(delegate()
                        {
                            ((MediaElement)LogicalTreeHelper.FindLogicalNode(window, "MediaEl")).Source = new Uri(@"..\\voice.mp3", UriKind.Relative);
                        }));
                    }
                    _FileStream.Close();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
                WriteFile();
            }
        }

        void Play()
        {
            Window window = win as Window;
            lock (block)
            {
                window.Dispatcher.BeginInvoke(new ThreadStart(delegate()
                {
                    ((MediaElement)LogicalTreeHelper.FindLogicalNode(window, "MediaEl")).Play();
                }));
            }
        }

        void MediaEnded(object sender, EventArgs e)
        {
            MediaElement mediaEl = sender as MediaElement;
            mediaEl.Stop();
        }

        //далі йде код зі створення елементів...
        protected void AddVoiceBord()
        {
            Window window = win as Window;
            Style imgStyle = window.FindResource("ImageStyle") as Style;
            Style bordImgStyle = window.FindResource("BorderImageStyle") as Style;
            Border bord = DynamicElements.CreateBorder(bordImgStyle, 3, 0, 1, 2);
            bord.Name = "Speech";
            bord.HorizontalAlignment = HorizontalAlignment.Center;
            Image image = new Image();
            image.Style = imgStyle;
            image.HorizontalAlignment = HorizontalAlignment.Center;
            bord.Child = image;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            grid.Children.Add(bord);
        }

        protected MediaElement CreateMediElement(Style style)
        {
            MediaElement media = new MediaElement();
            DynamicElements.SetRowColumnProperties(media, 0, 0, 1, 1);
            media.Style = style;
            media.Name = "MediaEl";
            media.LoadedBehavior = MediaState.Manual;
            return media;
        }

        /// <summary>
        /// Add transcription, voice, part of speech elements
        /// </summary>
        protected void AddTVPS()
        {
            Viewbox vB;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(win as Window, "DataGrid");
            if (answer.Transcription != null)
            {
                vB = DynamicElements.CreateViewBoxLabel(answer.Transcription, answer.WordId);
                DynamicElements.SetRowColumnProperties(vB, 5, 0, 1, 1);
                grid.Children.Add(vB);
                //win.DataGrid.Children.Add(vB);
            }
            if (answer.PartOfSpeech != null)
            {
                vB = DynamicElements.CreateViewBoxLabel(answer.PartOfSpeech, answer.WordId);
                vB.ToolTip = "Часть речи";
                DynamicElements.SetRowColumnProperties(vB, 7, 0, 1, 2);
                grid.Children.Add(vB);
                //win.DataGrid.Children.Add(vB);
            }
            AddVoice();
        }

        protected void AddVoice() {
            Window window = win as Window;
            if (answer.Voice.Length > 1)
            {
                AddVoiceBord();
            }
            else
            {
                try
                {
                    VoiceAPI voice = new VoiceAPI();
                    answer.Voice = voice.UploadVoice(answer.Word);
                    if (answer.Voice.Length > 1)
                    {
                        AddVoiceBord();
                    }
                }
                catch { } //TODO:maybe I need to wrote some message
            }
            MediaElement media = CreateMediElement(window.FindResource("MediaStyle") as Style);
            media.Volume = 1;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            grid.Children.Add(media);
            SetVoice();
        }

        /// <summary>
        /// Add content elements, difference between elements content which are determined by type
        /// </summary>
        /// <param name="type"></param>
        //protected abstract void AddContent(string type);

        protected void AddNextComplete(int WordsCount)
        {
            Border nextBord;
            Style bordStyle;
            Window window = win as Window;
            if (WordsCount != 0)
            {
                bordStyle = window.FindResource("NextStyle") as Style;
                nextBord = DynamicElements.CreateBorder(bordStyle, 0);//CreateOwnBorder(bordStyle, 0);
                nextBord.Margin = new Thickness(0, 4, 4, 4);
                nextBord.Child = DynamicElements.CreateViewBoxLabel("Дальше", 0);
                nextBord.Name = "NextBord";
            }
            else
            {
                bordStyle = window.FindResource("CompleteStyle") as Style;
                nextBord = DynamicElements.CreateBorder(bordStyle, 0);//CreateOwnBorder(bordStyle, 0);
                nextBord.Margin = new Thickness(0, 4, 4, 4);
                nextBord.Child = DynamicElements.CreateViewBoxLabel("Завершить", 0);
                nextBord.Name = "CompleteBord";
            }
            DynamicElements.SetRowColumnProperties(nextBord, 7, 1, 1, 1);
            nextBord.Visibility = Visibility.Collapsed;
            nextBord.Background = window.FindResource("BrushBlue") as Brush;
            Grid grid = (Grid)LogicalTreeHelper.FindLogicalNode(window, "DataGrid");
            grid.Children.Add(nextBord);
        }

    }

}
