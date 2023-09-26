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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ListBox.xaml
    /// </summary>
    public partial class ListBox : Window
    {
        public ListBox()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //cb1
            cb1.ItemsSource = new List<string>() { "a", "b" };
            cb1.SelectedIndex = 0;
            //cb1.SelectedItem = "a";
            //cb1.SelectedValue = "a";

            //cb2
            cb2.ItemsSource = new List<Person1>() {
                new Person1() { Name = "tri1", Age = 1 },
                new Person1() { Name = "tri2", Age = 2 },
                new Person1() { Name = "tri3", Age = 3 },
                new Person1() { Name = "tri4", Age = 4 },
                new Person1() { Name = "tri5", Age = 5 }
            };

            cb2.DisplayMemberPath = "Age";
            cb2.SelectedValuePath = "Name";
            cb2.SelectedValue = "tri3";

            //cb3
            cb3.ItemsSource = new List<Person1>() {
                new Person1() { Name = "tri1", Age = 1 },
                new Person1() { Name = "tri2", Age = 2 },
                new Person1() { Name = "tri3", Age = 3 },
                new Person1() { Name = "tri4", Age = 4 },
                new Person1() { Name = "tri5", Age = 5 }
            };
            //cb3.SelectedValuePath = "Name";
            //cb3.SelectedValue = "tri3";

            //cb4
            cb4.ItemsSource = new List<Person1>() {
                new Person1() { Name = "tri1", Age = 1 },
                new Person1() { Name = "tri2", Age = 2 },
                new Person1() { Name = "tri3", Age = 3 },
                new Person1() { Name = "tri4", Age = 4 },
                new Person1() { Name = "tri5", Age = 5 }
            };
            cb4.SelectedValuePath = "Name";
            cb4.SelectedValue = "tri3";

            //cbColor
            cbColor.ItemsSource = typeof(Colors).GetProperties();
        }
    }
    public class Person1
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
