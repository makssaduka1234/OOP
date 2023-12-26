using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using UnitTestProject;
using ReservationManagers;

namespace UnitTestProject.Tests
{
    [TestFixture]
    public class ReservationManagerTests
    {
        private ReservationManager reservationManager;

        [SetUp]
        public void SetUp()
        {
            reservationManager = new ReservationManager();
        }

        [Test]
        public void AddRestaurant_ValidInput_RestaurantAdded()
        {
            string restaurantName = "TestRestaurant";
            int tableCount = 5;

            reservationManager.AddRestaurant(restaurantName, tableCount);

            Assert.AreEqual(1, reservationManager.res.Count);
            Assert.AreEqual(restaurantName, reservationManager.res[0].name);
            Assert.AreEqual(tableCount, reservationManager.res[0].table.Length);
        }

        [Test]
        public void FindAllFreeTables_ValidDate_ReturnsListOfFreeTables()
        {
            reservationManager.AddRestaurant("TestRestaurant", 3);

            var freeTables = reservationManager.FindAllFreeTables(DateTime.Now);

            Assert.AreEqual(3, freeTables.Count);
            Assert.IsTrue(freeTables.All(table => table.StartsWith("TestRestaurant - Table")));
        }

        [Test]
        public void SortResByAvail_ValidDate_RestaurantsSortedByAvailability()
        {
            reservationManager.AddRestaurant("Restaurant1", 3);
            reservationManager.AddRestaurant("Restaurant2", 5);

            reservationManager.SortResByAvail(DateTime.Now);

            Assert.AreEqual("Restaurant2", reservationManager.res[0].name);
            Assert.AreEqual("Restaurant1", reservationManager.res[1].name);
        }
    }
}
