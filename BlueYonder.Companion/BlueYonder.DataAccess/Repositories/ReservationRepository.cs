using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;

namespace BlueYonder.DataAccess.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        TravelCompanionContext context;

        public ReservationRepository(string connectionName)
        {
            context = new TravelCompanionContext(connectionName);
        }

        public ReservationRepository()
        {
            context = new TravelCompanionContext();
        }

        public ReservationRepository(TravelCompanionContext dbContext)
        {
            context = dbContext;
        }

        public Reservation GetSingle(int entityKey)
        {
            return context.Reservations.SingleOrDefault(item => item.ReservationId == entityKey);
        }

        public IQueryable<Reservation> GetAll()
        {             
            return context.Reservations.AsQueryable<Reservation>();
        }

        public IQueryable<Reservation> FindBy(Expression<Func<Reservation, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public void Add(Reservation entity)
        {
            context.Reservations.Add(entity);
        }

        public void Delete(Reservation entity)
        {
            context.Reservations.Find(entity.ReservationId);
            if (entity.DepartFlightScheduleID != 0)
                context.Entry(entity.DepartureFlight).State = System.Data.EntityState.Deleted;
            if (entity.ReturnFlightScheduleID != 0)
                context.Entry(entity.ReturnFlight).State = System.Data.EntityState.Deleted;
            context.Reservations.Remove(entity);
        }

        public void Edit(Reservation entity)
        {
            var origin = context.Reservations.Find(entity.ReservationId);
            context.Entry(origin).CurrentValues.SetValues(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }
        
        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }
       
    }
}
