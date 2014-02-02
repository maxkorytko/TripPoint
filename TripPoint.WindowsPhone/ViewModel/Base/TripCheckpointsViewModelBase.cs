using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.Utils;
using System.Windows.Media.Imaging;

namespace TripPoint.WindowsPhone.ViewModel.Base
{
    public class TripCheckpointsViewModelBase : TripViewModelBase
    {
        private static readonly string LAST_VIEWED_CHECKPOINT_ID = "LastViewedCheckpointID";

        private static readonly BitmapSource CHECKPOINT_THUMBNAIL_PLACEHOLDER =
            new BitmapImage(new Uri("/Assets/Images/checkpoint.thumb.png", UriKind.RelativeOrAbsolute));

        protected IList<Checkpoint> _checkpoints;
        private CollectionPaginator<Checkpoint> _checkpointPaginator;
        private bool _canPaginateCheckpoints;

        public TripCheckpointsViewModelBase(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            PaginateCheckpointsCommand = new RelayCommand(PaginateCheckpointsAction);
            ViewCheckpointDetailsCommand = new RelayCommand<Checkpoint>(ViewCheckpointDetailsAction);
        }

        /// <summary>
        /// A collection of checkpoints being displayed to the user
        /// </summary>
        public IList<Checkpoint> Checkpoints
        {
            get
            {
                InitializeCheckpoints();
                return _checkpoints;
            }
            set
            {
                if (_checkpoints == value) return;

                _checkpoints = value;
            }
        }

        public bool CanPaginateCheckpoints
        {
            get { return _canPaginateCheckpoints; }
            set
            {
                if (_canPaginateCheckpoints == value) return;

                _canPaginateCheckpoints = value;
                RaisePropertyChanged("CanPaginateCheckpoints");
            }
        }

        public ICommand PaginateCheckpointsCommand { get; private set; }

        public ICommand ViewCheckpointDetailsCommand { get; private set; }

        private void InitializeCheckpoints()
        {
            if (_checkpoints != null) return;

            _checkpoints = new ObservableCollection<Checkpoint>();

            _checkpointPaginator = CreateCheckpointPaginator(Trip.Checkpoints);
            PaginateCheckpoints();
        }

        private static CollectionPaginator<Checkpoint> CreateCheckpointPaginator(
            IEnumerable<Checkpoint> checkpoints)
        {
            var paginator = new CollectionPaginator<Checkpoint>(checkpoints);
            paginator.PageSize = 5;

            return paginator;
        }

        private void PaginateCheckpoints()
        {
            if (_checkpoints == null || _checkpointPaginator == null) return;

            var paginatedCheckpoints = _checkpointPaginator.Paginate();

            _checkpoints.Add(paginatedCheckpoints);
            LoadThumbnailsForCheckpoints(paginatedCheckpoints);

            CanPaginateCheckpoints = _checkpointPaginator.CanPaginate;
        }

        private static void LoadThumbnailsForCheckpoints(IEnumerable<Checkpoint> checkpoints)
        {
            if (checkpoints == null) return;

            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.Thumbnail == null)
                    checkpoint.Thumbnail = CreateThumbnailForCheckpoint(checkpoint);
            }
        }

        private static Thumbnail CreateThumbnailForCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint == null) return null;

            return new Thumbnail
            {
                Picture = checkpoint.Pictures.LastOrDefault(),
                Placeholder = CHECKPOINT_THUMBNAIL_PLACEHOLDER
            };
        }

        private void PaginateCheckpointsAction()
        {
            PaginateCheckpoints();
        }

        private void ViewCheckpointDetailsAction(Checkpoint checkpoint)
        {
            if (checkpoint == null) return;

            RememberCheckpoint(checkpoint);
            Navigator.Navigate(string.Format("/Checkpoints/{0}/Details", checkpoint.ID));
        }

        protected void RememberCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint == null) return;

            StateManager.Instance.Set<int>(LAST_VIEWED_CHECKPOINT_ID, checkpoint.ID);
        }

        public override void OnBackNavigatedTo()
        {
 	        base.OnBackNavigatedTo();

            RefreshLastViewedCheckpoint();
            ForgetCheckpoint();
        }

        private void RefreshLastViewedCheckpoint()
        {
            if (_checkpointPaginator == null) return;

            if (Trip.Checkpoints.Count != _checkpointPaginator.Dataset.Count())
            {
                // last viewed checkpoint has been deleted
                // for simplicity's sake, let's just reload checkpoints
                //
                ReloadCheckpoints();
                return;
            }

            // last viewed checkpoint may have been updated
            // refresh it to make the view up-to-date
            //
            RefreshCheckpoint(StateManager.Instance.Get<int>(LAST_VIEWED_CHECKPOINT_ID));
        }

        private void ReloadCheckpoints()
        {
            var currentPage = _checkpointPaginator.CurrentPage;

            _checkpointPaginator = CreateCheckpointPaginator(Trip.Checkpoints);
            _checkpoints.Clear();

            do
            {
                PaginateCheckpoints();
            }
            while (_checkpointPaginator.CurrentPage < currentPage && _checkpointPaginator.CanPaginate);
        }

        private void RefreshCheckpoint(int checkpointID)
        {
            var oldCheckpoint = _checkpoints.SingleOrDefault(c => c.ID == checkpointID);
            var newCheckpoint = Trip.Checkpoints.SingleOrDefault(c => c.ID == checkpointID);

            RefreshCheckpoint(oldCheckpoint, newCheckpoint);
        }

        private void RefreshCheckpoint(Checkpoint oldCheckpoint, Checkpoint newCheckpoint)
        {
            if (oldCheckpoint == null || newCheckpoint == null) return;

            ReplaceCheckpointAt(_checkpoints.IndexOf(oldCheckpoint), newCheckpoint);
            newCheckpoint.Thumbnail = CreateThumbnailForCheckpoint(newCheckpoint);
        }

        private void ReplaceCheckpointAt(int index, Checkpoint checkpoint)
        {
            if (index < 0 || index >= _checkpoints.Count) return;
            if (checkpoint == null) return;

            // it's safe to bypass the checkpoint paginator and mutate the collection directly,
            // as long as the number of items remains the same
            // also, checkpoint paginator only moves forward, so it won't even notice the new checkpoint
            //
            _checkpoints.RemoveAt(index);
            _checkpoints.Insert(index, checkpoint);
        }

        private void ForgetCheckpoint()
        {
            StateManager.Instance.Remove(LAST_VIEWED_CHECKPOINT_ID);
        }

        public override void ResetViewModel()
        {
            base.ResetViewModel();

            _checkpointPaginator = null;
            _checkpoints = null;
        }
    }
}
