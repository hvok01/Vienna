﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Vienna.Models;

namespace Vienna
{
    public class Startup
    {

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login";
                    options.LogoutPath = "/Home/Logout";
                    //antes era restringido pero no tenemos xd
                    options.AccessDeniedPath = "/Home/Index";
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["TokenAuthentication:Issuer"],
                        ValidAudience = configuration["TokenAuthentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"])),
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Empleado"));
                options.AddPolicy("DuenioEvento", policy => policy.RequireClaim(ClaimTypes.Role, "DuenioEvento"));
            });
            services.AddMvc();
            //services.AddTransient<IRepositorio<Empleado>, RepositorioEmpleado>();
            //services.AddTransient<IRepositorioEmpleado, RepositorioEmpleado>();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
            });
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
