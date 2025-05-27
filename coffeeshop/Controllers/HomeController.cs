using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using coffeeshop.Models;
using coffeeshop.Models.Interface;

namespace TH01.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository productRepository;
        public HomeController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public IActionResult Index()
        { 
            return View(productRepository.GetAllProducts());
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }

    }
}
