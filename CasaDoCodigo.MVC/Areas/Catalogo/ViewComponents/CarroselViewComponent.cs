using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
