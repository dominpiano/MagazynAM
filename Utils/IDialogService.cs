using MagazynekAM.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazynekAM.Utils {
    public interface IDialogService {
        bool? ShowDialog(bool isEditing = false, DataGridViewModel refViewModel = null);
    }
}
