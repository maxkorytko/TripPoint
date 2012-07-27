#region SDK Usings

using System;
using System.Windows.Input;

#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : TripPointViewModelBase
    {
        private IRepositoryFactory _repositoryFactory;
        private ITripRepository _tripRepository;

        public CreateCheckpointViewModel(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

            _repositoryFactory = repositoryFactory;
            _tripRepository = repositoryFactory.TripRepository;

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

        public string Notes { get; set; }

        public ICommand CreateCheckpointCommand { get; private set; }

        public ICommand CancelCreateCheckpointCommand { get; private set; }

        public ICommand AddPicturesCommand { get; private set; }

        private void CreateCheckpointAction()
        {
            Logger.Log(this, "{0}", Checkpoint);

            Checkpoint.Notes.Add(
                new Note { Text = Notes }
                );

            var trip = _tripRepository.CurrentTrip;

            trip.Checkpoints.Add(Checkpoint);

            _tripRepository.SaveTrip(trip);

            Navigator.Navigate("/Trip/Current");
        }

        private void CancelCreateCheckpointAction()
        {
            Navigator.GoBack();
        }

        private void AddPicturesAction()
        {
            Logger.Log(this, "Add pictures");
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tripRepository = _repositoryFactory.TripRepository;
        }
    }
}
