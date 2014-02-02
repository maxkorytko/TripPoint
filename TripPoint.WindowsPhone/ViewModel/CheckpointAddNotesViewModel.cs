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
    public class CheckpointAddNotesViewModel : Base.TripPointViewModelBase
    {
        private int _checkpointID;
        private string _notes;
        private ICheckpointRepository _checkpointRepository;

        public CheckpointAddNotesViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddNotesCommand = new RelayCommand(AddNotesAction);
            CancelAddNotesCommand = new RelayCommand(CancelAddNotesAction);
        }

        public string Notes 
        {
            get { return _notes; }
            set
            {
                if (_notes == value) return;

                _notes = value;
                RaisePropertyChanged("Notes");
            }
        }

        public ICommand AddNotesCommand { get; private set; }

        public ICommand CancelAddNotesCommand { get; private set; }

        private void AddNotesAction()
        {
            if (String.IsNullOrWhiteSpace(Notes)) return;

            AddNotesToCheckpoint(_checkpointID);
            ResetViewModel();
            Navigator.GoBack();
        }

        private void AddNotesToCheckpoint(int checkpointID)
        {
            AddNotesToCheckpoint(_checkpointRepository.FindCheckpoint(checkpointID));
        }

        private void AddNotesToCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint == null) return;

            checkpoint.Notes.Add(new Note { Text = Notes });
            _checkpointRepository.UpdateCheckpoint(checkpoint);
        }

        private void ResetViewModel()
        {
            Notes = String.Empty;
        }

        private void CancelAddNotesAction()
        {
            ResetViewModel();
            Navigator.GoBack();
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _checkpointRepository = RepositoryFactory.CheckpointRepository;
            _checkpointID = TripPointConvert.ToInt32(GetParameter(e.View, "checkpointID"));
        }
    }
}
