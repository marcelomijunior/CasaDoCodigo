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
        public IViewComponentResult Invoke(IList<Produto> produtosDaCategoria, int indice, int tamanhoDaPagina)
        {
            var produtosNaPagina = produtosDaCategoria
                .Skip(indice * tamanhoDaPagina)
                .Take(tamanhoDaPagina)
                .ToList();

            return View("Default", new CarroselViewModel(produtosNaPagina, indice));
        }
    }
}
