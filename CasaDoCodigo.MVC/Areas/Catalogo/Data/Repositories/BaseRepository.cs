using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Data.Repositories
{
    public class BaseRepository<T> where T : BaseModel
    {
        protected readonly IConfiguration configuration;
        protected readonly CatalogoDbContext catalogoDbContext;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(IConfiguration configuration, CatalogoDbContext catalogoDbContext)
        {
            this.configuration = configuration;
            this.catalogoDbContext = catalogoDbContext;
            this.dbSet = catalogoDbContext.Set<T>();
        }
    }
}
