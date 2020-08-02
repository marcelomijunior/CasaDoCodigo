using CasaDoCodigo.MVC.Areas.Catalogo.Data.Repositories.Interfaces;
using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using CasaDoCodigo.MVC.Areas.Identity.Data;
using CasaDoCodigo.MVC.Data;
using CasaDoCodigo.MVC.Models;
using CasaDoCodigo.MVC.Models.ViewsModel;
using CasaDoCodigo.MVC.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Repository
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IHttpHelper httpHelper;
        private readonly ICadastroRepository cadastroRepository;
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly IRelatorioHelper relatorioHelper;
        private readonly IProdutoRepository produtoRepository;

        public PedidoRepository(
            IConfiguration configuration,
            ApplicationDbContext contexto,
            IHttpContextAccessor contextAccessor,
            IHttpHelper sessionHelper,
            ICadastroRepository cadastroRepository,
            UserManager<AppIdentityUser> userManager,
            IRelatorioHelper relatorioHelper,
            IProdutoRepository produtoRepository
            ) : base(configuration, contexto)
        {
            this.contextAccessor = contextAccessor;
            this.httpHelper = sessionHelper;
            this.cadastroRepository = cadastroRepository;
            this.userManager = userManager;
            this.relatorioHelper = relatorioHelper;
            this.produtoRepository = produtoRepository;
        }

        public async Task AddItemAsync(string codigo)
        {
            var produto = await produtoRepository.GetProdutoAsync(codigo);

            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado");
            }

            var pedido = await GetPedidoAsync();

            var itemPedido = await contexto
                .Set<ItemPedido>()
                .Where(i => i.CodigoProduto == codigo && i.Pedido.Id == pedido.Id)
                .SingleOrDefaultAsync();

            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto.Codigo, produto.Nome, 1, produto.Preco);
                
                await contexto.Set<ItemPedido>().AddAsync(itemPedido);
                await contexto.SaveChangesAsync();
            }
        }

        public async Task<Pedido> GetPedidoAsync()
        {
            var pedidoId = httpHelper.GetPedidoId();
            var pedido = await dbSet
                .Include(p => p.Itens)
                .Include(p => p.Cadastro)
                .Where(p => p.Id == pedidoId)
                .SingleOrDefaultAsync();

            if (pedido == null)
            {
                var claimsPrincipal = contextAccessor.HttpContext.User;
                var clienteId = userManager.GetUserId(claimsPrincipal);

                pedido = new Pedido(clienteId);
                
                await dbSet.AddAsync(pedido);
                await contexto.SaveChangesAsync();
                
                httpHelper.SetPedidoId(pedido.Id);
            }

            return pedido;
        }

        public async Task<UpdateQuantidadeResponse> UpdateQuantidadeAsync(ItemPedido itemPedido)
        {
            var itemPedidoDB = await GetItemPedidoAsync(itemPedido.Id);

            if (itemPedidoDB != null)
            {
                itemPedidoDB.AtualizaQuantidade(itemPedido.Quantidade);

                if (itemPedido.Quantidade == 0)
                {
                    await RemoveItemPedidoAsync(itemPedido.Id);
                }

                await contexto.SaveChangesAsync();

                var pedido = await GetPedidoAsync();
                var carrinhoViewModel = new CarrinhoViewModel(pedido.Itens);

                return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
            }

            throw new ArgumentException("ItemPedido não encontrado");
        }

        public async Task<Pedido> UpdateCadastroAsync(Cadastro cadastro)
        {
            var pedido = await GetPedidoAsync();
            await cadastroRepository.UpdateAsync(pedido.Cadastro.Id, cadastro);
            
            httpHelper.ResetPedidoId();

            return pedido;
        }

        private async Task<ItemPedido> GetItemPedidoAsync(int itemPedidoId)
        {
            return await contexto.Set<ItemPedido>()
                .Where(ip => ip.Id == itemPedidoId)
                .SingleOrDefaultAsync();
        }

        private async Task RemoveItemPedidoAsync(int itemPedidoId)
        {
            contexto.Set<ItemPedido>().Remove(await GetItemPedidoAsync(itemPedidoId));
        }
    }
}
