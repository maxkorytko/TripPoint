using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TripPoint.Model.Domain;
using TripPoint.Model.Data;

namespace TripPoint.Test.Data
{
    [TestClass]
    public class DataContextTest
    {
        public static readonly string ConnectionString = "Data Source=isostore:/DataContextTest.sdf";

        private TripPointDataContext _dataContext;

        [ClassInitialize]
        public void InitializeClass()
        {
            _dataContext = new TripPointDataContext(ConnectionString);

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
        public void InitializeTest()
        {
            _dataContext.Checkpoints.DeleteAllOnSubmit<Checkpoint>(
                _dataContext.Checkpoints.Select(checkpoint => checkpoint));

            _dataContext.Trips.DeleteAllOnSubmit<Trip>(
                _dataContext.Trips.Select(trip => trip));
        }

        [TestMethod]
        public void TestCreateCheckpoint()
        {
            var trip = new Trip
            {
                Name = "Trip A",
                StartDate = DateTime.Now.AddDays(-1)
            };

            var checkpointA = new Checkpoint
            {
                Title = "Checkpoint A"
            };

            var checkpointB = new Checkpoint
            {
                Title = "Checkpoint B"
            };

            trip.Checkpoints.Add(checkpointA);
            trip.Checkpoints.Add(checkpointB);

            _dataContext.Trips.InsertOnSubmit(trip);
            _dataContext.SubmitChanges();

            trip = _dataContext.Trips.First();

            Assert.AreEqual<string>("Trip A", trip.Name);
            Assert.AreEqual<int>(2, trip.Checkpoints.Count);
            Assert.AreEqual<string>("Checkpoint A", trip.Checkpoints.First().Title);

            var checkpoints = from checkpoint in _dataContext.Checkpoints
                              where checkpoint.Trip == trip
                              select checkpoint;

            Assert.AreEqual<int>(2, checkpoints.Count());
        }

        [TestMethod]
        public void TestUpdateCheckpoint()
        {
            var trip = new Trip
            {
                Name = "Trip A",
                StartDate = DateTime.Now.AddDays(-1)
            };

            var checkpoint = new Checkpoint
            {
                Title = "Checkpoint A"
            };

            trip.Checkpoints.Add(checkpoint);

            _dataContext.Trips.InsertOnSubmit(trip);
            _dataContext.SubmitChanges();

            _dataContext.Dispose();
            _dataContext = new TripPointDataContext(ConnectionString);

            trip = _dataContext.Trips.First();
            checkpoint = trip.Checkpoints.First();

            Assert.AreEqual<string>("Checkpoint A", checkpoint.Title);

            checkpoint.Title = "My Checkpoint";

            _dataContext.SubmitChanges();

            checkpoint = _dataContext.Checkpoints.First();

            Assert.AreEqual<string>("My Checkpoint", checkpoint.Title);
        }

        [TestMethod]
        public void TestAddCheckpointToExistingTrip()
        {
            var trip = new Trip
            {
                Name = "Trip"
            };

            _dataContext.Trips.InsertOnSubmit(trip);
            _dataContext.SubmitChanges();

            trip = _dataContext.Trips.First();

            trip.Checkpoints.Add(new Checkpoint { Title = "Checkpoint" });

            _dataContext.SubmitChanges();

            var checkpoint = _dataContext.Checkpoints.FirstOrDefault();

            Assert.IsNotNull(checkpoint);

            trip = _dataContext.Trips.First();

            Assert.AreEqual<int>(1, trip.Checkpoints.Count());
        }
    }
}
