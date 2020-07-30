using CasaDoCodigo.Models;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repository.Interfaces
{
    public interface IRelatorioHelper
    {
        Task GerarRelatorio(Pedido pedido);
    }
}
