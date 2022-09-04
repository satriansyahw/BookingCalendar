using BookingCalendar.Models.Instance;
using BookingCalendar.Models.Interface;
using BookingCalendar.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BookingCalendar.Extensions
{
    public static class AddJWTTokenServicesExtensions
    {

        public static void AddJWTTokenServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            // Add Jwt Setings
            var bindJwtSettings = new JwtSettings();
            Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
            Services.AddSingleton(bindJwtSettings);


            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            //.AddJwtBearer(options => {
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            //    {
            //        ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
            //        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
            //        ValidateIssuer = bindJwtSettings.ValidateIssuer,
            //        ValidIssuer = bindJwtSettings.ValidIssuer,
            //        ValidateAudience = bindJwtSettings.ValidateAudience,
            //        ValidAudience = bindJwtSettings.ValidAudience,
            //        RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
            //        ValidateLifetime = bindJwtSettings.RequireExpirationTime,
            //        ClockSkew = TimeSpan.FromDays(1),
            //    };
            //    options.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = AdditionalValidation
            //    };
            //});
        }
        static  Task AdditionalValidation(TokenValidatedContext context)
        {
            ILogin loginDao = InsLogin.GetLogin();
            string  un = context.Principal.Claims.Where(a => a.Type == "un").FirstOrDefault().Value.ToString();
            bool isLogin =  loginDao.IsLoginIn(un).GetAwaiter().GetResult();
            if (!isLogin)
                context.Fail("Error Token validation");
            return Task.CompletedTask;
        }
    }
}
