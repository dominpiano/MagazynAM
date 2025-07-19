using ChatApp.Core;
using MagazynAM.Utils;
using MagazynekAM.MVVM.Model;
using MagazynekAM.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagazynekAM.MVVM.ViewModel {
    public class DataGridViewModel : ObservableObject {

        public ObservableCollection<StoreItem> GridStoreItems { get; } = new ObservableCollection<StoreItem>();
        
        public ICollectionView ItemsToShow { get; set; }

        private StoreItem currSelItem;
        public StoreItem CurrentlySelectedItem {
            get { return currSelItem; }
            set {
                currSelItem = value;
                OnPropertyChanged();
            }
        }

        public JSONHandler jsonHandler;

        public DataGridViewModel() {
            jsonHandler = new JSONHandler();
            LoadAllItems();
        }

        public void AddItem(StoreItem addItem) {
            jsonHandler.AddItemToJSON(addItem);
            LoadAllItems();
        }

        public void DeleteItem() {
            jsonHandler.RemoveItemFromJSON(CurrentlySelectedItem);
            LoadAllItems();
        }

        public void EditItem(StoreItem changeItem) {
            jsonHandler.EditItemInJSON(changeItem);
            LoadAllItems();
        }

        public void LoadAllItems() {

            var items = jsonHandler.LoadItemsFromJSON();
            if (items == null) return;

            GridStoreItems.Clear();
            foreach (var item in items)
                GridStoreItems.Add(item);
        }

    }
}
