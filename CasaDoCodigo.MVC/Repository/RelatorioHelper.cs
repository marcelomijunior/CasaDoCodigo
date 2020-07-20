using CasaDoCodigo.Models;
using CasaDoCodigoMVC.Repository.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CasaDoCodigoMVC
{
    public class RelatorioHelper : IRelatorioHelper
    {
        public async Task GerarRelatorio(Pedido pedido)
        {
            string textoRealtorio = await GetTextoRelatorio(pedido);
            //await System.IO.File.AppendAllLinesAsync("Relatorio.txt", new string[] { linhaRelatorio });
            
            using (HttpClient httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(textoRealtorio);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                Uri uri = new Uri("http://localhost:5011/api/relatorio");

                await httpClient.PostAsync(uri, httpContent);
            }
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
                    i.Produto.Codigo,
                    i.PrecoUnitario,
                    i.Produto.Nome,
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
