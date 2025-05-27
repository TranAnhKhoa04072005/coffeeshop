using coffeeshop.Models.Interface;
using Microsoft.AspNetCore.Mvc;

namespace coffeeshop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Sẽ tìm View Contact.cshtml trong Views/Contact
        }
    }
}

