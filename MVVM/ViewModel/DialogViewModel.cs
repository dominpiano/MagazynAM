using ChatApp.Core;
using MagazynekAM.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MagazynekAM.MVVM.ViewModel {
    public class DialogViewModel : ObservableObject {

        //Props
        private string _nameText;
        public string NameText {
            get { return _nameText; }
            set {
                _nameText = value;
                OnPropertyChanged();
            }
        }

        private string _modelText;
        public string ModelText {
            get { return _modelText; }
            set {
                _modelText = value;
                OnPropertyChanged();
            }
        }

        private string _supplierText;
        public string SupplierText {
            get { return _supplierText; }
            set {
                _supplierText = value;
                UpdateSuggestion();
                OnPropertyChanged();
            }
        }

        private string _suggestedSupplierText;
        public string SuggestedSupplierText {
            get => _suggestedSupplierText;
            set {
                if (_suggestedSupplierText == value) return;
                _suggestedSupplierText = value;
                OnPropertyChanged();
            }
        }

        public int ItemQuantity;
        private string _quantityText;
        public string QuantityText {
            get {
                return _quantityText;
            }
            set {
                _quantityText = value;
                ItemQuantity = Convert.ToInt32(_quantityText);
                OnPropertyChanged();
            }
        }

        //Dialog Window Commands
        public RelayCommand? ConfirmCommand { get; set; }
        public RelayCommand? CancelCommand { get; set; }
        public RelayCommand? ChangeQtyCommand { get; set; }

        //Requesting closing window with desired outcome
        public event Action<bool?> RequestClose;

        private DataGridViewModel gridVM;

        private bool IsEditingItem;

        //Suggestions for suppliers
        public ObservableCollection<string> AllSuggestions { get; } = new ObservableCollection<string> {
            "ABB", "Advantech", "Allen-Bradley", "B&R", "Beckhoff", "Bosch Rexroth", "Delta Electronics", "Festo", "Hitachi", "Keyence", "Lenze", 
            "Mitsubishi Electric", "Modicon", "Omron", "Panasonic", "Phoenix Contact", "Schneider Electric", "Siemens", "Texas Instruments", "WAGO", "WEG", "Yokogawa"
        };

        public DialogViewModel(bool isEditing, DataGridViewModel refViewModel) {
            QuantityText = "1";
            IsEditingItem = isEditing;
            gridVM = refViewModel;

            //Inserting data if in editing mode
            if (IsEditingItem) {
                NameText = gridVM.CurrentlySelectedItem.Nazwa;
                ModelText = gridVM.CurrentlySelectedItem.Model;
                SupplierText = gridVM.CurrentlySelectedItem.Producent;
                QuantityText = gridVM.CurrentlySelectedItem.Ilosc.ToString();
            }

            //Initializing commands
            ConfirmCommand = new RelayCommand(ConfirmDialog, o => { return !string.IsNullOrEmpty(NameText); });
            CancelCommand = new RelayCommand(CancelDialog);
            ChangeQtyCommand = new RelayCommand(ChangeQty);
        }

        private void ConfirmDialog(object param) {
            StoreItem newItem = new StoreItem() {
                Nazwa = NameText,
                Model = ModelText,
                Producent = SupplierText,
                Ilosc = ItemQuantity
            };

            if (IsEditingItem) {
                newItem.ID = gridVM.CurrentlySelectedItem.ID;
                gridVM.EditItem(newItem);
            }
            else
                gridVM.AddItem(newItem);

            RequestClose?.Invoke(true);
        }

        private void CancelDialog(object param) {
            RequestClose?.Invoke(false);
        }

        private void ChangeQty(object param) {
            bool isAdd = bool.Parse((string)param);
            if (isAdd) {
                ItemQuantity++;
            }
            else {
                if (ItemQuantity > 0)
                    ItemQuantity--;
            }
            QuantityText = ItemQuantity.ToString();
        }

        private void UpdateSuggestion() {
            var suggestion = AllSuggestions.FirstOrDefault(s => s.StartsWith(SupplierText ?? "", StringComparison.InvariantCultureIgnoreCase));
            SuggestedSupplierText = suggestion ?? SupplierText;
        }

    }
}
