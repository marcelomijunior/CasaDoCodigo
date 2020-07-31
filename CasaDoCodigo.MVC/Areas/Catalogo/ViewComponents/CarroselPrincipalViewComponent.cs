using CasaDoCodigo.Areas.Catalogo.Models.ViewModels;
using CasaDoCodigo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.ViewComponents
{
    public class CarroselPrincipalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Categoria categoria, IList<Produto> produtos, int tamanhoDaPagina)
        {
            var produtosDaCategoria = produtos
                .Where(p => p.Categoria.Id == categoria.Id)
                .ToList();

            int numeroDePaginas = (int)Math.Ceiling((double)produtosDaCategoria.Count() / tamanhoDaPagina);

            return View("Default", new CarroselPrincipalViewModel(categoria, produtosDaCategoria, tamanhoDaPagina, numeroDePaginas));
        }
    }
}
