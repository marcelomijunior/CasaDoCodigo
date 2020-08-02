using System.Collections.Generic;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels
{
    public class CategoriasViewModel
    {
        public CategoriasViewModel(IList<Categoria> categorias, IList<Produto> produtos, int tamanhoPagina)
        {
            Categorias = categorias;
            Produtos = produtos;
            TamanhoPagina = tamanhoPagina;
        }

        public IList<Categoria> Categorias{ get; set; }
        public IList<Produto> Produtos { get; set; }
        public int TamanhoPagina { get; set; }


    }
}
