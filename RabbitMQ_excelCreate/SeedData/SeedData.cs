namespace RabbitMQ_excelCreate.SeedData
{
    public static class SeedData
    {
        public static async Task SeedAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                await SeedUsersAsync(userManager, dbContext);

                dbContext.Database.Migrate();

                await SeedUsersAsync(userManager, dbContext);
            }
        }



        private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager, AppDbContext dbContext)
        {

            if (!dbContext.Users.Any())
            {
                await userManager.CreateAsync(new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com"
                }, "Test123.");


                await userManager.CreateAsync(new IdentityUser()
                {
                    UserName = "Member",
                    Email = "member@gmail.com",
                }, "Test123.");

            }
            else
            {
                Console.WriteLine("Kullanıcılar zaten mevcut.");
            }
        }
    }
}
