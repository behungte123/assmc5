using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers
{
    public class AccountController : Controller
    {
        // Action xử lý khi người dùng bị từ chối truy cập do thiếu Claim [1], [2]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}