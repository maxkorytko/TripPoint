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
            Trip trip = new Trip
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
            Trip trip = new Trip();

            bool isValid = trip.Validate();

            Assert.IsFalse(isValid, "Should fail validation when 1 or more required properties are not set");
        }
    }
}
