using CasaDoCodigo.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels
{
    public class CarroselViewModel
    {
        public CarroselViewModel(IList<Produto> produtos, int indice)
        {
            Produtos = produtos;
            Indice = indice;
        }

        public IList<Produto> Produtos { get; set; }
        public int Indice { get; set; }
    }
}
