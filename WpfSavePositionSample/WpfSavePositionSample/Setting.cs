using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Interactivity;
using System.Windows.Input;

namespace WpfSavePositionSample
{
    /// <summary>
    /// ViewModdelにMainWindowを登録します。
    /// </summary>
    public class RegisterViewAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            this.Setting.Assign(this.AssociatedObject as MainWindow);
        }

        /// <summary>
        /// ViewModelを管理します
        /// </summary>
        public Setting Setting
        {
            get { return (Setting)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainWindowViewModelProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingProperty =
            DependencyProperty.Register("SettingProperty", typeof(Setting), typeof(RegisterViewAction), new PropertyMetadata(0));
    }

    public class Setting
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

        public void Assign(MainWindow w)
        {

        }

        public Rect MainWindowBounds
        {
            get
            {
                return Properties.Settings.Default.MainWindow_Bounds;
            }
            private set
            {
                Properties.Settings.Default.MainWindow_Bounds = value;
            }
        }

        public void Save(Window w)
        {
            Rect r = (w.WindowState == WindowState.Minimized) ?
                w.RestoreBounds : new Rect(w.Left, w.Top, w.Width, w.Height);

            if (w.Name == "FMainWindow")
            {
                MainWindowBounds = r;
            }
        }
    }

    /// <summary>
    /// Windowのサイズが変わったら保存する
    /// </summary>
    public class WindowStateSaveBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
            AssociatedObject.LocationChanged += AssociatedObject_LocationChanged;
            base.OnAttached();
        }

        /// <summary>
        /// Windowが移動したイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_LocationChanged(object sender, EventArgs e)
        {
            var w = AssociatedObject as Window;
            Setting.Instance.Save(w);
        }

        /// <summary>
        /// Windowのサイズが変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w = AssociatedObject as Window;
            Setting.Instance.Save(w);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
            AssociatedObject.LocationChanged -= AssociatedObject_LocationChanged;
            base.OnDetaching();
        }
    }
}
