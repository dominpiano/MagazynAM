using ChatApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazynekAM.MVVM.ViewModel {
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
