using System;
using System.Reflection;
using BookingCalendar.Models.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingCalendar.Models.Dao
{
	public class DaoLogin: ILogin
	{
        private readonly EnitamContext context = new EnitamContext();
        public bool IsLoginIn(string userName)
        {
            var cari = (from a in context.Login
                        where a.UserName == userName
                        select a).ToList();
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
    }
}

