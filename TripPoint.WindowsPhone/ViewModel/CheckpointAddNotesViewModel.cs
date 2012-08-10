#region SDK Usings
using System;
using System.Linq;
using System.Windows.Input;
using Microsoft.Phone.Controls;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointAddNotesViewModel : TripPointViewModelBase
    {
        private Checkpoint _checkpoint;

        private ICheckpointRepository _checkpointRepository;

        public CheckpointAddNotesViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            _checkpointRepository = RepositoryFactory.CheckpointRepository;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddNotesCommand = new RelayCommand(AddNotesAction);
            CancelAddNotesCommand = new RelayCommand(CancelAddNotesAction);
        }

        private void AddNotesAction()
        {
            if (_checkpoint == null) return;
            if (string.IsNullOrEmpty(Notes)) return;

            AddNotesToCheckpoint(new Note { Text = Notes });
            SaveCheckpoint();

            Navigator.GoBack();
        }

        private void AddNotesToCheckpoint(Note note)
        {
            if (note == null) return;

            Logger.Log(this, string.Format("checkpoint {0}, note {1}", _checkpoint, note));

            _checkpoint.Notes.Add(note);
        }

        private void SaveCheckpoint()
        {
            _checkpointRepository.SaveCheckpoint(_checkpoint);
        }

        private void CancelAddNotesAction()
        {
            Navigator.GoBack();
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var checkpointID = GetCheckpointID(e.View);
            _checkpoint = GetCheckpoint(checkpointID);
        }

        /// <summary>
        /// Extracts the index of the checkpoint being updated
        /// The index must be passed to the view as a parameter (e.g. a query string)
        /// </summary>
        /// <returns></returns>
        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null) return -1;

            var checkpointIdParameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(checkpointIdParameter);
        }

        private Checkpoint GetCheckpoint(int checkpointID)
        {
            return _checkpointRepository.FindCheckpoint(checkpointID);
        }

        public string Notes { get; set; }

        public ICommand AddNotesCommand { get; private set; }

        public ICommand CancelAddNotesCommand { get; private set; }
    }
}
