using FreeCourse.Services.Basket.Consumers;
using FreeCourse.Services.Basket.Extensions;
using FreeCourse.Services.Basket.Redis;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.ServicesContract;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace FreeCourse.Services.Basket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CourseNameChangedEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                    cfg.ReceiveEndpoint("course-name-changed-event-basket-service", e =>
                    {
                        e.ConfigureConsumer<CourseNameChangedEventConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            var requiredUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); // Gelen tokenda user parametresi beklemesini söyleyen policy döner;
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requiredUserPolicy));
            });


            #region Jwt
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); // Bu ayar Claimlerden Sub => nameIdentifer ismine dönüþmesini engeller
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerUrl"]; // Yetkilendirme hangi urlden verilecek
                opt.Audience = "resource_basket"; // Jwt payload audience parametresinde resource_catalog bekliyor
                opt.RequireHttpsMetadata = false; // Http isteklerine izin verilmesi 
            });
            #endregion

            services.AddHttpContextAccessor();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IBasketService, BasketService>();

            #region Redis Configuration (Options Pattern)

            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));
            services.AddSingleton<IRedisService>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
                var service = new RedisService(redisSettings);
                service.Connect();
                return service;
            });

            #endregion

            #region Consul
            services.ConfigureConsul(Configuration);
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Basket", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Basket v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.RegisterWithConsul(lifetime, Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
