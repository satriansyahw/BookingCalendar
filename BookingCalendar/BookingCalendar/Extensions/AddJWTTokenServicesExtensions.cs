using BookingCalendar.Models.Instance;
using BookingCalendar.Models.Interface;
using BookingCalendar.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
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
            

            Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = bindJwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),
                };
            
                options.Events = new JwtBearerEvents
                {
                    
                    OnAuthenticationFailed= CheckValidationError,
                    OnTokenValidated = AdditionalValidation,
                    OnMessageReceived=Message ,
                    OnChallenge= Chakee


                };
             



            });
        }
        static Task Chakee(JwtBearerChallengeContext context)
        {
            return Task.CompletedTask;

        }
        static  Task AdditionalValidation(TokenValidatedContext context)
        {
            ILogin loginDao = InsLogin.GetLogin();
            string  un = context.Principal.Claims.Where(a => a.Type == "un").FirstOrDefault().Value.ToString();
            bool isLogin =  loginDao.IsLoginIn(un).GetAwaiter().GetResult();
            var tokenRefresh = (context.SecurityToken as JwtSecurityToken).RawData;
            
          if (!isLogin)
                context.Fail("Error Token validation");
            bool isLogin2 = loginDao.IsLoginIn(un, tokenRefresh).GetAwaiter().GetResult();
            if ((!context.Request.Path.Equals("/api/login/refresh") && isLogin2)
                | (context.Request.Path.Equals("/api/login/refresh") && !isLogin2)

                )
            {
               
                    context.Fail("Error Token validation");
            }
            return Task.CompletedTask;
        }
        static Task CheckValidationError(AuthenticationFailedContext context)
        {
            //ILogin loginDao = InsLogin.GetLogin();
            //string un = context.Principal.Claims.Where(a => a.Type == "un").FirstOrDefault().Value.ToString();
            //bool isLogin = loginDao.IsLoginIn(un).GetAwaiter().GetResult();
            //if (!isLogin)
            //    context.Fail("Error Token validation");

            return Task.CompletedTask;
        }

        static Task Message(MessageReceivedContext context)
        {
            var sss = context.Token;
            //ILogin loginDao = InsLogin.GetLogin();
            //string un = context.Principal.Claims.Where(a => a.Type == "un").FirstOrDefault().Value.ToString();
            //bool isLogin = loginDao.IsLoginIn(un).GetAwaiter().GetResult();
            //if (!isLogin)
            //    context.Fail("Error Token validation");
      return Task.CompletedTask;
        }
    }
}

