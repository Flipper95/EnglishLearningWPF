using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SystemForEnglishLearning
{
    //статичний клас який повертає створені компоненти, створений для зменшення повторення коду при генерації вмісту вікна
    static class DynamicElements
    {
        public static Grid CreateGrid(int columns, int rows, GridUnitType columnType, GridUnitType rowType)
        {
            Grid grid = new Grid();
            for (int i = 0; i < columns; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, columnType);
                grid.ColumnDefinitions.Add(col);
            }
            for (int i = 0; i < rows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, rowType);
                grid.RowDefinitions.Add(row);
            }
            return grid;
        }

        public static Viewbox CreateViewBox(int gridRow, int gridColumn, int gridColumnSpan, int gridRowSpan)
        {
            Viewbox viewBox = new Viewbox();
            SetRowColumnProperties(viewBox, gridRow, gridColumn, gridColumnSpan, gridRowSpan);
            return viewBox;
        }

        public static Viewbox CreateViewBox(double height)
        {
            Viewbox viewBox = new Viewbox();
            viewBox.Height = height;
            return viewBox;
        }

        public static Label CreateLabel(string content, int fontSize)
        {
            Label lb = new Label();
            lb.Content = content;
            lb.FontSize = fontSize;
            return lb;
        }

        public static void SetRowColumnProperties(UIElement el, int gridRow, int gridColumn, int gridColumnSpan, int gridRowSpan)
        {
            Grid.SetRow(el, gridRow);
            Grid.SetColumn(el, gridColumn);
            Grid.SetColumnSpan(el, gridColumnSpan);
            Grid.SetRowSpan(el, gridRowSpan);
        }

        //public static TextBlock CreateTextBlock(string text) {
        //    TextBlock tb = new TextBlock();
        //    tb.Text = text;
        //    return tb;
        //}

        public static CheckBox CreateCheckBox(bool isChecked) {
            CheckBox box = new CheckBox();
            box.IsChecked = isChecked;
            return box;
        }

        public static Border CreateBorder(Style bordStyle) {
            Border bord = new Border();
            bord.Style = bordStyle;
            return bord;
        }

        public static Border CreateBorder(Style bordStyle, object word)
        {
            Border bord = new Border();
            bord.Style = bordStyle;
            bord.Tag = word;
            return bord;
        }

        public static Border CreateBorder(Style bordStyle, int gridRow, int gridColumn, int gridColumnSpan, int gridRowSpan)
        {
            Border bord = CreateBorder(bordStyle);
            SetRowColumnProperties(bord, gridRow, gridColumn, gridColumnSpan, gridRowSpan);
            return bord;
        }

        public static Button CreateButton(string content) {
            Button btn = new Button();
            btn.Content = content;
            return btn;
        }

        public static Image CreateImage(byte[] byteImage, int gridRow, int gridColumn, int gridColumnSpan, int gridRowSpan, double height)
        {
            Image img = ConvertByteToImage(byteImage);
            SetRowColumnProperties(img, gridRow, gridColumn, gridColumnSpan, gridRowSpan);
            img.Height = height;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            return img;
        }

        static Image ConvertByteToImage(byte[] byteImage)
        {
            Image img = new Image();
            using (var ms = new System.IO.MemoryStream(byteImage))
            {
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                img.Source = bi;
            }
            return img;
        }

        public static Viewbox CreateViewBoxLabel(string LabelContent, int wordId)
        {
            Viewbox vB = new Viewbox();
            Label lB = new Label();
            lB.Content = LabelContent;
            vB.Child = lB;
            vB.Tag = wordId;
            return vB;
        }

    }
}
