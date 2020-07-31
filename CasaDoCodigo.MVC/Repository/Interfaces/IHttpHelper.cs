using CasaDoCodigo.MVC.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Repository.Interfaces
{
    public interface IHttpHelper
    {
        IConfiguration Configuration { get; }
        int? GetPedidoId();
        void SetPedidoId(int pedidoId);
        void ResetPedidoId();
    }
}
