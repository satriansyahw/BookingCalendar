using BookingCalendar.Dto.Response;
using BookingCalendar.Models.Domain;
using BookingCalendar.Models.Interface;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookingCalendar.Models.Dao
{
    public class DaoKalendar : IKalendar
    {
        private readonly EnitamContext context = new EnitamContext();
        private readonly ILogger logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        }).CreateLogger<DaoKalendar>();
        public async Task<bool> Delete(long calendarId)
        {
            if (calendarId > 0)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Kalendar? checker = await context.Kalendar.FindAsync(calendarId);
                        if (checker != null)
                        {
                            Kalendar cal = new Kalendar() { Id = calendarId };
                            context.Entry(cal).State = EntityState.Deleted;
                            await context.SaveChangesAsync();
                            transaction.Commit();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.StackTrace);
                        transaction.Rollback();
                    }
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
            List<Kalendar> listCalsss = await (from a in context.Kalendar select a).ToListAsync();
            List<Kalendar> listCalAllDay = await (from a in context.Kalendar
                                  where a.UserName == item.UserName && a.CalDate == item.CalDate
                                  && a.IsAllDay == true
                                  select a).ToListAsync();
            List<Kalendar> listCal = await (from a in context.Kalendar
                                       where a.UserName == item.UserName && a.CalDate == item.CalDate
                                       && (a.CalTimeStart.IsBetween(item.CalTimeStart,item.CallTimeEnd) | a.CallTimeEnd.IsBetween(item.CalTimeStart, item.CallTimeEnd))
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
            if (!string.IsNullOrEmpty(item.UserName) && !string.IsNullOrEmpty(item.EventName))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Event events = new Event();
                        events.EventCode = "code";
                        events.EventName =  "name";
                        context.Add(events);
                        item.EventId = events.Id;
                        context.Add(item);
                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)

                    {
                        logger.LogError(ex.StackTrace);
                        transaction.Rollback();
                    }
                }
            }
            return item;
        }

        public async Task<Kalendar> Update(Kalendar item)
        {
            if (item.Id > 0 && !string.IsNullOrEmpty(item.UserName) && !string.IsNullOrEmpty(item.EventName))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Kalendar? checker = await context.Kalendar.FindAsync(item.Id);
                        if (checker != null)
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
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.StackTrace);
                        transaction.Rollback();
                    }
                }
            }
            return null;
        }

        public async Task<KalEventResDto> Getx(string userName, long calendarId)
        {
            var ev = from a in context.Event select a;
            var calev2 = (from a in context.Kalendar select a);
            var cal = await (from a in context.Kalendar
                             join b in context.Event on a.EventId equals b.Id
                             where a.UserName == userName && a.Id == calendarId
                             select new KalEventResDto
                             {
                                 CalDate = a.CalDate.ToString("yyyy-MM-dd"),
                                 EventCode = b.EventCode,
                                 EventId = a.EventId
                               ,
                                 EventName = b.EventName,
                                 Id = a.Id,
                                 IsAllDay = a.IsAllDay,
                                 UserName = a.UserName
                             }).FirstOrDefaultAsync();

            return cal;
        }
        public async Task<KalEventResDto> Gety(string userName, long calendarId)
        {
            var ev = from a in context.Event select a;
            var calev2 = (from a in context.Kalendar select a);
            var cal = await (from a in context.Kalendar
                             join b in context.Event on a.EventId equals b.Id into tempTable
                             where a.UserName == userName && a.Id == calendarId
                             from temp in tempTable
                             select new KalEventResDto
                             {
                                 CalDate = a.CalDate.ToString("yyyy-MM-dd"),
                                 EventCode = temp.EventCode,
                                 EventId = a.EventId
                               ,
                                 EventName = temp.EventName,
                                 Id = a.Id,
                                 IsAllDay = a.IsAllDay,
                                 UserName = a.UserName
                             }).FirstOrDefaultAsync();

            return cal;
        }
    }

}
