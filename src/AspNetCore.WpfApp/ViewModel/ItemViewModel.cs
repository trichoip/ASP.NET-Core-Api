using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class ItemViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;
        public RelayCommand ShowHomeViewCommand => new RelayCommand(execute => CurrentView = new HomeVM());
        public RelayCommand ShowMenuViewCommand => new RelayCommand(execute => CurrentView = new MenuVM());
        public ObservableCollection<Item> Items { get; set; }

        public RelayCommand AddCommand => new RelayCommand(execute => AddItem());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteItem(), canExcute => SelectedItem != null);
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveItem(), canExcute => CanSave());

        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public ViewModelBase CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }

        public ItemViewModel()
        {
            CurrentView = new HomeVM();
            Items = new ObservableCollection<Item>();
            Items.Add(new Item { Name = "Item 1", Number = "1", Quantity = 1 });
            Items.Add(new Item { Name = "Item 2", Number = "2", Quantity = 3 });
        }

        private void AddItem()
        {
            Items.Add(new Item { Name = "Item 3", Number = "3", Quantity = 5 });
        }

        private void DeleteItem()
        {
            Items.Remove(SelectedItem);
        }
        private void SaveItem()
        {

        }

        private bool CanSave()
        {
            return true;
        }

    }
}
