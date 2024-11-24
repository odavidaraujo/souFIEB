using Microsoft.EntityFrameworkCore;

namespace Soufieb.Webapp.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Informatives> Informatives { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<Cardapio> Cardapio { get; set; }
        public DbSet<Leitura> Leitura { get; set; }
    }
}
