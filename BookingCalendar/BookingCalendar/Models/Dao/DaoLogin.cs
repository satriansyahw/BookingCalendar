using System;
using System.Reflection;
using BookingCalendar.Models.Interface;
using BookingCalendar.UseCase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingCalendar.Models.Dao
{
	public class DaoLogin: ILogin
	{
        private readonly EnitamContext context = new EnitamContext();
        private readonly ILogger logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        }).CreateLogger<DaoLogin>();
        public async Task<bool> IsLoginIn(string userName)
        {
            var cari = await (from a in context.Login
                        where a.UserName == userName
                        select a).ToListAsync();
            logger.LogInformation("checking is loginIn..." + cari.Count.ToString());
            if (!cari.IsNullOrEmpty<Login>())
                return true;
            else
                return false;

        }

        public async Task<bool> IsLoginIn(string userName,string refreshToken)
        {
            var cari = await (from a in context.Login
                              where a.UserName.Equals(userName) && a.RefreshToken.Equals(refreshToken)
                              select a).ToListAsync();
            logger.LogInformation("checking is loginIn..." + cari.Count.ToString());
            if (!cari.IsNullOrEmpty<Login>())
                return true;
            else
                return false;
        }

            public async Task<Login> Save(Login item)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    logger.LogInformation("data", item);
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
            return item;
        }
    }
}

