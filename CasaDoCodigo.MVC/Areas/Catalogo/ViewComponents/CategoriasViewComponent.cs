using CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels;
using CasaDoCodigo.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.ViewComponents
{
    public class CategoriasViewComponent : ViewComponent
    {
        private const int TamanhoPagina = 4;

        public IViewComponentResult Invoke(IList<Produto> produtos)
        {
            var categorias = produtos
                .Select(m => m.Categoria)
                .Distinct()
                .ToList();

            return View("Default", new CategoriasViewModel(categorias, produtos, TamanhoPagina));
        }
    }
}
