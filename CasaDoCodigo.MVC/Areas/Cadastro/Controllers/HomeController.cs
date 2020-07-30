using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Areas.Identity.Data;
using CasaDoCodigo.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CasaDoCodigo.Areas.Cadastro.Controllers
{
    [Area("Cadastro")]
    public class HomeController : Controller
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly UserManager<AppIdentityUser> userManager;

        public HomeController(IPedidoRepository pedidoRepository, UserManager<AppIdentityUser> userManager)
        {
            this.pedidoRepository = pedidoRepository;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var pedido = await pedidoRepository.GetPedidoAsync();

            if (pedido == null)
            {
                return RedirectToAction("/");
            }

            var usuario = await userManager.GetUserAsync(User);

            pedido.Cadastro.Nome = usuario.Nome;
            pedido.Cadastro.Email = usuario.Email;
            pedido.Cadastro.Telefone = usuario.Telefone;
            pedido.Cadastro.Endereco = usuario.Endereco;
            pedido.Cadastro.Complemento = usuario.Complemento;
            pedido.Cadastro.Bairro = usuario.Bairro;
            pedido.Cadastro.Municipio = usuario.Municipio;
            pedido.Cadastro.UF = usuario.UF;
            pedido.Cadastro.CEP = usuario.CEP;

            //pedido.Cadastro.Nome = User.FindFirst("name")?.Value;

            return View(pedido.Cadastro);
        }
    }
}
