using System.Collections.Generic;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels
{
    public class CarroselPrincipalViewModel
    {
        public CarroselPrincipalViewModel(Categoria categoria, IList<Produto> produtos, int tamanhoDaPagina, int numeroDePaginas)
        {
            Categoria = categoria;
            Produtos = produtos;
            TamanhoDaPagina = tamanhoDaPagina;
            NumeroDePaginas = numeroDePaginas;
        }

        public Categoria Categoria { get; set; }
        public IList<Produto> Produtos { get; set; }
        public int TamanhoDaPagina { get; set; }
        public int NumeroDePaginas { get; set; }
    }
}
