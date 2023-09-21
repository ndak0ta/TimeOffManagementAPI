using TimeOffManagementAPI.Business.Seeders;
using TimeOffManagementAPI.Web.Extensions;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddJsonFile(builder.Configuration);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddCustomMvc();

builder.Services.AddMapper();

builder.Services.AddIdentity();

builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddCustomAuthorization();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddScoped();

builder.Services.AddMediatr();

builder.Services.AddHostedService();

builder.Services.AddHttpClient();

builder.Services.AddCustomCors();


WebApplication app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using IServiceScope scope = app.Services.CreateScope();
    await DbSeeder.SeedDevelopment(scope.ServiceProvider);
}
else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
    using IServiceScope scope = app.Services.CreateScope();
    await DbSeeder.Seed(scope.ServiceProvider);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.Run();

