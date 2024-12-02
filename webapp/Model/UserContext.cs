using Microsoft.EntityFrameworkCore;
using webapp.Model;

public class UserContext : DbContext
{

    public DbSet<User> Users { get; set; }


    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}
