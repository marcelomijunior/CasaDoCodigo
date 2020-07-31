using CasaDoCodigo.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.ViewComponents
{
    public class CardProdutoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Produto produto)
        {
            return View("Default", produto);
        }
    }
}
