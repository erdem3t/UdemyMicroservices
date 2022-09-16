using FreeCourse.Services.Payment.Consumers;
using FreeCourse.Services.Payment.Extensions;
using FreeCourse.Shared.Settings;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace FreeCourse.Services.Payment
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
                x.AddConsumer<StockReservedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ReceiveEndpoint(RabbitMQSettingsConst.StockReservedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<StockReservedEventConsumer>(context);
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
                opt.Audience = "resource_payment"; // Jwt payload audience parametresinde resource_catalog bekliyor
                opt.RequireHttpsMetadata = false; // Http isteklerine izin verilmesi 
            });
            #endregion

            #region Consul
            services.ConfigureConsul(Configuration);
            #endregion

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Payment", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Payment v1"));
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
