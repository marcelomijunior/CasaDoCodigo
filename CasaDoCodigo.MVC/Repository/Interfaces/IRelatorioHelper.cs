using CasaDoCodigo.Models;
using System.Threading.Tasks;

namespace CasaDoCodigoMVC.Repository.Interfaces
{
    public interface IRelatorioHelper
    {
        Task GerarRelatorio(Pedido pedido);
    }
}
