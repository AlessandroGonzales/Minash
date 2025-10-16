using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class MinashDbContextFactory : IDesignTimeDbContextFactory<MinashDbContext>
{
    public MinashDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MinashDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=minash;Username=postgres;Password=Jamancapiero85.");

        return new MinashDbContext(optionsBuilder.Options);
    }
}
