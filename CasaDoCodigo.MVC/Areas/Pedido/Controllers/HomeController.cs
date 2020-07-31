using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.MVC.Areas.Identity.Data;
using CasaDoCodigo.MVC.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CasaDoCodigo.MVC.Areas.Pedido.Controllers
{
    [Area("Pedido")]
    public class HomeController : Controller
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly UserManager<AppIdentityUser> userManager;

        public HomeController(IPedidoRepository pedidoRepository, UserManager<AppIdentityUser> userManager)
        {
            this.pedidoRepository = pedidoRepository;
            this.userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Index(Models.Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                var usuario = await userManager.GetUserAsync(this.User);

                usuario.Nome = cadastro.Nome;
                usuario.Email = cadastro.Email;
                usuario.Telefone = cadastro.Telefone;
                usuario.Endereco = cadastro.Endereco;
                usuario.Complemento = cadastro.Complemento;
                usuario.Bairro = cadastro.Bairro;
                usuario.Municipio = cadastro.Municipio;
                usuario.UF = cadastro.UF;
                usuario.CEP = cadastro.CEP;

                await userManager.UpdateAsync(usuario);

                return View(await pedidoRepository.UpdateCadastroAsync(cadastro));
            }

            return RedirectToAction("/Cadastro");
        }
    }
}
