using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Repository;
using CasaDoCodigo.Repository.Interfaces;
using CasaDoCodigoMVC;
using CasaDoCodigoMVC.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CasaDoCodigo
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;

        public Startup(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession();

            string connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHttpHelper, HttpHelper>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
            services.AddTransient<ICadastroRepository, CadastroRepository>();
            services.AddTransient<IRelatorioHelper, RelatorioHelper>();

            services.AddAuthentication().AddMicrosoftAccount(options => 
            {
                options.ClientId = Configuration["ExternalLogin:Microsoft:ClientId"];
                options.ClientSecret = Configuration["ExternalLogin:Microsoft:ClientSecret"];
            });
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration["ExternalLogin:Google:ClientId"];
                options.ClientSecret = Configuration["ExternalLogin:Google:ClientSecret"];
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            _loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=BuscaProdutos}/{codigo?}");
            });

            var dataService = serviceProvider.GetRequiredService<IDataService>();
            dataService.InicializaDBAsync(serviceProvider).Wait();
        }
    }
}
