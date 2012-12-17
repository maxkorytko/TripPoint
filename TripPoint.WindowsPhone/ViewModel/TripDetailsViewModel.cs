using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.I18N;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripDetailsViewModel : Base.TripViewModelBase
    {
        public TripDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ViewCheckpointDetailsCommand = new RelayCommand<Checkpoint>(ViewCheckpointDetailsAction);
            DeleteTripCommand = new RelayCommand(DeleteTripAction);
        }

        private void ViewCheckpointDetailsAction(Checkpoint checkpoint)
        {
            Navigator.Navigate(string.Format("/Checkpoints/{0}/Details", checkpoint.ID));
        }

        private void DeleteTripAction()
        {
            var userDecision = MessageBox.Show(Resources.ConfirmDeleteTrip, Resources.Deleting,
                MessageBoxButton.OKCancel);

            if (userDecision != MessageBoxResult.OK) return;

            try
            {
                DeleteTrip();
                Navigator.GoBack();
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.DeleteTripFailed);
            }
        }

        private void DeletePictures()
        {
            var picturesToDelete = Trip.Checkpoints.SelectMany(checkpoint => checkpoint.Pictures);
            RepositoryFactory.PictureRepository.DeletePictures(picturesToDelete);
        }

        private void DeleteCheckpoints()
        {
            RepositoryFactory.CheckpointRepository.DeleteCheckpoints(Trip.Checkpoints);
        }

        private void DeleteTrip()
        {
            DeletePictures();
            DeleteCheckpoints();
            TripRepository.DeleteTrip(Trip);
        }

        public ICommand ViewCheckpointDetailsCommand { get; private set; }

        public ICommand DeleteTripCommand { get; private set; }
    }
}