using System;
using System.Text;
using AutoMapper;
using Manager.API.Tokens;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.Dtos;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Manager.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      #region AutoMapper

      var autoMapperConfig = new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<User, UserDto>().ReverseMap();
        cfg.CreateMap<CreateUserViewModel, UserDto>().ReverseMap();
        cfg.CreateMap<UpdateUserViewModel, UserDto>().ReverseMap();
      });

      services.AddSingleton(autoMapperConfig.CreateMapper());

      #endregion

      #region Dependencies Injection

      services.AddSingleton(d => Configuration);
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<ITokenGenerator, TokenGenerator>();

      services.AddDbContext<ManagerContext>(options =>
      {
        var connectionString = Configuration.GetConnectionString("USER_MANAGER");

        options.UseSqlServer(connectionString);
      });

      #endregion

      #region JWT

      var secretKey = Configuration["Jwt:Key"];

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

      #endregion

      # region SWAGGER

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "Manager API",
          Version = "v1",
          Description = "API construída na serie de vídeos no canal Lucas Eschechola.",
          Contact = new OpenApiContact
          {
            Name = "Marco Antônio",
            Email = "rootslowed@gmail.com",
            Url = new Uri("https://eschechola.com.br")
          },
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description = "Missing Bearer <TOKEN>",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
          },
          new string[] { }
        }
      });
      });

      # endregion
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseDeveloperExceptionPage();

      app.UseSwagger();

      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manager.API v1"));

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}