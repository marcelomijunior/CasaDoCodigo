using System;
using System.IdentityModel.Tokens.Jwt;
using CasaDoCodigo.MVC.Areas.Catalogo.Data;
using CasaDoCodigo.MVC.Data;
using CasaDoCodigo.MVC.Repository;
using CasaDoCodigo.MVC.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CasaDoCodigo.MVC
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

            ConfigureDbContext<ApplicationDbContext>(services, "Default");
            ConfigureDbContext<CatalogoDbContext>(services, "Catalogo");

            ConfigureDirectoryServices(services);

            ExternalAuthentications(services);
            //ConfigureIdentityServer4(services);
        }

        private void ConfigureIdentityServer4(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                // forma de autenticação local
                options.DefaultScheme = "Cookies";
                // protocolo que define o fluxo da autenticação
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie()
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = Configuration["CasaDoCodigo.IdentityServer4Url"];
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "CasaDoCodigo.MVC";
                    options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                });
        }

        private void ExternalAuthentications(IServiceCollection services)
        {
            services.AddAuthentication()
                            .AddMicrosoftAccount(options =>
                            {
                                options.ClientId = Configuration["externallogin:microsoft:clientid"];
                                options.ClientSecret = Configuration["externallogin:microsoft:clientsecret"];
                            }).AddGoogle(options =>
                            {
                                options.ClientId = Configuration["externallogin:google:clientid"];
                                options.ClientSecret = Configuration["externallogin:google:clientsecret"];
                            });
        }

        private static void ConfigureDirectoryServices(IServiceCollection services)
        {
            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHttpHelper, HttpHelper>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
            services.AddTransient<ICadastroRepository, CadastroRepository>();
            services.AddTransient<IRelatorioHelper, RelatorioHelper>();

            services.AddHttpClient<IRelatorioHelper, RelatorioHelper>();
        }

        private void ConfigureDbContext<T>(IServiceCollection services, string connectinoName) where T : DbContext
        {
            string connectionString = Configuration.GetConnectionString(connectinoName);

            services.AddDbContext<T>(options =>
                options.UseSqlServer(connectionString)
            );
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

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    name: "AreaCatalogo",
                    areaName: "Catalogo",
                    template: "Catalogo/{controller=Home}/{action=Index}/{pesquisa?}");

                routes.MapAreaRoute(
                    name: "AreaCarrinho",
                    areaName: "Carrinho",
                    template: "Carrinho/{controller=Home}/{action=Index}/{codigo?}");

                routes.MapAreaRoute(
                    name: "AreaCadastro",
                    areaName: "Cadastro",
                    template: "Cadastro/{controller=Home}/{action=Index}");

                routes.MapAreaRoute(
                    name: "AreaPedido",
                    areaName: "Pedido",
                    template: "Pedido/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            var dataService = serviceProvider.GetRequiredService<IDataService>();
            dataService.InicializaDBAsync(serviceProvider).Wait();
        }
    }
}
