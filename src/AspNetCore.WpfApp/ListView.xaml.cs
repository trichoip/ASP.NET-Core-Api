using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : Window
    {
        public bool IsSort;
        public ListView()
        {

            InitializeComponent();
            cbProperties.ItemsSource = typeof(User).GetProperties().Select(u => u.Name);
            IsSort = false;

            List<User> items = new List<User>
            {
              new User() { Name = "atrichoip", Age = 15, Mail = "gg.mail" ,Sex = SexType.Male },
              new User() { Name = "John Doe", Age = 2, Mail = "gg1.mail",Sex = SexType.Famale },
              new User() { Name = "admin", Age = 12, Mail = "gg2.mail",Sex = SexType.Male },
              new User() { Name = "user", Age = 15, Mail = "gg22.mail",Sex = SexType.Male }
            };

            lvUser.ItemsSource = items;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUser.ItemsSource);

            // group
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Sex");
            view.GroupDescriptions.Add(groupDescription);

            // sort
            //SortDescription sortAgeDescription = new SortDescription("Age", ListSortDirection.Ascending);
            //SortDescription sortNameDescription = new SortDescription("Name", ListSortDirection.Descending);
            //view.SortDescriptions.Add(sortAgeDescription);
            //view.SortDescriptions.Add(sortNameDescription);

            // filter
            //view.Filter = UserFilter;

        }

        // sort
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader header = sender as GridViewColumnHeader;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUser.ItemsSource);
            view.SortDescriptions.Clear();
            SortDescription sortDescription;
            if (IsSort)
            {
                sortDescription = new SortDescription(header.Content.ToString(), ListSortDirection.Ascending);

            }
            else
            {
                sortDescription = new SortDescription(header.Content.ToString(), ListSortDirection.Descending);
            }
            view.SortDescriptions.Add(sortDescription);
            IsSort = !IsSort;

        }

        // filter
        private bool UserFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as User).Name.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            //return (item as User).Name.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase);
        }

        private bool NameFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as User).Name.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;

        }

        private bool MailFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as User).Mail.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;

        }

        public Predicate<object> GetFilter()
        {
            switch (cbProperties.SelectedItem as string)
            {
                case "Name":
                    return NameFilter;
                case "Mail":
                    return MailFilter;
                default:
                    return NameFilter;
            }

        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            //CollectionViewSource.GetDefaultView(lvUser.ItemsSource).Refresh();

            if (!String.IsNullOrEmpty(txtFilter.Text))
                lvUser.Items.Filter = GetFilter();
            else
                lvUser.Items.Filter = null;

        }

        private void cbProperties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvUser.Items.Filter = GetFilter();
        }
    }
    public enum SexType
    {
        Male, Famale
    }
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Mail { get; set; }

        public SexType Sex { get; set; }

    }
}
