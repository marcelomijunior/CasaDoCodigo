using CasaDoCodigo.MVC.Models;
using CasaDoCodigo.MVC.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Repository.Interfaces
{
    public interface IProdutoRepository
    {
        Task SaveProdutosAsync(List<Livro> livros);
        Task<IList<Produto>> GetProdutosAsync();
        Task<CatalogoViewModel> GetProdutosAsync(string pesquisa);
    }
}
