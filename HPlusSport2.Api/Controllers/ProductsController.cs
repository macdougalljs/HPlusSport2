using HPlusSport2.Api.Classes;
using HPlusSport2.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport2.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //  [HttpGet]     
        //  public IEnumerable<Product> GetAllProducts()
        //  {
        //      return _context.Products.ToArray();
        //  }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            IQueryable<Product> products = _context.Products; // Iqueryable is important because by default it thinks a DbSet

            if (queryParameters.MinPrice != null && queryParameters.MaxPrice != null)
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value &&
                p.Price <= queryParameters.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
            {
                products = products.Where(p => p.Sku.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
               p.Name.ToLower().Contains(queryParameters.SearchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(p => p.Sku == queryParameters.Sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(
                    p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }

            products = products
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(await products.ToArrayAsync());
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
