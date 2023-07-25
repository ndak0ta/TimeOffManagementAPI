using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TimeOffManagementAPI.Web.Filters;
using TimeOffManagementAPI.Business.BackgroundServices;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Business.Services;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Repositories;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Access.Seeders;
using TimeOffManagementAPI.Data.Model.Mappings;
using TimeOffManagementAPI.Data.Model.Models;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<TimeOffManagementDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TimeOffManagementDBContext")));

builder.Services.AddMvc(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var mapperConfig = new MapperConfiguration(map =>
{
    map.AddProfile<UserMappingProfile>();
    map.AddProfile<TimeOffMappingProfile>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddIdentity<User, Role>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
    o.User.RequireUniqueEmail = true;
    o.Lockout.MaxFailedAccessAttempts = 5;
    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<TimeOffManagementDBContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ITimeOffRepository, TimeOffRepository>();
builder.Services.AddScoped<ITimeOffService, TimeOffService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ICalendarService, CalendarService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHostedService<AnnualTimeOffBackgroundService>();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

DbSeeder.SeedAsync(builder.Services.BuildServiceProvider().GetRequiredService<UserManager<User>>(),
 builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>()).Wait();

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

