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
        private ITripRepository _tripRepository;

        private int _checkpointIndex;

        public CheckpointAddNotesViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            _tripRepository = repositoryFactory.TripRepository;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddNotesCommand = new RelayCommand(AddNotesAction);
            CancelAddNotesCommand = new RelayCommand(CancelAddNotesAction);
        }

        private void AddNotesAction()
        {
            if (_checkpointIndex != -1)
            {
                var note = new Note { Text = Notes };

                AddNotesToCheckpoint(_checkpointIndex, note);
            }

            Navigator.GoBack();
        }

        private void AddNotesToCheckpoint(int checkpointIndex, Note note)
        {
            Logger.Log(this, string.Format("checkpoint {0}, note {1}", checkpointIndex, note));

            var trip = GetTrip();

            if (trip == null) return;

            var checkpoint = trip.Checkpoints.ElementAtOrDefault(checkpointIndex);

            if (checkpoint == null) return;

            checkpoint.Notes.Add(note);

            _tripRepository.SaveTrip(trip);
        }

        /// <summary>
        /// Fetches the trip containing the checkpoint being updated
        /// </summary>
        /// <returns></returns>
        private Trip GetTrip()
        {
            return _tripRepository.CurrentTrip;
        }

        private void CancelAddNotesAction()
        {
            Navigator.GoBack();
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _checkpointIndex = GetCheckpointIndex(e.View);
        }

        /// <summary>
        /// Extracts the index of the checkpoint being updated
        /// The index must be passed to the view as a parameter (e.g. a query string)
        /// </summary>
        /// <returns></returns>
        private static int GetCheckpointIndex(PhoneApplicationPage view)
        {
            var index = -1;

            if (view != null)
            {
                var checkpointIndexParameter = view.TryGetQueryStringParameter("checkpointIndex");
                if (!Int32.TryParse(checkpointIndexParameter, out index))
                    index = -1;
            }

            return index;
        }

        public string Notes { get; set; }

        public ICommand AddNotesCommand { get; private set; }

        public ICommand CancelAddNotesCommand { get; private set; }
    }
}
