using ChatApp.Core;

namespace MagazynAM.MVVM.ViewModel {
    class ConfirmDeleteViewModel {

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public event Action<bool?> RequestClose;

        public ConfirmDeleteViewModel() {
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Confirm(object param) {
            RequestClose?.Invoke(true);
        }

        private void Cancel(object param) {
            RequestClose?.Invoke(false);
        }
    }
}
