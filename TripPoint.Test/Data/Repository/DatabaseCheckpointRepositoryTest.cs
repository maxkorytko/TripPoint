using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

using TripPoint.Model.Domain;
using TripPoint.Model.Data;
using TripPoint.Model.Data.Repository;

namespace TripPoint.Test.Data.Repository
{
    [TestClass]
    public class DatabaseCheckpointRepositoryTest
    {
        public static readonly string ConnectionString =
            "Data Source=isostore:/DatabaseCheckpointRepositoryTest.sdf";

        TripPointDataContext _dataContext;
        DatabaseCheckpointRepository _checkpointRepository;

        Checkpoint _checkpointA, _checkpointB, _checkpointC;

        [ClassInitialize]
        public void InitializeClass()
        {
            _dataContext = new TripPointDataContext(ConnectionString);
            _checkpointRepository = new DatabaseCheckpointRepository(_dataContext);

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
            InsertTestCheckpoints();
        }

        [TestCleanup]
        public void Cleanup()
        {
            DeleteAllCheckpoints();
        }

        [TestMethod]
        public void TestFindCheckpoint()
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(_checkpointA.ID);

            Assert.AreEqual<string>(_checkpointA.Title, checkpoint.Title, "Did not find the right checkpoint");
            Assert.AreEqual<int>(_checkpointA.Notes.Count, checkpoint.Notes.Count, "Did not find the right checkpoint");
        }

        [TestMethod]
        public void TestFindCheckpointDoesNotExist()
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(-1);

            Assert.IsNull(checkpoint, "Must not find any checkpoints");
        }

        [TestMethod]
        public void TestSaveCheckpoint()
        {
            DeleteAllCheckpoints();
            
            var trip = new Trip
            {
                Name = "My Trip"
            };

            var location = new GeoLocation
            {
                Latitude = 43.8663,
                Longitude = -79.5695
            };

            var checkpoint = new Checkpoint
            {
                Title = "Testing",
                Trip = trip,
                Location = location
            };

            _checkpointRepository.SaveCheckpoint(checkpoint);

            var savedCheckpoint = _dataContext.Checkpoints.FirstOrDefault();

            Assert.IsNotNull(savedCheckpoint, "Checkpoint was not saved");
            Assert.AreEqual<string>(checkpoint.Title, savedCheckpoint.Title, "Checkpoint was not saved");
            Assert.AreEqual<string>(checkpoint.Trip.Name, savedCheckpoint.Trip.Name, "Checkpoint was not saved");
            Assert.AreEqual<double>(location.Latitude, savedCheckpoint.Location.Latitude, "Location was not saved");
        }

        [TestMethod]
        public void TestSaveCheckpointNull()
        {
            DeleteAllCheckpoints();

            _checkpointRepository.SaveCheckpoint(null);

            Assert.AreEqual<int>(0, _dataContext.Checkpoints.Count());
        }

        [TestMethod]
        public void TestUpdateCheckpoint()
        {
            _checkpointA.Title = "New Title";

            _checkpointRepository.UpdateCheckpoint(_checkpointA);

            var updatedCheckpoint = _dataContext.Checkpoints.Where(c => c.ID == _checkpointA.ID).FirstOrDefault();

            Assert.AreEqual<string>(_checkpointA.Title, updatedCheckpoint.Title, "Checkpoint was not updated");
        }

        [TestMethod]
        public void TestDeleteCheckpoint()
        {
            var location = _checkpointA.Location;

            _checkpointRepository.DeleteCheckpoint(_checkpointA);

            var deletedCheckpoint = _dataContext.Checkpoints.Where(c => c.ID == _checkpointA.ID).FirstOrDefault();
            var deletedLocation = _dataContext.Locations.Where(l => l.ID == location.ID).FirstOrDefault();

            Assert.IsNull(deletedCheckpoint, "Checkpoint was not deleted");
            Assert.IsNull(deletedLocation, "Location was not deleted");
        }

        private void InsertTestCheckpoints()
        {
            var trip = new Trip { Name = "Trip" };

            _checkpointA = new Checkpoint
            {
                Title = "Checkpoint A",
                Trip = trip,
                Location = new GeoLocation { Latitude = 43.8663, Longitude = -79.5695 }
            };
            _checkpointA.Notes.Add(new Note 
            {
                Text = "unit test",
                Checkpoint = _checkpointA
            });

            _checkpointB = new Checkpoint
            {
                Title = "Checkpoint B",
                Trip = trip,
                Location = new GeoLocation { Latitude = 12.5023, Longitude = -83.9375 }
            };

            _checkpointC = new Checkpoint
            {
                Title = "Checkpoint C",
                Trip = trip,
                Location = new GeoLocation { Latitude = 34.6334, Longitude = -45.1297 }
            };

            _dataContext.Checkpoints.InsertOnSubmit(_checkpointA);
            _dataContext.Checkpoints.InsertOnSubmit(_checkpointB);
            _dataContext.Checkpoints.InsertOnSubmit(_checkpointC);

            _dataContext.SubmitChanges();
        }

        private void DeleteAllCheckpoints()
        {
            var notes = _dataContext.Notes.Select(note => note);
            _dataContext.Notes.DeleteAllOnSubmit(notes);

            var checkpoints = _dataContext.Checkpoints.Select(checkpoint => checkpoint);
            var locations = checkpoints.Select(checkpoint => checkpoint.Location);
            
            _dataContext.Checkpoints.DeleteAllOnSubmit(checkpoints);
            _dataContext.Locations.DeleteAllOnSubmit(locations);

            var trips = _dataContext.Trips.Select(trip => trip);
            _dataContext.Trips.DeleteAllOnSubmit(trips);

            _dataContext.SubmitChanges();
        }
    }
}
