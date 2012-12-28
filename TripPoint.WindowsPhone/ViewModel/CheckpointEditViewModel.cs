using System.Windows.Input;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointEditViewModel : Base.TripPointViewModelBase
    {
        private ICheckpointRepository _checkpointRepository;
        
        public CheckpointEditViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            Checkpoint = new Checkpoint();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCheckpointCommand = new RelayCommand(SaveCheckpointAction);
            CancelEditCheckpointCommand = new RelayCommand(CancelEditCheckpointAction);
        }

        public Checkpoint Checkpoint { get; set; }

        public ICommand SaveCheckpointCommand { get; private set; }

        public ICommand CancelEditCheckpointCommand { get; private set; }

        private void SaveCheckpointAction()
        {
            _checkpointRepository.UpdateCheckpoint(Checkpoint);
            Navigator.GoBack();
        }

        private void CancelEditCheckpointAction()
        {
            Navigator.GoBack();
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _checkpointRepository = RepositoryFactory.CheckpointRepository;

            var checkpointID = GetCheckpointID(e.View);
            InitializeCheckpoint(checkpointID);
        }

        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null) return -1;

            var parameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(parameter);
        }

        private void InitializeCheckpoint(int checkpointID)
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(checkpointID);

            if (checkpoint != null)
                Checkpoint = checkpoint;
        }
    }
}
