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
    /// Interaction logic for ValueConverter.xaml
    /// </summary>
    public partial class ValueConverter : Window
    {
        public ValueConverter()
        {
            InitializeComponent();
        }
    }
    public class AgeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime? selectedDate = value as DateTime?;
            if (selectedDate == null)
                return null;
            return selectedDate.Value.Year;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int year = (int)value;
            return new DateTime(year, 1, 1);
        }
    }
}
