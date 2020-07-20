using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repository.Interfaces
{
    public interface IProdutoRepository
    {
        Task SaveProdutosAsync(List<Livro> livros);
        Task<IList<Produto>> GetProdutosAsync();
        Task<BuscaProdutosViewModel> GetProdutosAsync(string pesquisa);
    }
}
