using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Globalization;

namespace WpfSavePositionSample
{
    public sealed class Setting
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Setting()
        {
        }

        /// <summary>
        /// 唯一のSettingオブジェクト
        /// </summary>
        private static Setting _setting = new Setting();

        /// <summary>
        /// Settingオブジェクトにアクセスする唯一のインターフェース
        /// </summary>
        public static Setting Instance
        {
            get
            {
                return _setting;
            }
        }

        #region MainWindow位置、サイズ保存

        /// <summary>
        /// MainWindow位置、サイズ保存
        /// </summary>
        public Rect MainWindowBounds
        {
            get
            {
                return Properties.Settings.Default.MainWindow_Bounds;
            }
            private set
            {
                Properties.Settings.Default.MainWindow_Bounds = value;
                Properties.Settings.Default.Save();
            }
        }

        public void Save(Window savWindow)
        {
            var w = savWindow;
            Rect r = (w.WindowState == WindowState.Minimized) ?
                w.RestoreBounds : new Rect(w.Left, w.Top, w.Width, w.Height);

            if (w.Name == "FMainWindow")
            {
                MainWindowBounds = r;
            }
        }

        #endregion

        #region DataGridカラム位置、幅保存

        public List<ColumnSetting> DataGridColumns
        {
            get
            {
                return Properties.Settings.Default.DataGrid_Columns;
            }
            private set
            {
                Properties.Settings.Default.DataGrid_Columns = value;
                Properties.Settings.Default.Save();
            }
        }

        public void Save(DataGrid dg)
        {
            if (dg.Name == "FDataGrid")
            {
                var c = new List<ColumnSetting>();
                foreach (var item in dg.Columns)
                {
                    double width = (item.Width.IsAuto) ? -1.0 : item.Width.Value;
                    c.Add(new ColumnSetting() { DisplayIndex = item.DisplayIndex, Width = width });
                }
                DataGridColumns = c;
            }
        }

        #endregion

    }

    public class DoubleToDataGridLengthConverter : IValueConverter
    {
        /// <summary>
        /// Doubleの値をDataGridLengthに変換します。-1のときはDataGridLength.Autoに変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridLength l = 0;
            string d = value.ToString();
            switch (d)
            {
                case "-1":
                    l = DataGridLength.Auto;
                    break;
                default:
                    l = (double)value;
                    break;
            }
            return l;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Windowのサイズが変わったら保存する
    /// </summary>
    public class WindowStateSaveBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Closed += AssociatedObject_Closed;
            base.OnAttached();
        }

        /// <summary>
        /// Windowを閉じた時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Closed(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// プロパティを保存する
        /// </summary>
        private void Save()
        {
            var w = AssociatedObject as Window;
            Setting.Instance.Save(w);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Closed -= AssociatedObject_Closed;
            base.OnDetaching();
        }
    }

    /// <summary>
    /// DataGridColumnの設定保存用クラス
    /// </summary>
    [Serializable]
    public class ColumnSetting
    {
        public int DisplayIndex { get; set; }
        public double Width { get; set; }

        public override bool Equals(object obj)
        {
            var c = obj as ColumnSetting;
            if (this.DisplayIndex != c.DisplayIndex)
            {
                return false;
            }
            if (this.Width != c.Width)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// DataGridのカラム情報を保存します。
    /// </summary>
    public class DataGridColumnsSaveBehavior:Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.ColumnDisplayIndexChanged += AssociatedObject_ColumnDisplayIndexChanged;
            base.OnAttached();
        }

        /// <summary>
        /// プロパティを保存する
        /// </summary>
        private void Save()
        {
            var dg = AssociatedObject as DataGrid;
            Setting.Instance.Save(dg);
        }


        private void AssociatedObject_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            this.Save();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ColumnDisplayIndexChanged -= AssociatedObject_ColumnDisplayIndexChanged;
            base.OnDetaching();
        }
    }
}
