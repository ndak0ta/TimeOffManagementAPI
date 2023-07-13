using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Web.Filters;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Repositories;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Business.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddDbContext<TimeOffManagementDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TimeOffManagementDBContext")));

builder.Services.AddScoped<ITimeOffRepository, TimeOffRepository>();
builder.Services.AddScoped<ITimeOffService, TimeOffService>();

builder.Services.AddMvc(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

