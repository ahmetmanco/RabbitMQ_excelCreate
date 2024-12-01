namespace RabbitMQ_excelCreate.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) 
        {
            
        }
        public DbSet<UserFile> UserFiles { get; set; }
    }
}
