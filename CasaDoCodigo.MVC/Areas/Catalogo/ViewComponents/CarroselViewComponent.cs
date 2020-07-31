using CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels;
using CasaDoCodigo.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.ViewComponents
{
    public class CarroselViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IList<Produto> produtos, int indice, int tamanhoPagina)
        {
            var produtosNaPagina = produtos
                .Skip(indice * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            return View("Default", new CarroselViewModel(produtosNaPagina, indice));
        }
    }
}
