using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.I18N;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripDetailsViewModel : Base.TripViewModelBase
    {
        private bool _isDeletingTrip;

        public TripDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
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

        public ICommand ViewCheckpointDetailsCommand { get; private set; }

        public ICommand DeleteTripCommand { get; private set; }

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

            DeleteTripAsync();
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
            RepositoryFactory.PictureRepository.DeletePictures(picturesToDelete);
        }

        private void DeleteCheckpoints()
        {
            RepositoryFactory.CheckpointRepository.DeleteCheckpoints(Trip.Checkpoints);
        }

        public override void  OnNavigatedTo(TripPointNavigationEventArgs e)
        {
 	         base.OnNavigatedTo(e);

             ResetViewModel();
        }

        private void ResetViewModel()
        {
            IsDeletingTrip = false;
        }
    }
}