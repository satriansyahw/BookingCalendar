using BookingCalendar.Dto.Response;
using BookingCalendar.Models.Domain;
using BookingCalendar.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookingCalendar.Models.Dao
{
    public class DaoKalendar : IKalendar
    {
        private readonly EnitamContext context = new EnitamContext();
        public async Task<bool> Delete(long calendarId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Kalendar cal = new Kalendar() { Id = calendarId };
                    context.Entry(cal).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return false;           

        }

        public async Task<Kalendar> Get(string userName, long calendarId)
        {
            var cal =  await (from a in context.Kalendar
                          where a.UserName == userName && a.Id == calendarId
                          select a).FirstOrDefaultAsync();
            return cal;
        }

        public async Task<List<KalendarResDto>> Get(string userName)
        {
            List<KalendarResDto> listCall = await (from a in context.Kalendar
                                  where a.UserName == userName
                                    select new KalendarResDto
                                    {
                                        CalDate = a.CalDate.ToString("yyyy-MM-dd"),
                                        CalTimeStart = a.CalTimeStart.ToString("HH:mm"),
                                        CalTimeEnd = a.CallTimeEnd.ToString("HH:mm"),
                                        EventName = a.EventName,
                                        Id = a.Id,
                                        IsAllDay = a.IsAllDay,
                                        UserName = a.UserName
                                    }

                                  ).ToListAsync();
            return listCall;
        }

        public async Task<bool> IsAlreadyExist(Kalendar item)
        {
            List<Kalendar> listCalAllDay = await (from a in context.Kalendar
                                  where a.UserName == item.UserName && a.CalDate == item.CalDate
                                  && a.IsAllDay == true
                                  select a).ToListAsync();
            List<Kalendar> listCal = await (from a in context.Kalendar
                                       where a.UserName == item.UserName && a.CalDate == item.CalDate
                                       && a.CalTimeStart >= item.CalTimeStart && a.CallTimeEnd <= item.CallTimeEnd
                                       select a).ToListAsync();
            if(listCalAllDay !=null && listCalAllDay.Count > 0)
                return true;
            if (listCal != null && listCal.Count > 0)
                return true;

            return false;
        }

        public async Task<bool> IsAlreadyExistByEventName(string userName, string eventName)
        {
            var cal = await(from a in context.Kalendar
                            where a.UserName == userName && a.EventName.Trim().ToLower() == eventName.Trim().ToLower()
                            select a).FirstOrDefaultAsync();
            if (cal != null)
                return true;
            return false;
        }

        public async Task<Kalendar> Save(Kalendar item)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Add(item);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return item;
        }

        public async Task<Kalendar> Update(Kalendar item)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Attach(item);
                    context.Entry(item).Property("EventName").IsModified = true;
                    context.Entry(item).Property("CalDate").IsModified = true;
                    context.Entry(item).Property("CalTimeStart").IsModified = true;
                    context.Entry(item).Property("CallTimeEnd").IsModified = true;
                    context.Entry(item).Property("IsAllDay").IsModified = true;
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    return item;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return null;
        }
    }

}
