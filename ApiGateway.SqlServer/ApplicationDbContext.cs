using ApiGateway.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

internal class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :  base(options)
    {

    }

    public DbSet<Usuario> Usuarios { get; set; }
}