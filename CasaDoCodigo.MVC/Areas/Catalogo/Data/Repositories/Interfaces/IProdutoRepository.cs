using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Data.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task SaveProdutosAsync(List<Livro> livros);
        Task<IList<Produto>> GetProdutosAsync();
        Task<Produto> GetProdutoAsync(string codigo);
        Task<CatalogoViewModel> GetProdutosAsync(string pesquisa);
    }
}
