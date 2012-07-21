using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Data.Linq;

using TripPoint.Model.Domain;
using TripPoint.Model.Data;
using TripPoint.Model.Data.Repository;

namespace TripPoint.Test.Data.Repository
{
    [TestClass]
    public class DatabaseTripRepositoryTest
    {
        public static readonly string ConnectionString = "Data Source=isostore:/TripPointTest.sdf";

        TripPointDataContext _dataContext;
        DatabaseTripRepository _tripRepository;

        Trip _tripA, _tripB, _tripC;

        [ClassInitialize]
        public void InitializeClass()
        {
            _dataContext = new TripPointDataContext(ConnectionString);
            _tripRepository = new DatabaseTripRepository(_dataContext);

            if (!_dataContext.DatabaseExists())
                _dataContext.CreateDatabase();
        }

        [ClassCleanup]
        public void CleanupClass()
        {
            if (_dataContext.DatabaseExists())
                _dataContext.DeleteDatabase();
        }

        [TestInitialize]
        public void Initialize()
        {
            InsertTestTrips();
        }

        [TestCleanup]
        public void Cleanup()
        {
            DeleteAllTrips();
        }

        [TestMethod]
        public void TestSaveTrip()
        {
            DeleteAllTrips();

            _tripRepository.SaveTrip(new Trip { Name = "My Trip" });
            _tripRepository.SaveTrip(new Trip { Name = "My Second Trip" });

            Assert.AreEqual<int>(2, _dataContext.Trips.Count(), "There must be 2 trips in the database");
        }

        [TestMethod]
        public void TestSaveTripIsNull()
        {
            DeleteAllTrips();

            _tripRepository.SaveTrip(null);

            Assert.AreEqual<int>(0, _dataContext.Trips.Count(), "Null trip must not be saved");
        }

        [TestMethod]
        public void TestUpdateTrip()
        {
            DeleteAllTrips();

            _tripRepository.SaveTrip(new Trip { Name = "Trip" });

            var trip = _dataContext.Trips.First();

            trip.Name = "New Trip";

            _tripRepository.SaveTrip(trip);

            trip = _dataContext.Trips.First();

            Assert.AreEqual<string>("New Trip", trip.Name, "The trip has not been udpated");
        }

        [TestMethod]
        public void TestUpdateTripAfterAddingCheckpoint()
        {
            _tripA.Checkpoints.Add(new Checkpoint { Title = "Checkpoint" });

            _tripRepository.SaveTrip(_tripA);

            _tripA = _dataContext.Trips.Where(t => t.ID == _tripA.ID).FirstOrDefault();

            Assert.AreEqual<int>(1, _tripA.Checkpoints.Count());
        }

        [TestMethod]
        public void TestTrips()
        {
            var trips = _tripRepository.Trips;

            Assert.AreEqual<int>(3, trips.Count(), "The database contains a different number of trips");
        }

        [TestMethod]
        public void TestTripsAfterTripDelete()
        {
            _dataContext.Trips.DeleteOnSubmit(_tripA);
            _dataContext.SubmitChanges();

            var trips = _tripRepository.Trips;

            Assert.AreEqual<int>(2, trips.Count(), "The database contains a different number of trips");

            DeleteAllTrips();

            Assert.AreEqual<int>(0, trips.Count(), "The database contains a different number of trips");
        }

        [TestMethod]
        public void TestFind()
        { 
            var trip = _tripRepository.FindTrip(_tripB.ID);

            Assert.AreSame(_tripB, trip, "Did not find the right trip");
        }

        [TestMethod]
        public void TestFindNoTripExists()
        {
            var trip = _tripRepository.FindTrip(-1);

            Assert.IsNull(trip, "Must not find any trips");
        }

        [TestMethod]
        public void TestDelete()
        {
            _tripRepository.DeleteTrip(_tripA);
            _tripRepository.DeleteTrip(_tripB);

            Assert.AreEqual<int>(1, _dataContext.Trips.Count(), "Did not delete trips");
        }

        [TestMethod]
        public void TestDeleteTripTwice()
        {
            _tripRepository.DeleteTrip(_tripA);
            _tripRepository.DeleteTrip(_tripA);

            Assert.AreEqual<int>(2, _dataContext.Trips.Count(), "Must delete trip once");
        }

        [TestMethod]
        public void TestDeleteNullTrip()
        {
            _tripRepository.DeleteTrip(null);

            Assert.AreEqual<int>(3, _dataContext.Trips.Count(), "Must not delete any trips");
        }

        private void DeleteAllTrips()
        {
            var checkpoints = _dataContext.Checkpoints.Select(checkpoint => checkpoint);
            _dataContext.Checkpoints.DeleteAllOnSubmit(checkpoints);

            var trips = _dataContext.Trips.Select(trip => trip);
            _dataContext.Trips.DeleteAllOnSubmit<Trip>(trips);
            
            _dataContext.SubmitChanges();
        }

        private void InsertTestTrips()
        {
            _tripA = new Trip
            {
                Name = "Trip A",
                StartDate = DateTime.Now.AddDays(-1)
            };

            _tripB = new Trip
            {
                Name = "Trip B",
                StartDate = DateTime.Now.AddHours(-1)
            };

            _tripC = new Trip
            {
                Name = "Trip C",
                StartDate = DateTime.Now.AddMinutes(-1)
            };

            _dataContext.Trips.InsertOnSubmit(_tripA);
            _dataContext.Trips.InsertOnSubmit(_tripB);
            _dataContext.Trips.InsertOnSubmit(_tripC);

            _dataContext.SubmitChanges();
        }
    }
}
