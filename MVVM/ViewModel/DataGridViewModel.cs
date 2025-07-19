using ChatApp.Core;
using MagazynAM.Utils;
using MagazynAM.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MagazynAM.MVVM.ViewModel {
    public class DataGridViewModel : ObservableObject {

        //Modifyable collection of items and corresponding filterable collection view
        public ObservableCollection<StoreItem> GridStoreItems { get; } = new ObservableCollection<StoreItem>();
        public ICollectionView ItemsToShow { get; set; }

        //Properties
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

        //Main methods
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

            //Refreshing item collection
            GridStoreItems.Clear();
            foreach (var item in items)
                GridStoreItems.Add(item);
        }
    }
}
