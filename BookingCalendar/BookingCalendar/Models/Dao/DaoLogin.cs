using System;
using System.Reflection;
using BookingCalendar.Models.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookingCalendar.Models.Dao
{
	public class DaoLogin: ILogin
	{
        private readonly EnitamContext context;

        public DaoLogin(EnitamContext context) {
            this.context = context;
        }

        public bool IsLoginIn(string userName)
        {
            throw new NotImplementedException();
        }

        public Login Save(Login item)
        {
            throw new NotImplementedException();
        }
    }
}

