using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using Microsoft.AspNetCore.Mvc;

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
