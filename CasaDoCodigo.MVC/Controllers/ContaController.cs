using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasaDoCodigo.Controllers
{
    public class ContaController : Controller
    {
        [Authorize]
        public async Task<ActionResult> Login()
        {
            return Redirect(Url.Content("~/"));
        }

        [Authorize]
        public async Task Logout()
        {
            // atualizar cookies (autenticacao local)
            await HttpContext.SignOutAsync("Cookies");
            // desconexão identity server
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
