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
using TimeOffManagementAPI.Data.Model.Mappings;
using TimeOffManagementAPI.Data.Model.Models;
using AutoMapper;


namespace TimeOffManagementAPI.Web.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddJsonFile(this IServiceCollection services, IConfigurationBuilder builder)
    {
        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);
        builder.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
        // builder.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TimeOffManagementDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TimeOffManagementDBContext")));
    }

    public static void AddCustomMvc(this IServiceCollection services)
    {
        services.AddMvc(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void AddMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(map =>
        {
            map.AddProfile<UserMappingProfile>();
            map.AddProfile<TimeOffMappingProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o =>
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
    }

    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }

    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeOffManagementAPI.Web", Version = "v1" });
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
    }

    public static void AddScoped(this IServiceCollection services)
    {
        services.AddScoped<ITimeOffRepository, TimeOffRepository>();
        services.AddScoped<ITimeOffService, TimeOffService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICalendarService, CalendarService>();
    }

    public static void AddHostedService(this IServiceCollection services)
    {
        services.AddHostedService<AnnualTimeOffBackgroundService>();
    }

    public static void AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
