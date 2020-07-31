using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models.ViewsModel
{
    public class CatalogoViewModel
    {
        public CatalogoViewModel(IList<Produto> produtos, string pesquisa)
        {
            Produtos = produtos;
            Pesquisa = pesquisa;
        }

        public IList<Produto> Produtos { get; }
        public string Pesquisa { get; set; }
    }
}
