using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC
{
    public interface IDataService
    {
        Task InicializaDBAsync(IServiceProvider provider);
    }
}
