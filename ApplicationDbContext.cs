using Microsoft.EntityFrameworkCore;
//Utilizando Entity para as classes virarem tabela e as propriedades do banco de dados
public class ApplicationDbContext : DbContext
{
    public DbSet<Address> Address { get; set; }
    public DbSet<Client> Client { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Address>()
           .Property(c => c.city).HasMaxLength(100).IsRequired();
        builder.Entity<Address>()
           .Property(s => s.state).HasMaxLength(100).IsRequired();
    }
}