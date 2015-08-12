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
    /// MainWindowに対応したViewModelを定義します。
    /// </summary>
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            var l = this.People;
            l.Add(new Person() { SeqNo = 1, Name = "佐藤", Age = 34, Comment = "これはダミーデータです。" });
            l.Add(new Person() { SeqNo = 2, Name = "大谷", Age = 40, Comment = "あああああ" });
            l.Add(new Person() { SeqNo = 3, Name = "山本", Age = 27, Comment = "いいい" });
            l.Add(new Person() { SeqNo = 4, Name = "田中", Age = 50, Comment = "" });
            l.Add(new Person() { SeqNo = 5, Name = "石田", Age = 24, Comment = "おおおおお。" });
            l.Add(new Person() { SeqNo = 6, Name = "宮本", Age = 17, Comment = "English is also ok." });
        }
        private ObservableCollection<Person> _people = new ObservableCollection<Person>();

        public ObservableCollection<Person> People
        {
            get
            {
                return this._people;
            }
        }

    }

    public class Person
    {
        public int SeqNo { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Comment { get; set; }
    }

}
