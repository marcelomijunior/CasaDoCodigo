using CasaDoCodigo.MVC.Models;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Repository.Interfaces
{
    public interface IRelatorioHelper
    {
        Task GerarRelatorio(Pedido pedido);
    }
}
