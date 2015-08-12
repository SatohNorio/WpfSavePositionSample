using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfSavePositionSample;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void SettingUnitTest()
        {
            MainWindow w = new MainWindow();
            w.Name = "FMainWindow";
            w.Left = 50;
            w.Top = 60;
            w.Width = 70;
            w.Height = 80;
            Setting.Instance.Save(w);
            var r = Setting.Instance.MainWindowBounds;
            Assert.IsTrue(50 == r.Left);
            Assert.IsTrue(60 == r.Top);
            Assert.IsTrue(70 == r.Width);
            Assert.IsTrue(80 == r.Height);
        }
    }
}
