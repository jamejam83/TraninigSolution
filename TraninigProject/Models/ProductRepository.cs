using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TraninigProject.Models
{
    public class ProductRepository : IProductRepository
    {
        private  ShopDBContext _Dbcontext=new ShopDBContext();     

        public List<Product> GetALLProducts(string Category)
        { 
            if(String.IsNullOrEmpty(Category))
            {
                return _Dbcontext.Products.Include(p => p.Images).ToList();
            }
            else
            return _Dbcontext.Products.Where(p => p.Category == Category).Include(p => p.Images).ToList();
        }

        public Product GetProduct(int ProductId)
        {
            return _Dbcontext.Products.Where(x=>x.ProductId==ProductId).Include(p=>p.Images).FirstOrDefault();
        }
    }
}
