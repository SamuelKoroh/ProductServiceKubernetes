using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProductsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] Filter filter)
        {
            _dataContext.Database.Migrate();

            if (!_dataContext.Products.Any())
            {
                var productsList = new List<Product>
                {
                    new Product{  Name = "T-Shirt", Price= 200, Description="nice product"},
                    new Product{  Name = "Bags", Price= 240, Description="nice product"},
                    new Product{  Name = "Clothes", Price= 100, Description="nice product"},
                    new Product{  Name = "T-Shirt", Price= 500, Description="nice product"}
                };
                _dataContext.Products.AddRange(productsList);
                await _dataContext.SaveChangesAsync();
            }
            
            var products = await _dataContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _dataContext.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dataContext.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

             _dataContext.Products.Remove(product);
             await _dataContext.SaveChangesAsync();

            return Ok(product);
        }

        public class Filter {
            public string Name { get; set; }
        }
    }
}
