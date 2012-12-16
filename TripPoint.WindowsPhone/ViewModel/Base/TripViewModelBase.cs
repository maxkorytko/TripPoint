using System;
using System.Linq;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.State.Data;

namespace TripPoint.WindowsPhone.ViewModel.Base
{
    public abstract class TripViewModelBase : TripPointViewModelBase
    {
        private Trip _trip;
        
        public TripViewModelBase(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {   
        }

        public Trip Trip
        {
            get { return _trip; }
            set
            {
                if (_trip == value) return;

                _trip = value;
                RaisePropertyChanged("Trip");
            }
        }

        protected ITripRepository TripRepository { get; private set; }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            TripRepository = RepositoryFactory.TripRepository;

            InitializeTrip(GetTripID(e.View));
            InitializeThumbnailsForTripCheckpoints();
        }

        private static int GetTripID(PhoneApplicationPage view)
        {
            var tripID = -1;

            if (view != null)
                tripID = TripPointConvert.ToInt32(view.TryGetQueryStringParameter("tripID"));

            return tripID;
        }

        protected virtual void InitializeTrip(int tripID)
        {
            if (TripRepository == null) return;

            Trip = TripRepository.FindTrip(tripID);
        }

        protected void InitializeThumbnailsForTripCheckpoints()
        {
            if (Trip == null) return;

            foreach (var checkpoint in Trip.Checkpoints)
            {
                var thumbnail = (PictureThumbnail)checkpoint.Thumbnail;

                if (thumbnail == null)
                {
                    thumbnail = new PictureThumbnail(new Uri("/Assets/Images/Dark/checkpoint.thumb.png",
                        UriKind.Relative));
                    checkpoint.Thumbnail = thumbnail;
                }

                thumbnail.Picture = checkpoint.Pictures.LastOrDefault();
            }
        }
    }
}
