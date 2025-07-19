using MagazynAM.MVVM.ViewModel;

namespace MagazynAM.Utils {
    public interface IDialogService {
        bool? ShowDialog(bool isEditing = false, DataGridViewModel refViewModel = null);
    }
}
