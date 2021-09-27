using AutoMapper;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.Dtos;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Manager.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
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
      services.AddDbContext<ManagerContext>(
        options => options.UseSqlServer(Configuration["ConnectionStrings:USER_MANAGER"]), ServiceLifetime.Transient);
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IUserRepository, UserRepository>();

      #endregion

      services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Manager.API", Version = "v1"}); });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manager.API v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}