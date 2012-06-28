#region SDK Usings

using System;
using System.Windows;
using System.Windows.Input;

#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : TripPointViewModelBase
    {
        private ITripRepository _tripRepository;

        public CreateCheckpointViewModel(ITripRepository tripRepository)
        {
            if (tripRepository == null)
                throw new ArgumentNullException("tripRepository");

            _tripRepository = tripRepository;

            Checkpoint = new Checkpoint
            {
                Timestamp = DateTime.Now
            };

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            CancelCreateCheckpointCommand = new RelayCommand(CancelCreateCheckpointAction);
            AddPicturesCommand = new RelayCommand(AddPicturesAction);
        }

        public Checkpoint Checkpoint { get; private set; }

        public ICommand CreateCheckpointCommand { get; private set; }

        public ICommand CancelCreateCheckpointCommand { get; private set; }

        public ICommand AddPicturesCommand { get; private set; }

        private void CreateCheckpointAction()
        {
            Logger.Log(this, "{0}", Checkpoint);

            var trip = (Application.Current as App).CurrentTrip;
            
            trip.Checkpoints.Add(Checkpoint);

            _tripRepository.SaveTrip(trip);

            TripPointNavigation.Navigate("/Trip/Current");
        }

        private void CancelCreateCheckpointAction()
        {
            TripPointNavigation.GoBack();
        }

        private void AddPicturesAction()
        {
            Logger.Log(this, "Add pictures");
        }
    }
}
