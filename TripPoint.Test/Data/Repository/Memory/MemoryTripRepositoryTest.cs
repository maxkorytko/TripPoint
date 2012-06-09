using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Memory;

namespace TripPoint.Test.Data.Repository.Memory
{
    [TestClass]
    public class MemoryTripRepositoryTest
    {
        ITripRepository _tripRepository = new MemoryTripRepository();

        [TestInitialize]
        public void SetUp()
        {
            _tripRepository.DeleteAll();
        }

        [TestMethod]
        public void TestSaveTrip()
        {
            var trip = new Trip();

            _tripRepository.SaveTrip(trip);

            Assert.AreEqual(1, _tripRepository.Count, "Trip has not been saved");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestSaveTripIsNull()
        {
            _tripRepository.SaveTrip(null);
        }

        [TestMethod]
        public void TestFindTrip()
        {
            bool success = true;

            var trip = new Trip();

            _tripRepository.SaveTrip(trip);

            var foundTrip = _tripRepository.FindTrip(trip.ID);

            if (!trip.ID.Equals(foundTrip.ID)) success = false;

            if (!trip.Name.Equals(foundTrip.Name)) success = false;

            Assert.IsTrue(success, "Trip was not found");
        }

        [TestMethod]
        public void TestFindTripDoesNotExist()
        {
            bool success = true;

            Trip trip = _tripRepository.FindTrip("123ABC");

            if (trip != null) success = false;

            trip = _tripRepository.FindTrip(null);

            if (trip != null) success = false;

            Assert.IsTrue(success, "Should return null when ID is null or no trip");
        }

        [TestMethod]
        public void TestCount()
        {
            var tripA = new Trip();

            _tripRepository.SaveTrip(tripA);
            _tripRepository.SaveTrip(new Trip { Name = "Trip B" });
            _tripRepository.SaveTrip(new Trip { Name = "Trip C", StartDate = DateTime.Now.AddDays(-10) });

            Assert.AreEqual(3, _tripRepository.Count, "Must be 3 trips in the repository");

            _tripRepository.DeleteTrip(tripA);

            Assert.AreEqual(2, _tripRepository.Count, "Must be 2 trips in the repository");
        }
    }
}
