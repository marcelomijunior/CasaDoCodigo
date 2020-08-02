using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Data
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            var produtos = GetProdutos();
            var categorias = produtos.Select(p => p.Categoria).Distinct();

            modelBuilder.Entity<Categoria>(ba =>
            {
                ba.HasKey(c => c.Id);
                ba.HasData(categorias); // propagaçaõ ou "seeding" de categorias.
            });

            modelBuilder.Entity<Produto>(ba =>
            {
                ba.HasKey(p => p.Id);
                ba.HasData(produtos.Select(p => new
                {
                    p.Id,
                    p.Codigo,
                    p.Nome,
                    p.Preco,
                    CategoriaId = p.Categoria.Id
                })); // propagaçaõ ou "seeding" de produtos.
            });
        }

        private IList<Livro> GetLivros()
        {
            var json = File.ReadAllText("data/Livros.json");

            return JsonConvert.DeserializeObject<IList<Livro>>(json);
        }

        private IList<Produto> GetProdutos()
        {
            var livros = GetLivros();

            var categorias = livros
                .Select(l => l.Categoria) // transformação de dados.
                .Distinct()
                .Select((nomeCategoria, indice) =>
                {
                    var categoria = new Categoria(nomeCategoria);
                    categoria.Id = indice + 1;

                    return categoria;
                });

            var produtos = (from livro in livros join categoria in categorias
                           on livro.Categoria equals categoria.Nome
                           select new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria))
                           .Select((produto, indice) =>
                           {
                               produto.Id = indice + 1;
                               
                               return produto;
                           })
                           .ToList();

            return produtos;
        }
    }
}
