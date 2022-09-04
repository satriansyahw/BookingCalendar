using BookingCalendar.Extensions;
using BookingCalendar.Models;
using BookingCalendar.Models.Dao;
using BookingCalendar.Models.Interface;
using BookingCalendar.UseCase;
using BookingCalendar.Utils;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddJWTTokenServices(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddSingleton<ILoginUseCase, LoginUseCase>();
builder.Services.AddTransient<ILogin, DaoLogin>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EnitamContext>(opt =>
    opt.UseInMemoryDatabase("EnitamDB"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("MyCorsPolicy");

app.Run();
