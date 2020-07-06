
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatProducer.Persistence.Contexts;
using ChatProducer.Persistence.Repositories;
using ChatProducer.Persistence.Repositories.Interface;
using ChatProducer.Services;
using ChatProducer.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ChatProducer.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ChatProducer.Extensions;
using ChatProducer.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace ChatProducer
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
            services.AddDbContext<AppDbContext>(opt => {
                opt.UseInMemoryDatabase("client-chat");
            }); 

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddHealthChecks();
            services.AddControllers();

            services.AddTokenAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });

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
