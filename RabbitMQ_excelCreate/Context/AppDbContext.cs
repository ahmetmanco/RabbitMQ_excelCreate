namespace RabbitMQ_excelCreate.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) 
        {
            
        }
        public DbSet<UFiles> UserFiles { get; set; }
    }
}
