using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointEditViewModel : Base.TripPointViewModelBase
    {
        private ICheckpointRepository _checkpointRepository;
        
        public CheckpointEditViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
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
            InitializeCheckpoint(TripPointConvert.ToInt32(GetParameter(e.View, "checkpointID")));
        }

        private void InitializeCheckpoint(int checkpointID)
        {
            Checkpoint = _checkpointRepository.FindCheckpoint(checkpointID) ?? new Checkpoint();
        }
    }
}
