using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productservice.Data;
using productservice.Model;

namespace productservice.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "productscache")]
        public async Task<IActionResult> Get([FromQuery]int startat, [FromQuery]int quantity)
        {

            if (quantity == 0)
                quantity = 50;
            var products = await _context.Products
            .Where(x => x.enabled == true)
            .OrderBy(x => x.productId)
            .Skip(startat)
            .Take(quantity)
            .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ResponseCache(CacheProfileName = "productscache")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products
            .OrderBy(x => x.productId)
            .Where(x => x.productId == id)
            .FirstOrDefaultAsync();
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            product.productId = 0;
            product.parentProducId = null;
            product.childrenProductsIds = new int[0];

            if (ModelState.IsValid)
            {
                await _context.AddAsync(product);
                await _context.SaveChangesAsync();

                return Created($"api/products/{product.productId}", product);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Product product)
        {
            if (ModelState.IsValid)
            {
                var curTProduct = await _context.Products
                .AsNoTracking()
                .Where(x => x.productId == id)
                .FirstOrDefaultAsync();

                product.childrenProductsIds = curTProduct.childrenProductsIds;
                product.parentProducId = product.parentProducId;
                if (id != product.productId)
                {
                    return NotFound();
                }
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return NoContent();

            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
            .Where(x => x.productId == id)
            .FirstOrDefaultAsync();

            if (product != null)
            {
                product.enabled = false;
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
    }

}