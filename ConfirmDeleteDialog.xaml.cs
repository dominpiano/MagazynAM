using MagazynAM.MVVM.ViewModel;
using System.Windows;

namespace MagazynAM {
    /// <summary>
    /// Interaction logic for ConfirmDeleteDialog.xaml
    /// </summary>
    public partial class ConfirmDeleteDialog : Window {
        public ConfirmDeleteDialog() {
            InitializeComponent();
            var vm = new ConfirmDeleteViewModel();
            this.DataContext = vm;
            vm.RequestClose += (result) => {
                DialogResult = result;
                Close();
            };
        }
    }
}
