using Library.API.Entities;
using Library.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using Library.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Library.API.Helpers;

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

            services.AddControllers(config =>
            {
                config.ReturnHttpNotAcceptable = true;
                
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).AddXmlSerializerFormatters();

            services.AddAuthorization(options => {
                options.AddPolicy("ManagerOnly", builder => builder.RequireClaim("ManagerId")); 
                options.AddPolicy("LimitedUsers", builder => builder.RequireClaim("UserId", new string[] { "1", "2", "3" }));
                options.AddPolicy("RegisteredMoreThan3Days", builder => builder.Requirements.Add(new RegisteredMoreThan3DaysRequirement()));
            });
            //services.AddMvc(config =>
            //{
            //    config.EnableEndpointRouting = false;

            //    config.ReturnHttpNotAcceptable = true;
            //    config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            //});

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
			services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<LibraryDbContext>(option => option.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                optionBuilder => optionBuilder.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name)));
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
			

            app.UseHttpsRedirection();

            // app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
