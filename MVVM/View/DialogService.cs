using MagazynAM.MVVM.ViewModel;
using MagazynAM.Utils;
using System.Windows;

namespace MagazynAM.MVVM.View {
    public class DialogService : IDialogService {

        private Type windowType;
        public DialogService(Type dialogType) {
            windowType = dialogType;
        }

        //Generic function to create an instance of a window with given type (can be ItemDialog or ConfirmDeleteDialog)
        public bool? ShowDialog(bool isEditing = false, DataGridViewModel mandViewModel = null) {
            var dialogWindow = (Window) Activator.CreateInstance(windowType)!;
            dialogWindow.Owner = Application.Current.MainWindow;
            dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dialogWindow is SingleItemDialog window) {
                window.IsEditing = isEditing;
                window.refViewModel = mandViewModel;
            }
            return dialogWindow.ShowDialog();
        }
    }
}
