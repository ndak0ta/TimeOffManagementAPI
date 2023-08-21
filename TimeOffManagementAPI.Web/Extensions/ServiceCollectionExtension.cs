using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TimeOffManagementAPI.Web.Filters;
using TimeOffManagementAPI.Business.BackgroundServices;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Repositories;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Mappings;
using TimeOffManagementAPI.Data.Model.Models;
using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Business.Auth.Commands;

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
        var keyString = configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(keyString))
            throw new ArgumentNullException("Jwt:Key");

        var key = Encoding.ASCII.GetBytes(keyString);

        services.AddAuthentication(options =>
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
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                NameClaimType = JwtRegisteredClaimNames.Sub
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
    }

    public static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<LoginCommand>();
        });
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
