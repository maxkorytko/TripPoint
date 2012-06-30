#region SDK Usings
using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointAddNotesViewModel : TripPointViewModelBase
    {
        private ITripRepository _tripRepository;

        public CheckpointAddNotesViewModel(ITripRepository tripRepository)
        {
            if (tripRepository == null)
                throw new ArgumentNullException("tripRepository");

            _tripRepository = tripRepository;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddNotesCommand = new RelayCommand(AddNotesAction);
            CancelAddNotesCommand = new RelayCommand(CancelAddNotesAction);
        }

        public string Notes { get; set; }

        public ICommand AddNotesCommand { get; private set; }

        public ICommand CancelAddNotesCommand { get; private set; }

        private void AddNotesAction()
        {
            var checkpointIndex = GetCheckpointIndex();

            if (checkpointIndex != -1)
            {
                var note = new Note { Text = Notes };

                AddNotesToCheckpoint(checkpointIndex, note);
            }

            TripPointNavigation.GoBack();
        }

        /// <summary>
        /// Extracts the index of the checkpoint being updated
        /// The index must be passed to the view as a parameter (e.g. a query string)
        /// </summary>
        /// <returns></returns>
        private static int GetCheckpointIndex()
        {
            var index = -1;

            var page = TripPointNavigation.CurrentPage;

            if (page != null)
            {   
                var checkpointIndexParameter = page.TryGetQueryStringParameter("checkpointIndex");
                if (!Int32.TryParse(checkpointIndexParameter, out index))
                    index = -1;
            }

            return index;
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
        private static Trip GetTrip()
        {
            return (Application.Current as App).CurrentTrip;
        }

        private void CancelAddNotesAction()
        {
            TripPointNavigation.GoBack();
        }
    }
}
