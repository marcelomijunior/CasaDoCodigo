﻿using CasaDoCodigo.MVC.Models;
using CasaDoCodigo.MVC.Repository;
using CasaDoCodigo.MVC.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC
{
    public class DataService : IDataService
    {
        public async Task InicializaDBAsync(IServiceProvider provider)
        {
            var contexto = provider.GetService<ApplicationContext>();

            await contexto.Database.MigrateAsync();

            if (await contexto.Set<Produto>().AnyAsync())
            {
                return;
            }

            List<Livro> livros = await GetLivrosAsync();

            var produtoRepository = provider.GetService<IProdutoRepository>();
            await produtoRepository.SaveProdutosAsync(livros);
        }

        private async Task<List<Livro>> GetLivrosAsync()
        {
            var json = await File.ReadAllTextAsync("Livros.json");
            return JsonConvert.DeserializeObject<List<Livro>>(json);
        }
    }
}
