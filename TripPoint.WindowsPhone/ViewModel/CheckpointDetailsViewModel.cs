using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointDetailsViewModel : TripPointViewModelBase
    {
        public CheckpointDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            Checkpoint = new Checkpoint();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditCheckpointCommand = new RelayCommand(EditCheckpointAction);
            DeleteCheckpointCommand = new RelayCommand(DeleteCheckpointAction);
        }

        public Checkpoint Checkpoint { get; private set; }

        public ICommand EditCheckpointCommand { get; private set; }

        public ICommand DeleteCheckpointCommand { get; private set; }

        private void EditCheckpointAction()
        {
            Logger.Log(this, "Edit checkpoint");
        }

        private void DeleteCheckpointAction()
        {
            var caption = "";
            var message = "Are you sure you want to delete this checkpoint?";

            var userDecision = MessageBox.Show(message, caption, MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK)
                DeleteCheckpoint();
        }

        private void DeleteCheckpoint()
        {
            
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Logger.Log(this, "checkpoint ID: {0}", e.View.TryGetQueryStringParameter("checkpointID"));

            var checkpointID = GetCheckpointID(e.View);
            var checkpoint = GetCheckpoint(checkpointID);

            if (checkpoint != null)
                Checkpoint = checkpoint;
        }

        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null)
                return -1;

            var parameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(parameter);
        }

        private Checkpoint GetCheckpoint(int checkpointID)
        {
            return RepositoryFactory.CheckpointRepository.FindCheckpoint(checkpointID);
        }
    }
}
