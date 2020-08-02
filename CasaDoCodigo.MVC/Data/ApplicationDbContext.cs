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

            modelBuilder.Entity<Pedido>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<Pedido>()
                .HasMany(t => t.Itens)
                .WithOne(t => t.Pedido);
            modelBuilder.Entity<Pedido>()
                .HasOne(t => t.Cadastro)
                .WithOne(t => t.Pedido)
                .IsRequired();

            modelBuilder.Entity<ItemPedido>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<ItemPedido>()
                .HasOne(t => t.Pedido);
        }
    }
}
