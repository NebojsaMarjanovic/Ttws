using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TTWSApi
{
    public class Startup
    {
        public Startup(IHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        private IHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers().AddXmlSerializerFormatters();

            services.AddHttpClient<Symbol>((HttpClient client) =>
            {

                if (CurrentEnvironment.IsStaging())
                {
                    client.BaseAddress = new Uri("http://ttwsrelaytest.ttweb.net/ttws-net/");
                }
                else if (CurrentEnvironment.IsProduction())
                {
                    client.BaseAddress = new Uri("http://ttwsxml.ttweb.net/ttws-net/");

                }
                else if (CurrentEnvironment.IsDevelopment())
                {
                    client.BaseAddress = new Uri("http://bgttws10/ttws-net/");

                }

                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "ExchangeRateViewer");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TTWSApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TTWSApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
