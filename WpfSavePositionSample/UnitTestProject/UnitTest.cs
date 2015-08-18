using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfSavePositionSample;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void MainWindowBoundsTest()
        {
            Window w = new Window();
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

        [TestMethod]
        public void DataGridColumnsTest()
        {
            var dg = new DataGrid();
            dg.Name = "FDataGrid";
            var c1 = new ColumnSetting() { DisplayIndex = 0, Width = -1 };
            var c2 = new ColumnSetting() { DisplayIndex = 1, Width = 100 };
            var c3 = new ColumnSetting() { DisplayIndex = 2, Width = 200 };
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c1.DisplayIndex, Width = c1.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c2.DisplayIndex, Width = c2.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c3.DisplayIndex, Width = c3.Width });

            Setting.Instance.Save(dg);
            var cols = Setting.Instance.DataGridColumns;
            var c = cols[0];
            Assert.IsTrue(c1.Equals(c));
            c = cols[1];
            Assert.IsTrue(c2.Equals(c));
            c = cols[2];
            Assert.IsTrue(c3.Equals(c));
        }

        [TestMethod]
        public void DoubleToDataGridLengthConverterTest()
        {
            var cvt = new DoubleToDataGridLengthConverter();
            double v = 1000000;
            DataGridLength l = (DataGridLength)cvt.Convert(v, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
            Assert.IsTrue(l.Value == v);

            v = 0;
            l = (DataGridLength)cvt.Convert(v, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
            Assert.IsTrue(l.Value == v);

            v = -1;
            l = (DataGridLength)cvt.Convert(v, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
            Assert.IsTrue(DataGridLength.Auto == l);

        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void DoubleToDataGridLengthConverterConvertBackTest()
        {
            var cvt = new DoubleToDataGridLengthConverter();
            DataGridLength l = DataGridLength.Auto;

            double v = (double)cvt.ConvertBack(l, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
        }

        [TestMethod]
        public void WindowStateSaveBehaviorTest()
        {
            var bh = new WindowStateSaveBehavior();
            var w = new Window();
            w.Name = "FMainWindow";
            bh.Attach(w);
            w.ShowDialog();
            w.Close();
            bh.Detach();

        }

        [TestMethod]
        public void DataGridColumnsSaveBehaviorTest()
        {
            var bh = new DataGridColumnsSaveBehavior();
            var dg = new DataGrid();
            dg.Name = "FDataGrid";
            dg.Columns.Add(new DataGridTemplateColumn());
            dg.Columns.Add(new DataGridTemplateColumn());
            dg.Columns.Add(new DataGridTemplateColumn());
            bh.Attach(dg);
            dg.Columns[0].DisplayIndex = 1;
            bh.Detach();

        }
    }
}
