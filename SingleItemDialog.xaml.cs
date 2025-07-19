using ChatApp.Core;
using MagazynekAM.MVVM.ViewModel;
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

namespace MagazynekAM {
    /// <summary>
    /// Interaction logic for SingleItemDialog.xaml
    /// </summary>
    public partial class SingleItemDialog : Window {

        public bool IsEditing { get; set; }
        public DataGridViewModel refViewModel { get; set; }

        public SingleItemDialog() {
            InitializeComponent();
            Loaded += (s, e) => {
                var dialogVM = new DialogViewModel(IsEditing, refViewModel);
                DataContext = dialogVM;
                dialogVM.RequestClose += (result) => {
                    this.DialogResult = result;
                };
            };

        }
    }
}
