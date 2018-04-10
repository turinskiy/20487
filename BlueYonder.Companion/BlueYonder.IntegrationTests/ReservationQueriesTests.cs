﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using BlueYonder.DataAccess;
using BlueYonder.DataAccess.Repositories;
using BlueYonder.Entities;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace BlueYonder.IntegrationTests
{
    [TestClass]
    public class ReservationQueriesTests
    {
        [ClassInitialize]
        public static void TestInitialize(TestContext context)
        {
            (new TravelCompanionDatabaseInitializer()).InitializeDatabase(new TravelCompanionContext());
        }

        [TestMethod]
        public void GetSingleReservation()
        {
            using (var repository = new ReservationRepository())
            {
                var query = from r in repository.GetAll()
                            where r.ConfirmationCode == "1234"
                            select r;

                var reservation = query.FirstOrDefault();

                Assert.IsNotNull(reservation);
            }
        }

        [TestMethod]
        public void GetReservationWithFlightsEagerLoad()
        {
            Reservation reservation;
            using (var rep = new ReservationRepository())
            {
                var query = from r in rep.GetAll()
                    where r.ConfirmationCode == "1234"
                    select r;

                query = query.Include(r => r.DepartureFlight).Include(r => r.ReturnFlight);

                reservation = query.FirstOrDefault();
            }

            Assert.IsNotNull(reservation);
            Assert.IsNotNull(reservation.DepartureFlight);
            Assert.IsNotNull(reservation.ReturnFlight);
        }

        [TestMethod]
        public void GetReservationWithFlightsLazyLoad()
        {
            Reservation reservation;

            using (var repository = new ReservationRepository())
            {
                var query = from r in repository.GetAll()
                            where r.ConfirmationCode == "1234"
                            select r;

                reservation = query.FirstOrDefault();

                Assert.IsNotNull(reservation);
                Assert.IsNotNull(reservation.DepartureFlight);
                Assert.IsNotNull(reservation.ReturnFlight);
            }
        }

        [TestMethod]
        public void GetReservationsWithSingleFlightWithoutLazyLoad()
        {
            Reservation reservation;
            TravelCompanionContext context = new TravelCompanionContext();
            context.Configuration.LazyLoadingEnabled = false;

            using (var repository = new ReservationRepository(context))
            {
                var query = from r in repository.GetAll()
                            where r.ConfirmationCode == "1234"
                            select r;

                reservation = query.FirstOrDefault();

                Assert.IsNotNull(reservation);
                Assert.IsNull(reservation.DepartureFlight);
                Assert.IsNull(reservation.ReturnFlight);
            }
        }

        [TestMethod]
        public void GetOrderedReservations()
        {
            List<Reservation> reservations = null;
            using (TravelCompanionContext context = new TravelCompanionContext())
            {
                var sqlQuery = @"select value r from reservations as r order by r.confirmationCode DESC";

                var query = ((IObjectContextAdapter) context).ObjectContext.CreateQuery<Reservation>(sqlQuery);
                reservations = query.ToList();
                
                Assert.AreEqual(reservations.Count, 2);
                Assert.AreEqual(reservations.ElementAt(0).ConfirmationCode, "4321");
                Assert.AreEqual(reservations.ElementAt(1).ConfirmationCode, "1234");
            }
        }

        [TestMethod]
        public void GetReservationsForDepartFlightsBySeatClass()
        {
            using (TravelCompanionContext context = new TravelCompanionContext())
            {
                IEnumerable<Reservation> query = context.Database.SqlQuery<Reservation>(
                    @"select r.* from Reservations r
                      inner join Trips t on r.DepartFlightScheduleID = t.TripId
                      where t.Class = 2");

                List<Reservation> flights = query.ToList();

                Assert.AreEqual(flights.Count, 1);
                Assert.AreEqual(flights[0].ConfirmationCode, "1234");
            }
        }
    }
}