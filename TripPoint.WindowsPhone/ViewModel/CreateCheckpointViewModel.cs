#region SDK Usings

using System.Windows;
using System.Windows.Input;

#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Utils;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : TripPointViewModelBase
    {
        public CreateCheckpointViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            CancelCreateCheckpointCommand = new RelayCommand(CancelCreateCheckpointAction);
        }

        public ICommand CreateCheckpointCommand { get; private set; }

        public ICommand CancelCreateCheckpointCommand { get; private set; }

        private void CreateCheckpointAction()
        {
            Logger.Log(this, "CreateCheckpoint");
        }

        private void CancelCreateCheckpointAction()
        {
            if ((Application.Current as App).RootFrame.CanGoBack)
                (Application.Current as App).RootFrame.GoBack();
        }
    }
}
