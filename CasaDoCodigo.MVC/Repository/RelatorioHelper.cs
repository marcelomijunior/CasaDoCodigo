using CasaDoCodigo.MVC.Models;
using CasaDoCodigo.MVC.Repository.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using IdentityModel.Client;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System.Diagnostics;

namespace CasaDoCodigo.MVC.Repository
{
    public class RelatorioHelper : IRelatorioHelper
    {
        private const string RELATIVEURI = "/api/relatorio";
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;

        public RelatorioHelper(IConfiguration configuration, HttpClient httpClient)
        {
            this.configuration = configuration;
            this.httpClient = httpClient;
        }

        public async Task GerarRelatorio(Pedido pedido)
        {
            string textoRealtorio = await GetTextoRelatorio(pedido);
            //await System.IO.File.AppendAllLinesAsync("Relatorio.txt", new string[] { linhaRelatorio });

            //using (HttpClient httpClient = new HttpClient())
            //using (HttpClient httpClient = HttpClientFactory.Create())
            //{
            var json = JsonConvert.SerializeObject(textoRealtorio);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(configuration["IdentityServer4Url"]);

            if (discoveryResponse.IsError)
            {
                throw new ApplicationException(discoveryResponse.Error);
            }

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = discoveryResponse.TokenEndpoint,
                    ClientId = "CasaDoCodigo.MVC",
                    ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0",
                    Scope = "CasaDoCodigo.RelatorioWebApi"
                });

            if (tokenResponse.IsError)
            {
                Debug.WriteLine(tokenResponse.Error);
                return;
            }

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            Uri baseUri = new Uri(configuration["ApiRelatorioUrl"]);
            Uri uri = new Uri(baseUri, RELATIVEURI);

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(uri, httpContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new ApplicationException(httpResponseMessage.ReasonPhrase);
            }
            //}
        }

        private async Task<string> GetTextoRelatorio(Pedido pedido)
        {
            StringBuilder sb = new StringBuilder();

            string templatePedido = await System.IO.File.ReadAllTextAsync("TemplatePedido.txt");
            string templateItemPedido = await System.IO.File.ReadAllTextAsync("TemplateItemPedido.txt");

            string linhaPedido = string.Format(
                templatePedido,
                pedido.Id,
                pedido.Cadastro.Nome,
                pedido.Cadastro.Endereco,
                pedido.Cadastro.Complemento,
                pedido.Cadastro.Bairro,
                pedido.Cadastro.Municipio,
                pedido.Cadastro.UF,
                pedido.Cadastro.Telefone,
                pedido.Cadastro.Email,
                pedido.Itens.Sum(i => i.Subtotal)
                );

            sb.AppendLine(linhaPedido);

            foreach (var i in pedido.Itens)
            {
                string linhaItemPedido = string.Format(
                    templateItemPedido,
                    i.CodigoProduto,
                    i.PrecoUnitario,
                    i.NomeProduto,
                    i.Quantidade,
                    i.Subtotal
                    );

                sb.AppendLine(linhaItemPedido);
            }

            sb.AppendLine($@"=============================================");

            return sb.ToString();
        }
    }
}
