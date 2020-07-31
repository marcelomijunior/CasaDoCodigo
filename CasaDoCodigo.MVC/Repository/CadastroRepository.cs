using CasaDoCodigo.MVC.Models;
using CasaDoCodigo.MVC.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Repository
{
    public class CadastroRepository : BaseRepository<Cadastro>, ICadastroRepository
    {
        public CadastroRepository(IConfiguration configuration,
            ApplicationContext contexto) : base(configuration, contexto)
        {

        }

        public async Task<Cadastro> UpdateAsync(int cadastroId, Cadastro novoCadastro)
        {
            var cadastroDB = dbSet.Where(c => c.Id == cadastroId)
                .SingleOrDefault();

            if (cadastroDB == null)
            {
                throw new ArgumentNullException("cadastro");
            }

            cadastroDB.Update(novoCadastro);
            await contexto.SaveChangesAsync();
            return cadastroDB;
        }
    }
}
