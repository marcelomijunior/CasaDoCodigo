using CasaDoCodigo.MVC.Areas.Catalogo.Data.Repositories.Interfaces;
using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using CasaDoCodigo.MVC.Areas.Catalogo.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Data.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        static IList<Produto> listaProdutos;

        public ProdutoRepository(IConfiguration configuration, CatalogoDbContext contexto) : base(configuration, contexto)
        {
        }

        public async Task<Produto> GetProdutoAsync(string codigo)
        {
            return await dbSet
                .Where(p => p.Codigo == codigo)
                .Include(prod => prod.Categoria)
                .SingleOrDefaultAsync();
        }

        public async Task<IList<Produto>> GetProdutosAsync()
        {
            return await dbSet
                .Include(prod => prod.Categoria)
                .ToListAsync();
        }

        public async Task<CatalogoViewModel> GetProdutosAsync(string pesquisa)
        {
            if (listaProdutos == null)
            {
                listaProdutos =
                    await dbSet
                        .Include(prod => prod.Categoria)
                        .ToListAsync();
            }

            var resultado = listaProdutos;

            if (!string.IsNullOrEmpty(pesquisa))
            {
                pesquisa = pesquisa.ToLower();
                resultado = listaProdutos
                    .Where(q => q.Nome.ToLower().Contains(pesquisa) || q.Categoria.Nome.ToLower().Contains(pesquisa))
                    .ToList();
            }

            return new CatalogoViewModel(resultado, pesquisa);
        }

        //MELHORIA: 1) Métodos assíncronos
        //Para saber mais: C#: Paralelismo no mundo real
        //https://cursos.alura.com.br/course/csharp-paralelismo-no-mundo-real/task/27900
        public async Task SaveProdutosAsync(List<Livro> livros)
        {
            await SaveCategorias(livros);

            foreach (var livro in livros)
            {
                var categoria =
                    await catalogoDbContext.Set<Categoria>()
                        .SingleAsync(c => c.Nome == livro.Categoria);

                if (!await dbSet.Where(p => p.Codigo == livro.Codigo).AnyAsync())
                {
                    await dbSet.AddAsync(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria));
                }
            }
            await catalogoDbContext.SaveChangesAsync();
        }

        private async Task SaveCategorias(List<Livro> livros)
        {
            var categorias =
                livros
                    .OrderBy(l => l.Categoria)
                    .Select(l => l.Categoria)
                    .Distinct();

            foreach (var nomeCategoria in categorias)
            {
                var categoriaDB =
                    await catalogoDbContext.Set<Categoria>()
                    .SingleOrDefaultAsync(c => c.Nome == nomeCategoria);
                if (categoriaDB == null)
                {
                    await catalogoDbContext.Set<Categoria>()
                        .AddAsync(new Categoria(nomeCategoria));
                }
            }
            await catalogoDbContext.SaveChangesAsync();
        }
    }
}
