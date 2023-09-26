using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for XmlWpf.xaml
    /// </summary>
    public partial class XmlWpf : Window
    {
        public XmlWpf()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PersonHook p1 = new PersonHook() { Name = "David", Age = 30 };
            var xs = new XmlSerializer(typeof(PersonHook));
            using Stream s1 = File.Create("person.xml");
            xs.Serialize(s1, p1);
            s1.Close();
            using Stream s2 = File.OpenRead("person.xml");
            var p2 = (PersonHook)xs.Deserialize(s2);
            string xmlData = File.ReadAllText("person.xml");
            this.DataContext = xmlData;
            s2.Close();
        }
    }

    [XmlRoot("Candidate")]
    public class PersonHook
    {
        [XmlElement("FirstName")]
        public string Name { get; set; }

        [XmlElement("RoughAge")]
        public int Age { get; set; }
    }
}
