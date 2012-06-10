using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using TripPoint.Model.Domain;

namespace TripPoint.Test.Domain
{
    [TestClass]
    public class TripTest
    {
        [TestMethod]
        public void TestTripValidate()
        {
            var trip = new Trip
            {
                Name = "Test Trip",
                StartDate = DateTime.Now
            };

            bool isValid = trip.Validate();

            Assert.IsTrue(isValid, "Trip must be valid");
        }

        [TestMethod]
        public void TestTripValidateRequiredProperties()
        {
            var trip = new Trip();

            bool isValid = trip.Validate();

            Assert.IsFalse(isValid, "Should fail validation when 1 or more required properties are not set");
        }

        [TestMethod]
        public void TestTripID()
        {
            var trip = new Trip();

            Assert.IsNotNull(trip.ID, "ID must not be null");

            trip = new Trip
            {
                Name = "Test Trip",
                StartDate = DateTime.Now.AddDays(-5)
            };

            Assert.IsNotNull(trip.ID, "ID must not be null");
        }

        [TestMethod]
        public void TestTripIDForEqualObjects()
        {
            var tripA = new Trip
            {
                Name = "Test Trip",
                Notes = "Some trip notes"
            };

            var tripB = new Trip
            {
                Name = "Test Trip",
                Notes = "Some trip notes"
            };

            Assert.AreEqual(tripA.ID, tripB.ID, "Objects which properties have same values must have equal ID");
        }

        [TestMethod]
        public void TestTripIDRemainsUnchanged()
        {
            var trip = new Trip
            {
                Name = "Test Trip"
            };

            string id = trip.ID;
            string id2 = trip.ID;

            Assert.AreEqual(id, id2, "ID must remain the same unless the object state changes");
        }

        [TestMethod]
        public void TestTripIDChanges()
        {
            var trip = new Trip
            {
                Name = "Test Trip",
                StartDate = DateTime.Now,
                Notes = "Blah, blah"
            };

            string id = trip.ID;

            trip.Notes = string.Empty;

            string newId = trip.ID;

            Assert.AreNotEqual(id, newId, "ID must change when state of the object changes");
        }
    }
}
