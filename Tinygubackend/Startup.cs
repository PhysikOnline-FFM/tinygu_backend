using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Text;
using Tinygubackend;
using Tinygubackend.Contexts;
using Tinygubackend.Infrastructure;
using Tinygubackend.Models;
using Tinygubackend.Services;

#pragma warning disable 1591

namespace Tinygubackend
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional : false, reloadOnChange : true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional : true)
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = Configuration["Jwt:Issuer"],
          ValidAudience = Configuration["Jwt:Issuer"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
          };
        });
      // Add framework services.
      services.AddMvc();
      services.AddDbContext<TinyguContext>(options =>
        options.UseMySql(Configuration.GetConnectionString("Tinygu")));

      services.AddTransient<ILinksRepository, LinksRepository>();
      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<IAuthService, AuthService>();

      services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowCredentials()
        .AllowAnyHeader()));

      // Register the Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(c =>
      {
        c.AddSecurityDefinition("jwt", new ApiKeyScheme
        {
          Name = "Authorization",
            In = "header"
        });
        c.SwaggerDoc("v1", new Info { Title = "Tinygu", Version = "v1" });

        // Set the comments path for the Swagger JSON and UI.
        var basePath = AppContext.BaseDirectory;
        Console.WriteLine(basePath);
        var xmlPath = Path.Combine(basePath, "Tinygu.xml");
        //c.IncludeXmlComments(xmlPath);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tinygu V1");
      });

      app.UseAuthentication();

      app.UseCors("AllowAll");

      app.UseMvc();
    }
  }
}