using MagazynAM.MVVM.View;
using MagazynAM.MVVM.ViewModel;
using System.Windows;

namespace MagazynAM;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    public MainWindow() {
        InitializeComponent();
        //Initialization of Dialog Services
        var itemDialServ = new DialogService(typeof(SingleItemDialog));
        var confirmDialServ = new DialogService(typeof(ConfirmDeleteDialog));
        var main = new MainViewModel(itemDialServ, confirmDialServ);
        this.DataContext = main;
    }
}