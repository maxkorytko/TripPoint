using System;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.I18N;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.State;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripDetailsViewModel : Base.TripCheckpointsViewModelBase
    {
        private bool _isDeletingTrip;

        public TripDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditTripCommand = new RelayCommand(EditTripAction);
            DeleteTripCommand = new RelayCommand(DeleteTripAction);
        }

        public bool IsDeletingTrip
        {
            get { return _isDeletingTrip; }
            private set
            {
                if (_isDeletingTrip == value) return;

                _isDeletingTrip = value;
                RaisePropertyChanged("IsDeletingTrip");
            }
        }

        public ICommand EditTripCommand { get; private set; }

        public ICommand DeleteTripCommand { get; private set; }

        private void EditTripAction()
        {
            if (Trip == null) return;

            Navigator.Navigate(String.Format("/Trip/{0}/Edit", Trip.ID));
        }

        private void DeleteTripAction()
        {
            var userDecision = MessageBox.Show(Resources.ConfirmDeleteTrip, Resources.Deleting,
                MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK) DeleteTripAsync();
        }

        private void DeleteTripAsync()
        {
            IsDeletingTrip = true;

            var worker = new BackgroundWorker();

            worker.DoWork += (sender, args) => { DeleteTrip(); };
            worker.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error == null)
                {
                    Navigator.GoBack();
                    return;
                }

                MessageBox.Show(Resources.DeleteTripFailed);
                IsDeletingTrip = false;
            };

            worker.RunWorkerAsync();
        }

        private void DeleteTrip()
        {
            DeletePictures();
            DeleteCheckpoints();
            TripRepository.DeleteTrip(Trip);
        }

        private void DeletePictures()
        {
            var picturesToDelete = Trip.Checkpoints.SelectMany(checkpoint => checkpoint.Pictures);
            PictureStore.DeletePictures(picturesToDelete);
        }

        private void DeleteCheckpoints()
        {
            RepositoryFactory.CheckpointRepository.DeleteCheckpoints(Trip.Checkpoints);
            Checkpoints = null;
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
 	         base.OnNavigatedTo(e);

             IsDeletingTrip = false;
        }
    }
}