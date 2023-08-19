using TimeOffManagementAPI.Web.Extensions;


var builder = WebApplication.CreateBuilder(args);


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

app.UseCors("AllowAll");

app.Run();

