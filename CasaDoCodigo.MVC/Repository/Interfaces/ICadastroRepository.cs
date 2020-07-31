using CasaDoCodigo.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Repository.Interfaces
{
    public interface ICadastroRepository
    {
        Task<Cadastro> UpdateAsync(int cadastroId, Cadastro novoCadastro);
    }
}
