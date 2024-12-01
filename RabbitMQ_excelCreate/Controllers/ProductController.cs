using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ_excelCreate.Service;

namespace RabbitMQ_excelCreate.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public ProductController(UserManager<IdentityUser> userManager, AppDbContext appDbContext, RabbitMQPublisher rabbitMQPublisher)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateProductExcel()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";

            UserFile file = new UserFile()
            {
                UserId = user.Id,
                FileName = fileName,
                FileStatus = FileStatus.Creating
            };
            await _appDbContext.UserFiles.AddAsync(file);

            await _appDbContext.SaveChangesAsync();

            _rabbitMQPublisher.Publish(new ExcelMessage.CreateExcelMessage()
            {
                FileId = file.Id,
            });

            TempData["StartCreatingExcel"] = true;
            return RedirectToAction(nameof(Files));
        }

        public async Task<IActionResult> Files()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(await _appDbContext.UserFiles.Where(x=>x.UserId == user.Id).ToListAsync());
        }
    }
}
