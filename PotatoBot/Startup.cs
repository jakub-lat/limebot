using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PotatoBot.Bot;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using PotatoBot.Models;
using PotatoBot.Bot.Utils;
using DAL;
using VueCliMiddleware;
using Microsoft.AspNetCore.SpaServices;
using kedzior.io.ConnectionStringConverter;

namespace PotatoBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public BotService bot;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // load env variables
            Config.Load();

            // api
            services.AddControllers();
            services.AddSpaStaticFiles(opt => opt.RootPath = "clientapp/dist");


            var connString = Configuration.GetConnectionString("DefaultConnection");
            

            // database
            services.AddDbContext<GuildContext>(options=>
                options.UseMySql(connString));

            // bot
            var serviceProvider = services.BuildServiceProvider();
            bot = new BotService(serviceProvider, connString);
            services.AddSingleton(bot);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(config =>
            {
                config.Options.SourcePath = "clientapp";
                if (env.IsDevelopment())
                {
                    config.UseProxyToSpaDevelopmentServer("http://localhost:8080");
                }
            });
        }
    }
}

/*
    Add migration:    dotnet ef migrations add Logs --startup-project PotatoBot --project DAL
    Update database:  dotnet ef database update --project PotatoBot
*/
