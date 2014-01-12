using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.State.Data;
using TripPoint.WindowsPhone.Utils;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media.Imaging;

namespace TripPoint.WindowsPhone.ViewModel.Base
{
    public class TripCheckpointsViewModelBase : TripViewModelBase
    {
        private static readonly string LAST_VIEWED_CHECKPOINT_ID = "LastViewedCheckpointID";

        private static readonly BitmapSource CHECKPOINT_THUMBNAIL_PLACEHOLDER =
            new BitmapImage(new Uri("/Assets/Images/checkpoint.thumb.png", UriKind.RelativeOrAbsolute));

        protected ICollection<Checkpoint> _checkpoints;
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
        public ICollection<Checkpoint> Checkpoints
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

            StateManager.Instance.Set<int>(LAST_VIEWED_CHECKPOINT_ID, checkpoint.ID);

            Navigator.Navigate(string.Format("/Checkpoints/{0}/Details", checkpoint.ID));
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RefreshCheckpoints();
            StateManager.Instance.Remove(LAST_VIEWED_CHECKPOINT_ID);
        }

        private void RefreshCheckpoints()
        {
            if (_checkpoints == null || _checkpointPaginator == null) return;

            if (Trip.Checkpoints.Count == _checkpointPaginator.Dataset.Count())
            {
                // no checkpoints were added or removed
                // the user, however, could have updated the last viewed checkpoint
                // ensure updates (if any) are immediately visible
                //
                RefreshLastViewedCheckpoint();
                return;
            }

            _checkpoints.Clear();
            ReloadCheckpoints();
        }

        private void RefreshLastViewedCheckpoint()
        {
            if (!StateManager.Instance.Contains(LAST_VIEWED_CHECKPOINT_ID)) return;
            if (_checkpoints == null) return;

            var id = StateManager.Instance.Get<int>(LAST_VIEWED_CHECKPOINT_ID);

            var staleCheckpoint = _checkpoints.SingleOrDefault(c => c.ID == id);
            var updatedCheckpoint = Trip.Checkpoints.SingleOrDefault(c => c.ID == id);

            RefreshCheckpoint(staleCheckpoint, updatedCheckpoint);
        }

        protected static void RefreshCheckpoint(Checkpoint target, Checkpoint source)
        {
            if (target == null || source == null) return;

            target.Title = source.Title;
            RefreshCheckpointThumbnail(target, source);
        }

        private static void RefreshCheckpointThumbnail(Checkpoint target, Checkpoint source)
        {
            if (target.Pictures.Count == source.Pictures.Count) return;

            target.Pictures = source.Pictures;
            target.Thumbnail = CreateThumbnailForCheckpoint(target);
        }

        private void ReloadCheckpoints()
        {
            var currentPage = _checkpointPaginator.CurrentPage;

            _checkpointPaginator = CreateCheckpointPaginator(Trip.Checkpoints);

            do
            {
                PaginateCheckpoints();
            }
            while (_checkpointPaginator.CurrentPage < currentPage && _checkpointPaginator.CanPaginate);
        }
    }
}
