using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewsModel;
using CasaDoCodigo.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasaDoCodigo.MVC.Areas.Carrinho.Controllers
{
    [Area("Carrinho")]
    public class HomeController : Controller
    {
        private readonly IPedidoRepository pedidoRepository;

        public HomeController(IPedidoRepository pedidoRepository)
        {
            this.pedidoRepository = pedidoRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                await pedidoRepository.AddItemAsync(codigo);
            }

            var pedido = await pedidoRepository.GetPedidoAsync();

            List<ItemPedido> itens = pedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);

            return base.View(carrinhoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<UpdateQuantidadeResponse> UpdateQuantidade([FromBody] ItemPedido itemPedido)
        {
            return await pedidoRepository.UpdateQuantidadeAsync(itemPedido);
        }
    }
}
