using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class EndoMetricsContext: DbContext
{
    public EndoMetricsContext(DbContextOptions<EndoMetricsContext> options) : base(options)
    { }  

    public DbSet<User> Users { get; set; }

}