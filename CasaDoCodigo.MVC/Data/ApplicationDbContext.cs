using CasaDoCodigo.MVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pedido>(builderAction =>
            {
                builderAction.HasKey(p => p.Id);
                builderAction.HasMany(p => p.Itens).WithOne(i => i.Pedido);
                builderAction.HasOne(p => p.Cadastro).WithOne(c => c.Pedido).IsRequired();
            });

            modelBuilder.Entity<ItemPedido>(builderAction =>
            {
                builderAction.HasKey(ip => ip.Id);
                builderAction.HasOne(ip => ip.Pedido).WithMany(p => p.Itens);
            });
        }
    }
}
