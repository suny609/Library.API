using AutoMapper;
using Library.API.Entities;
using Library.API.Filters;
using Library.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Library.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.EnableEndpointRouting = false;
                //config.Filters.Add<JsonExceptionFilter>();

                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                config.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 60
                    });

                config.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<LibraryDbContext>(config => config.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<CheckAuthorExistFilterAttribute>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddDefaultTokenProviders();

            var tokenSection = Configuration.GetSection("Security:Token");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = tokenSection["Issuer"],
                      ValidAudience = tokenSection["Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSection["Key"])),
                      ClockSkew = TimeSpan.Zero
                  };
              });
        }
// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            IMapper Mapper,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseMvc();
        }
    }
}