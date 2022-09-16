using FreeCourse.Discount.Extensions;
using FreeCourse.Discount.Services;
using FreeCourse.Discount.ServicesContract;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Discount
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
            #region Jwt
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); // Bu ayar Claimlerden Sub => nameIdentifer ismine dönüþmesini engeller
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerUrl"]; // Yetkilendirme hangi urlden verilecek
                opt.Audience = "resource_discount"; // Jwt payload audience parametresinde resource_discount bekliyor
                opt.RequireHttpsMetadata = false; // Http isteklerine izin verilmesi 
            });
            #endregion

            services.AddHttpContextAccessor();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IDiscountService, DiscountService>();

            var requiredUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); // Gelen tokenda user parametresi beklemesini 

            // new AuthorizationPolicyBuilder().RequireClaim("scope", "discount-read"); custom policy oluþturma

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requiredUserPolicy));
            });

            #region Consul
            services.ConfigureConsul(Configuration);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.RegisterWithConsul(lifetime,Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
