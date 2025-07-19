using ChatApp.Core;
using MagazynAM.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagazynAM {
    /// <summary>
    /// Interaction logic for SingleItemDialog.xaml
    /// </summary>
    public partial class SingleItemDialog : Window {

        public bool IsEditing { get; set; }
        public DataGridViewModel refViewModel { get; set; }

        public SingleItemDialog() {
            InitializeComponent();
            Loaded += (s, e) => {
                //After it's loaded we can give it a data context
                var dialogVM = new DialogViewModel(IsEditing, refViewModel);
                DataContext = dialogVM;
                dialogVM.RequestClose += (result) => {
                    this.DialogResult = result;
                };
            };

        }
    }
}
