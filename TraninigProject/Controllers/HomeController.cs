using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraninigProject.Models;

namespace TraninigProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository Productrepository;

        public HomeController(IProductRepository _productrepository)
        {
            Productrepository = _productrepository;
        }

        public IActionResult Index(string Category)
        {
            var ProductList = Productrepository.GetALLProducts(Category);
            return View(ProductList);
        }
        [HttpPost]
        public IActionResult Serach(string Category)
        {
            return Index(Category);
        }

        
        public IActionResult Details(int pid)
        {
            Product pro = Productrepository.GetProduct(pid);
            return View(pro);
        }

        [HttpPost]
        public IActionResult AddOrderLine(int ProductId, int Count)
        {  
            if (Count > 0)
            {
                Cart cart = HttpContext.ReadFromSession<Cart>("Cart");
                List<OrderLine> LOL = new List<OrderLine>();
                //  OrderLine nol = new OrderLine() {Count=Count ,OrderLineItem= Productrepository.GetProduct(ProductId) };
                OrderLine ol = null;
                if (cart == null)
                {
                    cart = new Cart();
                }
                if (cart.Orders != null)
                {
                    LOL = cart.Orders;
                    ol = LOL.Where(x => x.OrderLineItem.ProductId == ProductId).FirstOrDefault();
                }
                if (ol != null)
                {
                    LOL.Remove(ol);
                    ol.Count += Count;
                }
                else
                {
                    ol = new OrderLine() { Count = Count, OrderLineItem = Productrepository.GetProduct(ProductId) };
                }
                LOL.Add(ol);
                cart.Orders = LOL;
                HttpContext.WriteToSession("Cart", cart);
                return RedirectToAction("ViewCart");

            }
            else
            {
                ModelState.AddModelError("Count", "تعداد نمی تواند صفر باشد.");
                 return RedirectToAction("Details", new { pid = ProductId });
            }         
            

        }
       
        public IActionResult DeleteOrderLine(int pid)
        {
            Cart cart = HttpContext.ReadFromSession<Cart>("Cart");
            List<OrderLine> LOL = cart.Orders;
            OrderLine ol = LOL.Where(x => x.OrderLineItem.ProductId == pid).First();
            LOL.Remove(ol);
            cart.Orders = LOL;
            HttpContext.WriteToSession("Cart", cart);
            return RedirectToAction("ViewCart");
        }

        public IActionResult ViewCart()
        {
            Cart cart = HttpContext.ReadFromSession<Cart>("Cart");            
            return View(cart);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
