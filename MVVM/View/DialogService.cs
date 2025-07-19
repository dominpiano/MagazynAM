using ChatApp.Core;
using MagazynekAM.MVVM.ViewModel;
using MagazynekAM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagazynekAM.MVVM.View {
    public class DialogService : IDialogService {

        private Type windowType;
        public DialogService(Type dialogType) {
            windowType = dialogType;
        }

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
