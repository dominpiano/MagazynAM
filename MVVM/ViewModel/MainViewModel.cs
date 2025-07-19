using ChatApp.Core;
using Google.Protobuf.Compiler;
using MagazynekAM.MVVM.Model;
using MagazynekAM.MVVM.View;
using MagazynekAM.Utils;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MagazynekAM.MVVM.ViewModel {
    public class MainViewModel : ObservableObject {

        public DataGridViewModel GridViewModel { get; } = new DataGridViewModel();
        public ICollectionView FilteredItems { get; set; }

        private string _searchText;
        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;
                OnPropertyChanged();
                FilteredItems.Refresh();
            }
        }

        private bool isDialOpen;
        public bool IsDialogOpen {
            get { return isDialOpen; }
            set {
                if (isDialOpen != value) {
                    isDialOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        //Main Window Commands
        public RelayCommand? AddCommand { get; set; }
        public RelayCommand? DeleteCommand { get; set; }
        public RelayCommand? EditCommand { get; set; }
        public RelayCommand? RecordDoubleClick { get; set; }

        private readonly IDialogService itemDialogService;
        private readonly IDialogService confirmDeleteDialogService;

        public MainViewModel(params IDialogService[] _dialServ) {
            itemDialogService = _dialServ[0];
            confirmDeleteDialogService = _dialServ[1];

            //Adding record
            AddCommand = new RelayCommand(OpenItemDialog);

            //Deleting record
            DeleteCommand = new RelayCommand(OpenConfirmDeleteDialog, CanOpenDeleteEditDialog);

            //Editing record
            EditCommand = new RelayCommand(OpenEditItemDialog, CanOpenDeleteEditDialog);

            //Double clicking is editing
            RecordDoubleClick = EditCommand;

            //Searching feature
            FilteredItems = CollectionViewSource.GetDefaultView(GridViewModel.GridStoreItems);
            FilteredItems.Filter = FilterItems;
        }

        private void OpenItemDialog(object param) {
            IsDialogOpen = true;
            bool? result = itemDialogService.ShowDialog(false, GridViewModel);
            IsDialogOpen = false;
        }

        private void OpenConfirmDeleteDialog(object param) {
            IsDialogOpen = true;
            bool? result = confirmDeleteDialogService.ShowDialog();
            IsDialogOpen = false;
            if (result == true)
                GridViewModel.DeleteItem();
        }

        private void OpenEditItemDialog(object param) {
            IsDialogOpen = true;
            bool? result = itemDialogService.ShowDialog(true, GridViewModel);
            IsDialogOpen = false;
        }

        private bool CanOpenDeleteEditDialog(object param) {
            if (GridViewModel.CurrentlySelectedItem == null)
                return false;
            return true;
        }

        private bool FilterItems(object obj) {
            if (obj is StoreItem item) {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return true;

                return item.Nazwa.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || item.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || item.Producent.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
